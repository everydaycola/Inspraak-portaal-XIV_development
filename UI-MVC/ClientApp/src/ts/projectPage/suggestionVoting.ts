import {getCurrentBaseUrl} from "../customHelpers/locationHelper";
import {fetchFromAPI} from "../customHelpers/apihelper";

interface VoteState {
    [key: string]: {
        currentlyPressedBtn: HTMLButtonElement | null;
    };
}

const voteStates: VoteState = {};

export function initVoteHandler() {
    const voteButtons = Array.from(document.querySelectorAll('.vote-btn')) as HTMLButtonElement[];

    voteButtons.forEach(button => {
        const suggestionId = button.dataset.suggestionId;
        if (!suggestionId) {
            console.error('Button is missing suggestion ID:', button);
            return;
        }

        if (!voteStates[suggestionId]) {
            voteStates[suggestionId] = {
                currentlyPressedBtn: null
            };
        }

        if (button.classList.contains('active')) {
            voteStates[suggestionId].currentlyPressedBtn = button;
        }

        console.log(suggestionId)

        button.addEventListener('click', () => handleVote(button, suggestionId));
    });
}

async function handleVote(button: HTMLButtonElement, suggestionId: string) {
    const currentState = voteStates[suggestionId];
    if (!currentState) return;

    const oldButton = currentState.currentlyPressedBtn;

    try {
        // Send vote to API
        const response = await fetch('/api/votes', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                "SuggestionId": suggestionId,
                "Type": button.dataset.voteType
            })
        });

        if (response.status === 404 || response.status == 401) {
            return;
        }

        if (response.status !== 204) {
            console.error('Unexpected response status:', response.status);
        }

        // Toggle behavior, if clicking the same button, remove vote
        if (oldButton === button) {
            currentState.currentlyPressedBtn = null;
            button.classList.remove('active');

            const countElement = button.querySelector('.vote-count');
            if (countElement) {
                countElement.textContent = Math.max(0, parseInt(countElement.textContent || '0') - 1).toString();
            }

            return;
        }

        // Remove old vote if exists
        if (oldButton) {
            oldButton.classList.remove('active');

            const oldCountElement = oldButton.querySelector('.vote-count');
            if (oldCountElement) {
                oldCountElement.textContent = Math.max(0, parseInt(oldCountElement.textContent || '0') - 1).toString();
            }
        }

        // Add new vote
        currentState.currentlyPressedBtn = button;
        button.classList.add('active');

        const newCountElement = button.querySelector('.vote-count');
        if (newCountElement) {
            newCountElement.textContent = (parseInt(newCountElement.textContent || '0') + 1).toString();
        }
    } catch (error) {
        console.error('Failed to send vote:', error);
    }
}

document.querySelectorAll("form.end-vote-form").forEach((form) => {
    form.addEventListener("submit", handleVoteToggle);
});

async function handleVoteToggle(event: Event) {
    event.preventDefault();

    const form = event.target as HTMLFormElement;
    const postId = (form.querySelector('input[name="postId"]') as HTMLInputElement).value

    try {
        const baseUrl = getCurrentBaseUrl()
        const response = await fetchFromAPI<{
            success: boolean
        }>(`${baseUrl}/api/PanelProjectPages/toggleVoting?postId=${postId}`,
            {method: "POST"})

        console.log(response)

        if (response.success) {
            window.location.reload()
        } else {
            console.error("Failed to toggle voting.");
        }
    } catch (err) {
        console.error("Error:", err);
    }

    return false; // Prevent default submit
}

async function ShowVotePercentages(postId: string) {
    const post = findSuggestionPost(postId) as HTMLDivElement
    const suggestionDivs = post.getElementsByClassName("suggestion") as HTMLCollectionOf<HTMLDivElement>;
    const suggestionItems = post.getElementsByClassName("suggestion-item") as HTMLCollectionOf<HTMLDivElement>;
    const majorityFactor = parseFloat(post.dataset.majorityFactor as string) 
    
    console.log(`Voting on post ${postId} stopped`)
    
    let highestPercentage: number = 0;
    let highestIndex: number = -1;

    for (let i = 0; i < suggestionDivs.length; i++) {
        const suggestionId = suggestionDivs.item(i)!!.querySelector("input")!!.value
        const percentage:number = await CalculateVotePercentage(suggestionId, postId)

        const percentageDiv = suggestionItems.item(i)!!.querySelector("div.vote-percentage") as HTMLDivElement;
        percentageDiv.querySelector("span")!!.innerText = `${percentage.toFixed(2)}%`
        
        if (percentage > highestPercentage){
            highestPercentage = percentage;
            highestIndex = i;
        }
    }

    if (highestIndex !== -1) {
        if (highestPercentage >= majorityFactor) {
            const percentageDiv = suggestionItems.item(highestIndex)!!.querySelector("div.vote-percentage") as HTMLDivElement;
            percentageDiv.querySelector("span")!!.classList.add("text-success")
        }
    }
}

async function CalculateVotePercentage(suggestionId: string, postId: string) {
    const baseUrl = getCurrentBaseUrl();
    const response = await fetch(`${baseUrl}/api/PanelProjectPages/votePercentage?suggestionId=${suggestionId}&postId=${postId}`);
    return await response.json();
}

function findSuggestionPost(postId: string): HTMLDivElement | null{
    const suggestionPosts = document.getElementsByClassName("suggestion-post") as HTMLCollectionOf<HTMLDivElement>
    for (let i = 0; i < suggestionPosts.length; i++) {
        if (suggestionPosts[i].dataset.postId == postId){
            return suggestionPosts[i]
        }
    }
    console.error(`Suggestion Post with if ${postId} not fond`)
    return null;
}

window.addEventListener("load", () => {
    const suggestionPosts = document.getElementsByClassName("suggestion-post") as HTMLCollectionOf<HTMLDivElement>
    
    for (let i = 0; i < suggestionPosts.length; i++) {
        const votingOpen = suggestionPosts[i].dataset.isVotingOpen === "True";
        if (!votingOpen) {
            const postId = suggestionPosts[i].dataset.postId as string;
            ShowVotePercentages(postId);
        }
    }
})

