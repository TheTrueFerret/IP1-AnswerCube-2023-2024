
export function RemoveLastDirectoryPartOf(the_url: string)
{
    var the_arr = the_url.split('/');
    the_arr.pop();
    return( the_arr.join('/') );
}
