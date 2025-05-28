// functions could be used for saving data on the session, not currently used. 

function setSessionItem(key: string, value: any): void {
    try {
        const serializedValue = JSON.stringify(value);
        sessionStorage.setItem(key, serializedValue);
    } catch (error) {
        console.error("Error setting sessionStorage item:", error);
    }
}
function getSessionItem<T>(key: string): T | null {
    try {
        const item = sessionStorage.getItem(key);
        return item ? JSON.parse(item) as T : null;
    } catch (error) {
        console.error("Error retrieving sessionStorage item:", error);
        return null;
    }
}
function removeSessionItem(key: string): void {
    try {
        sessionStorage.removeItem(key);
    } catch (error) {
        console.error("Error removing sessionStorage item:", error);
    }
}
