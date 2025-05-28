// apiClient.ts

import {getCurrentBaseUrl} from "../../customHelpers/locationHelper";
import {fetchFromAPI} from "../../customHelpers/apihelper";

export async function fetchCommunes(){
    const endpointuri = getCurrentBaseUrl() + "/api/Communes";
    return await fetchFromAPI(endpointuri);
}