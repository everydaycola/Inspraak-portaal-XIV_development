import '@popperjs/core';
import 'bootstrap';
import 'bootstrap-icons/font/bootstrap-icons.css';
import 'bootstrap/dist/css/bootstrap.css';

// Custom CSS imports
import '../scss/site.scss'
import '../scss/register.scss'
import '../scss/customnavbar.scss'
import {setupNavBarHover} from "./navbar/navbar";

// Custom Ts

console.log('The \'site\' bundle has been loaded!');
setupNavBarHover()
