// apiClient.ts
async function fetchFromAPI<T>(
    url: string,
    options?: RequestInit
): Promise<T> {
    try {
        const response = await fetch(url, {
            headers: {
                'Content-Type': 'application/json',
                ...options?.headers,
            },
            ...options,
        });

        if (!response.ok) {
            throw new Error(`API error: ${response.status} ${response.statusText}`);
        }

        const data: T = await response.json();
        return data;
    } catch (error) {
        console.error('Fetch error:', error);
        throw error;
    }
}
function getCurrentBaseUrl(): string {
    return `${window.location.protocol}//${window.location.hostname}${window.location.port ? `:${window.location.port}` : ''}`;
}
export async function fetchCommunes(){
    const endpointuri = getCurrentBaseUrl() + "/api/Commune";
    return await fetchFromAPI(endpointuri);
}
export async function fetchBasicCommunes(){
    const endpointuri = getCurrentBaseUrl() + "/api/Commune/basic";
    return await fetchFromAPI(endpointuri);
}