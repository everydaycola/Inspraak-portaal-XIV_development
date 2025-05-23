// apiClient.ts

import {getCurrentBaseUrl} from "../../customhelpers/locationHelper";
import {fetchFromAPI} from "../../customhelpers/apihelper";

export async function fetchCommunes(){
    const endpointuri = getCurrentBaseUrl() + "/api/Communes";
    return await fetchFromAPI(endpointuri);
}