
export async function fetchEndpoint(url: string) {
    let msg: String = await fetch(url)
        .then(response => response.json())
    
    return msg;
}