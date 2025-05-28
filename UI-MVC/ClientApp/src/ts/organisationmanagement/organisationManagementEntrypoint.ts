import {configureSearch} from "./subcomponents/organisationsearch";
import {configureEdit} from "./subcomponents/organisationEdit";
import {configureDelete} from "./subcomponents/organisationDelete";


console.log("Admin organisation panel entrypoint loaded.");
configureSearch();
configureEdit();
configureDelete();