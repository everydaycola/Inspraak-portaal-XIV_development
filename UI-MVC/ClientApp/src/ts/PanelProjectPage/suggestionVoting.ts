interface VoteState {
    [key: string]: {
        currentlyPressedBtn: HTMLButtonElement | null;
    };
}

const voteStates: VoteState = {};

document.addEventListener('DOMContentLoaded', () => {
    
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
});

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