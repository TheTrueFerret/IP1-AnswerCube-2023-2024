import '@popperjs/core';
import 'bootstrap';
import 'bootstrap-icons/font/bootstrap-icons.css';
import 'bootstrap/dist/css/bootstrap.css';

// Custom JS imports
// ... none at the moment

// Custom CSS imports
import '../scss/site.scss';

export function RemoveLastDirectoryPartOf(the_url: string)
{
    var the_arr = the_url.split('/');
    the_arr.pop();
    return( the_arr.join('/') );
} 
console.log('The \'site\' bundle has been loaded!');

