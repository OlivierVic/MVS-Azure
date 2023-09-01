/**
 * Change any case into js case
 * @param item: an object to change case
 * @param keyRenamingMapping: an object to do some key name changes
 */
function changeCase(item, keyRenamingMapping = {}) {
    function camelize(str) {
        return str.replace(/(?:^\w|[A-Z]|\b\w)/g, function (word, index) {
            return index === 0 ? word.toLowerCase() : word.toUpperCase();
        }).replace(/\s+/g, '');
    }

    for (var key in item) {
        var camelizeKey = camelize(key);

        if (keyRenamingMapping.hasOwnProperty(camelizeKey)) {
            camelizeKey = keyRenamingMapping[camelizeKey];
        }

        item[camelizeKey] = item[key];
        if (Array.isArray(item[camelizeKey])) {
            item[camelizeKey].map(x => changeCase(x, keyRenamingMapping));
        }

        delete item[key];
    }
    return item;
}

/**
 * Download a file fetched from an ajax call
 * @param {any} data: the data to save, fetched from ajax call
 * @param {string} fileName: the name wanted for our file
 */
function downloadFileFromAjax(data, fileName) {
    var element = document.createElement("a");
    var url = window.URL.createObjectURL(data);

    element.href = url;
    element.download = fileName;
    document.body.append(element);

    element.click();
    element.remove();

    window.URL.revokeObjectURL(url);
}

/**
 * Get the file name stored in the Content-Disposition response header
 * @param {jqXHR} xhr: the XHR from the response
 * @returns {string} the filename
 */
function getFileNameFromXHR(xhr) {
    var disposition = xhr.getResponseHeader("content-disposition");
    if (disposition && disposition.indexOf("attachment") !== -1) {
        var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
        var matches = filenameRegex.exec(disposition);
        if (matches != null && matches[1]) {
            return matches[1].replace(/['"]/g, '');
        }
    }
    return "filename";
}

/**
 * Generate a GUID
 * @returns {string} the generated guid
 */
function generateGuid() {
    // Since html ids must start with a letter, our GUIDs starts with `f`
    return "fxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0;
        var v = c === "x" ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

/**
 *
 * @param obj
 * @param strPath
 * @returns {*} the value in the object at the path
 */
Object.byString = function (obj, strPath) {
    if (strPath === null || strPath === undefined) {
        return obj;
    }
    strPath = strPath.replace(/\[(\w+)]/g, '.$1');
    strPath = strPath.replace(/^\./, '');
    var paths = strPath.split(".");
    for (var i = 0, n = paths.length; i < n; ++i) {
        var key = paths[i];
        if (key in obj) {
            obj = obj[key];
        } else {
            return;
        }
    }
    return obj;
}

/**
 * Truncate a string and add (...) if too long
 * @param {string} str
 * @param {number} n the max number of char
 * @return {string} the string truncated
 */
function truncate(str, n) {
    return (str.length > n) ? str.substr(0, n - 1) + "..." : str;
}

/**
 * Calls handler with the click event when the user clicks outside the selector
 * @param selector a selector for jQuery (can be an element)
 * @param handler a function that takes the click event
 * @return the event listener
 */
function onClickOutside(selector, handler, options) {
    const listener = (event) => {
        const $target = $(event.target);
        if (!$target.closest(selector).length) {
            handler(event);
        }
    };

    if (options?.one) {
        $(document).one('click', listener);
    } else {
        $(document).on('click', listener);
    }

    return listener;
}

/**
 * Set a cookie
 * @param {string} cookieName
 * @param {string} cookieValue
 * @param {number} expirationDays
 */
function setCookie(cookieName, cookieValue, expirationDays) {
    var date = new Date();
    date.setTime(date.getTime() + (expirationDays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + date.toUTCString();
    document.cookie = `${cookieName}=${cookieValue};${expires};path=/`;
}

/**
 * Remove the combobox generated by bootstrap-combobox
 * @param selector A jQuery selector
 */
function teardownCombobox(selector) {
    const $elements = $(selector);
    const comboboxData = $elements.data('combobox');
    comboboxData?.$container.remove();
    $elements.removeData('combobox');
}

/**
 * Returns a list of pages used for pagination
 * @param page the current page number
 * @param count the max page number
 * @returns {unknown[]} the list of page numbers
 */
function getPagesList(page, count) {
    const boundaryCount = 2;
    const siblingCount = 1;

    const range = (start, end) => {
        const length = end - start + 1;
        return Array.from({ length }, (_, i) => start + i);
    };
    const startPages = range(1, Math.min(boundaryCount, count));
    const endPages = range(Math.max(count - boundaryCount + 1, boundaryCount + 1), count);
    const siblingsStart = Math.max(
        Math.min(
            page - siblingCount,
            count - boundaryCount - siblingCount * 2 - 1,
        ),
        boundaryCount + 2,
    );
    const siblingsEnd = Math.min(
        Math.max(
            page + siblingCount,
            boundaryCount + siblingCount * 2 + 2,
        ),
        endPages[0] - 2,
    );
    const itemList = [
        ...startPages,
        ...(siblingsStart > boundaryCount + 2
            ? ['...']
            : boundaryCount + 1 < count - boundaryCount
                ? [boundaryCount + 1]
                : []),
        ...range(siblingsStart, siblingsEnd),
        ...(siblingsEnd < count - boundaryCount - 1
            ? ['...']
            : count - boundaryCount > boundaryCount
                ? [count - boundaryCount]
                : []),
        ...endPages,
    ];
    return itemList;
}
