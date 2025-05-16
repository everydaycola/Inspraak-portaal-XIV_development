// apiClient.ts
import {getCurrentBaseUrl} from "../../helpers/locationHelper";
import {fetchFromAPI} from "../../helpers/apihelper";

export async function fetchCommunes(){
    const endpointuri = getCurrentBaseUrl() + "/api/Commune";
    return await fetchFromAPI(endpointuri);
}