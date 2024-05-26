
export function RemoveLastDirectoryPartOf(the_url: string): string
{
    var the_arr = the_url.split('/');
    the_arr.pop();
    return( the_arr.join('/') );
}

export function getDomainFromUrl(the_url: string): string
{
    const url = new URL(the_url);
    const portString = url.port ? `:${url.port}` : '';
    return `${url.protocol}//${url.hostname}${portString}`;
}

export function getControllerNameFromUrl(the_url: string): string | null {
    const urlParts = the_url.split('/');
    // Check if the URL has at least two segments
    if (urlParts.length < 2) {
        return null;
    }
    // Extract domain (first segment) and controller (second segment)
    const controller = urlParts[1];
    return controller;
}
