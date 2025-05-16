// apiClient.ts
import {getCurrentBaseUrl} from "../../helpers/locationHelper";
import {fetchFromAPI} from "../../helpers/apihelper";

export async function fetchCommunes(){
    const endpointuri = getCurrentBaseUrl() + "/api/Commune";
    return await fetchFromAPI(endpointuri);
}
export async function fetchBasicCommunes(){
    const endpointuri = getCurrentBaseUrl() + "/api/Commune/basic";
    return await fetchFromAPI(endpointuri);
}