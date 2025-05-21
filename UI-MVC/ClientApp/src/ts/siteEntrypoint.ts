import '@popperjs/core';
import 'bootstrap';
import 'bootstrap-icons/font/bootstrap-icons.css';
import 'bootstrap/dist/css/bootstrap.css';

// Custom CSS imports
import '../scss/site.scss'
import '../scss/register.scss'
import '../scss/customnavbar.scss'
import '../scss/bootstrapOverwrite.scss'

// Custom Ts
import {configureAutoOpeningModals} from "./customhelpers/bootstrapAutoModalOpeningHelper";

console.log('The \'site\' bundle has been loaded!');
configureAutoOpeningModals()
