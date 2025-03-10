import '@popperjs/core';
import 'bootstrap';
import 'bootstrap-icons/font/bootstrap-icons.css';
import 'bootstrap/dist/css/bootstrap.css';

// Custom CSS imports
import '../css/site.css'

// Custom Ts
import {log} from "./Panel/makeNewPanel"

console.log('The \'site\' bundle has been loaded!');

if (window.location.href.endsWith("MakeNewPanel")) {
    log()
}
