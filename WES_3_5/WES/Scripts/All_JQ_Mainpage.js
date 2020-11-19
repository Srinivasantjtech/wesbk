/*!
* jQuery JavaScript Library v1.4.1
* http://jquery.com/
*
* Copyright 2010, John Resig
*
* Includes Sizzle.js
* http://sizzlejs.com/
* Copyright 2010, The Dojo Foundation
*
* Permission is hereby granted, free of charge, to any person obtaining
* a copy of this software and associated documentation files (the
* "Software"), to deal in the Software without restriction, including
* without limitation the rights to use, copy, modify, merge, publish,
* distribute, sublicense, and/or sell copies of the Software, and to
* permit persons to whom the Software is furnished to do so, subject to
* the following conditions:
* 
* The above copyright notice and this permission notice shall be
* included in all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
* MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
* NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
* LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
* OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
* WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
* Date: Mon Jan 25 19:43:33 2010 -0500
*/
(function (window, undefined) {

    // Define a local copy of jQuery
    var jQuery = function (selector, context) {
        // The jQuery object is actually just the init constructor 'enhanced'
        return new jQuery.fn.init(selector, context);
    },

    // Map over jQuery in case of overwrite
	_jQuery = window.jQuery,

    // Map over the $ in case of overwrite
	_$ = window.$,

    // Use the correct document accordingly with window argument (sandbox)
	document = window.document,

    // A central reference to the root jQuery(document)
	rootjQuery,

    // A simple way to check for HTML strings or ID strings
    // (both of which we optimize for)
	quickExpr = /^[^<]*(<[\w\W]+>)[^>]*$|^#([\w-]+)$/,

    // Is it a simple selector
	isSimple = /^.[^:#\[\.,]*$/,

    // Check if a string has a non-whitespace character in it
	rnotwhite = /\S/,

    // Used for trimming whitespace
	rtrim = /^(\s|\u00A0)+|(\s|\u00A0)+$/g,

    // Match a standalone tag
	rsingleTag = /^<(\w+)\s*\/?>(?:<\/\1>)?$/,

    // Keep a UserAgent string for use with jQuery.browser
	userAgent = navigator.userAgent,

    // For matching the engine and version of the browser
	browserMatch,

    // Has the ready events already been bound?
	readyBound = false,

    // The functions to execute on DOM ready
	readyList = [],

    // The ready event handler
	DOMContentLoaded,

    // Save a reference to some core methods
	toString = Object.prototype.toString,
	hasOwnProperty = Object.prototype.hasOwnProperty,
	push = Array.prototype.push,
	slice = Array.prototype.slice,
	indexOf = Array.prototype.indexOf;

    jQuery.fn = jQuery.prototype = {
        init: function (selector, context) {
            var match, elem, ret, doc;

            // Handle $(""), $(null), or $(undefined)
            if (!selector) {
                return this;
            }

            // Handle $(DOMElement)
            if (selector.nodeType) {
                this.context = this[0] = selector;
                this.length = 1;
                return this;
            }

            // Handle HTML strings
            if (typeof selector === "string") {
                // Are we dealing with HTML string or an ID?
                match = quickExpr.exec(selector);

                // Verify a match, and that no context was specified for #id
                if (match && (match[1] || !context)) {

                    // HANDLE: $(html) -> $(array)
                    if (match[1]) {
                        doc = (context ? context.ownerDocument || context : document);

                        // If a single string is passed in and it's a single tag
                        // just do a createElement and skip the rest
                        ret = rsingleTag.exec(selector);

                        if (ret) {
                            if (jQuery.isPlainObject(context)) {
                                selector = [document.createElement(ret[1])];
                                jQuery.fn.attr.call(selector, context, true);

                            } else {
                                selector = [doc.createElement(ret[1])];
                            }

                        } else {
                            ret = buildFragment([match[1]], [doc]);
                            selector = (ret.cacheable ? ret.fragment.cloneNode(true) : ret.fragment).childNodes;
                        }

                        // HANDLE: $("#id")
                    } else {
                        elem = document.getElementById(match[2]);

                        if (elem) {
                            // Handle the case where IE and Opera return items
                            // by name instead of ID
                            if (elem.id !== match[2]) {
                                return rootjQuery.find(selector);
                            }

                            // Otherwise, we inject the element directly into the jQuery object
                            this.length = 1;
                            this[0] = elem;
                        }

                        this.context = document;
                        this.selector = selector;
                        return this;
                    }

                    // HANDLE: $("TAG")
                } else if (!context && /^\w+$/.test(selector)) {
                    this.selector = selector;
                    this.context = document;
                    selector = document.getElementsByTagName(selector);

                    // HANDLE: $(expr, $(...))
                } else if (!context || context.jquery) {
                    return (context || rootjQuery).find(selector);

                    // HANDLE: $(expr, context)
                    // (which is just equivalent to: $(context).find(expr)
                } else {
                    return jQuery(context).find(selector);
                }

                // HANDLE: $(function)
                // Shortcut for document ready
            } else if (jQuery.isFunction(selector)) {
                return rootjQuery.ready(selector);
            }

            if (selector.selector !== undefined) {
                this.selector = selector.selector;
                this.context = selector.context;
            }

            return jQuery.isArray(selector) ?
			this.setArray(selector) :
			jQuery.makeArray(selector, this);
        },

        // Start with an empty selector
        selector: "",

        // The current version of jQuery being used
        jquery: "1.4.1",

        // The default length of a jQuery object is 0
        length: 0,

        // The number of elements contained in the matched element set
        size: function () {
            return this.length;
        },

        toArray: function () {
            return slice.call(this, 0);
        },

        // Get the Nth element in the matched element set OR
        // Get the whole matched element set as a clean array
        get: function (num) {
            return num == null ?

            // Return a 'clean' array
			this.toArray() :

            // Return just the object
			(num < 0 ? this.slice(num)[0] : this[num]);
        },

        // Take an array of elements and push it onto the stack
        // (returning the new matched element set)
        pushStack: function (elems, name, selector) {
            // Build a new jQuery matched element set
            var ret = jQuery(elems || null);

            // Add the old object onto the stack (as a reference)
            ret.prevObject = this;

            ret.context = this.context;

            if (name === "find") {
                ret.selector = this.selector + (this.selector ? " " : "") + selector;
            } else if (name) {
                ret.selector = this.selector + "." + name + "(" + selector + ")";
            }

            // Return the newly-formed element set
            return ret;
        },

        // Force the current matched set of elements to become
        // the specified array of elements (destroying the stack in the process)
        // You should use pushStack() in order to do this, but maintain the stack
        setArray: function (elems) {
            // Resetting the length to 0, then using the native Array push
            // is a super-fast way to populate an object with array-like properties
            this.length = 0;
            push.apply(this, elems);

            return this;
        },

        // Execute a callback for every element in the matched set.
        // (You can seed the arguments with an array of args, but this is
        // only used internally.)
        each: function (callback, args) {
            return jQuery.each(this, callback, args);
        },

        ready: function (fn) {
            // Attach the listeners
            jQuery.bindReady();

            // If the DOM is already ready
            if (jQuery.isReady) {
                // Execute the function immediately
                fn.call(document, jQuery);

                // Otherwise, remember the function for later
            } else if (readyList) {
                // Add the function to the wait list
                readyList.push(fn);
            }

            return this;
        },

        eq: function (i) {
            return i === -1 ?
			this.slice(i) :
			this.slice(i, +i + 1);
        },

        first: function () {
            return this.eq(0);
        },

        last: function () {
            return this.eq(-1);
        },

        slice: function () {
            return this.pushStack(slice.apply(this, arguments),
			"slice", slice.call(arguments).join(","));
        },

        map: function (callback) {
            return this.pushStack(jQuery.map(this, function (elem, i) {
                return callback.call(elem, i, elem);
            }));
        },

        end: function () {
            return this.prevObject || jQuery(null);
        },

        // For internal use only.
        // Behaves like an Array's method, not like a jQuery method.
        push: push,
        sort: [].sort,
        splice: [].splice
    };

    // Give the init function the jQuery prototype for later instantiation
    jQuery.fn.init.prototype = jQuery.fn;

    jQuery.extend = jQuery.fn.extend = function () {
        // copy reference to target object
        var target = arguments[0] || {}, i = 1, length = arguments.length, deep = false, options, name, src, copy;

        // Handle a deep copy situation
        if (typeof target === "boolean") {
            deep = target;
            target = arguments[1] || {};
            // skip the boolean and the target
            i = 2;
        }

        // Handle case when target is a string or something (possible in deep copy)
        if (typeof target !== "object" && !jQuery.isFunction(target)) {
            target = {};
        }

        // extend jQuery itself if only one argument is passed
        if (length === i) {
            target = this;
            --i;
        }

        for (; i < length; i++) {
            // Only deal with non-null/undefined values
            if ((options = arguments[i]) != null) {
                // Extend the base object
                for (name in options) {
                    src = target[name];
                    copy = options[name];

                    // Prevent never-ending loop
                    if (target === copy) {
                        continue;
                    }

                    // Recurse if we're merging object literal values or arrays
                    if (deep && copy && (jQuery.isPlainObject(copy) || jQuery.isArray(copy))) {
                        var clone = src && (jQuery.isPlainObject(src) || jQuery.isArray(src)) ? src
						: jQuery.isArray(copy) ? [] : {};

                        // Never move original objects, clone them
                        target[name] = jQuery.extend(deep, clone, copy);

                        // Don't bring in undefined values
                    } else if (copy !== undefined) {
                        target[name] = copy;
                    }
                }
            }
        }

        // Return the modified object
        return target;
    };

    jQuery.extend({
        noConflict: function (deep) {
            window.$ = _$;

            if (deep) {
                window.jQuery = _jQuery;
            }

            return jQuery;
        },

        // Is the DOM ready to be used? Set to true once it occurs.
        isReady: false,

        // Handle when the DOM is ready
        ready: function () {
            // Make sure that the DOM is not already loaded
            if (!jQuery.isReady) {
                // Make sure body exists, at least, in case IE gets a little overzealous (ticket #5443).
                if (!document.body) {
                    return setTimeout(jQuery.ready, 13);
                }

                // Remember that the DOM is ready
                jQuery.isReady = true;

                // If there are functions bound, to execute
                if (readyList) {
                    // Execute all of them
                    var fn, i = 0;
                    while ((fn = readyList[i++])) {
                        fn.call(document, jQuery);
                    }

                    // Reset the list of functions
                    readyList = null;
                }

                // Trigger any bound ready events
                if (jQuery.fn.triggerHandler) {
                    jQuery(document).triggerHandler("ready");
                }
            }
        },

        bindReady: function () {
            if (readyBound) {
                return;
            }

            readyBound = true;

            // Catch cases where $(document).ready() is called after the
            // browser event has already occurred.
            if (document.readyState === "complete") {
                return jQuery.ready();
            }

            // Mozilla, Opera and webkit nightlies currently support this event
            if (document.addEventListener) {
                // Use the handy event callback
                document.addEventListener("DOMContentLoaded", DOMContentLoaded, false);

                // A fallback to window.onload, that will always work
                window.addEventListener("load", jQuery.ready, false);

                // If IE event model is used
            } else if (document.attachEvent) {
                // ensure firing before onload,
                // maybe late but safe also for iframes
                document.attachEvent("onreadystatechange", DOMContentLoaded);

                // A fallback to window.onload, that will always work
                window.attachEvent("onload", jQuery.ready);

                // If IE and not a frame
                // continually check to see if the document is ready
                var toplevel = false;

                try {
                    toplevel = window.frameElement == null;
                } catch (e) { }

                if (document.documentElement.doScroll && toplevel) {
                    doScrollCheck();
                }
            }
        },

        // See test/unit/core.js for details concerning isFunction.
        // Since version 1.3, DOM methods and functions like alert
        // aren't supported. They return false on IE (#2968).
        isFunction: function (obj) {
            return toString.call(obj) === "[object Function]";
        },

        isArray: function (obj) {
            return toString.call(obj) === "[object Array]";
        },

        isPlainObject: function (obj) {
            // Must be an Object.
            // Because of IE, we also have to check the presence of the constructor property.
            // Make sure that DOM nodes and window objects don't pass through, as well
            if (!obj || toString.call(obj) !== "[object Object]" || obj.nodeType || obj.setInterval) {
                return false;
            }

            // Not own constructor property must be Object
            if (obj.constructor
			&& !hasOwnProperty.call(obj, "constructor")
			&& !hasOwnProperty.call(obj.constructor.prototype, "isPrototypeOf")) {
                return false;
            }

            // Own properties are enumerated firstly, so to speed up,
            // if last one is own, then all properties are own.

            var key;
            for (key in obj) { }

            return key === undefined || hasOwnProperty.call(obj, key);
        },

        isEmptyObject: function (obj) {
            for (var name in obj) {
                return false;
            }
            return true;
        },

        error: function (msg) {
            throw msg;
        },

        parseJSON: function (data) {
            if (typeof data !== "string" || !data) {
                return null;
            }

            // Make sure the incoming data is actual JSON
            // Logic borrowed from http://json.org/json2.js
            if (/^[\],:{}\s]*$/.test(data.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, "@")
			.replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, "]")
			.replace(/(?:^|:|,)(?:\s*\[)+/g, ""))) {

                // Try to use the native JSON parser first
                return window.JSON && window.JSON.parse ?
				window.JSON.parse(data) :
				(new Function("return " + data))();

            } else {
                jQuery.error("Invalid JSON: " + data);
            }
        },

        noop: function () { },

        // Evalulates a script in a global context
        globalEval: function (data) {
            if (data && rnotwhite.test(data)) {
                // Inspired by code by Andrea Giammarchi
                // http://webreflection.blogspot.com/2007/08/global-scope-evaluation-and-dom.html
                var head = document.getElementsByTagName("head")[0] || document.documentElement,
				script = document.createElement("script");

                script.type = "text/javascript";

                if (jQuery.support.scriptEval) {
                    script.appendChild(document.createTextNode(data));
                } else {
                    script.text = data;
                }

                // Use insertBefore instead of appendChild to circumvent an IE6 bug.
                // This arises when a base node is used (#2709).
                head.insertBefore(script, head.firstChild);
                head.removeChild(script);
            }
        },

        nodeName: function (elem, name) {
            return elem.nodeName && elem.nodeName.toUpperCase() === name.toUpperCase();
        },

        // args is for internal usage only
        each: function (object, callback, args) {
            var name, i = 0,
			length = object.length,
			isObj = length === undefined || jQuery.isFunction(object);

            if (args) {
                if (isObj) {
                    for (name in object) {
                        if (callback.apply(object[name], args) === false) {
                            break;
                        }
                    }
                } else {
                    for (; i < length; ) {
                        if (callback.apply(object[i++], args) === false) {
                            break;
                        }
                    }
                }

                // A special, fast, case for the most common use of each
            } else {
                if (isObj) {
                    for (name in object) {
                        if (callback.call(object[name], name, object[name]) === false) {
                            break;
                        }
                    }
                } else {
                    for (var value = object[0];
					i < length && callback.call(value, i, value) !== false; value = object[++i]) { }
                }
            }

            return object;
        },

        trim: function (text) {
            return (text || "").replace(rtrim, "");
        },

        // results is for internal usage only
        makeArray: function (array, results) {
            var ret = results || [];

            if (array != null) {
                // The window, strings (and functions) also have 'length'
                // The extra typeof function check is to prevent crashes
                // in Safari 2 (See: #3039)
                if (array.length == null || typeof array === "string" || jQuery.isFunction(array) || (typeof array !== "function" && array.setInterval)) {
                    push.call(ret, array);
                } else {
                    jQuery.merge(ret, array);
                }
            }

            return ret;
        },

        inArray: function (elem, array) {
            if (array.indexOf) {
                return array.indexOf(elem);
            }

            for (var i = 0, length = array.length; i < length; i++) {
                if (array[i] === elem) {
                    return i;
                }
            }

            return -1;
        },

        merge: function (first, second) {
            var i = first.length, j = 0;

            if (typeof second.length === "number") {
                for (var l = second.length; j < l; j++) {
                    first[i++] = second[j];
                }
            } else {
                while (second[j] !== undefined) {
                    first[i++] = second[j++];
                }
            }

            first.length = i;

            return first;
        },

        grep: function (elems, callback, inv) {
            var ret = [];

            // Go through the array, only saving the items
            // that pass the validator function
            for (var i = 0, length = elems.length; i < length; i++) {
                if (!inv !== !callback(elems[i], i)) {
                    ret.push(elems[i]);
                }
            }

            return ret;
        },

        // arg is for internal usage only
        map: function (elems, callback, arg) {
            var ret = [], value;

            // Go through the array, translating each of the items to their
            // new value (or values).
            for (var i = 0, length = elems.length; i < length; i++) {
                value = callback(elems[i], i, arg);

                if (value != null) {
                    ret[ret.length] = value;
                }
            }

            return ret.concat.apply([], ret);
        },

        // A global GUID counter for objects
        guid: 1,

        proxy: function (fn, proxy, thisObject) {
            if (arguments.length === 2) {
                if (typeof proxy === "string") {
                    thisObject = fn;
                    fn = thisObject[proxy];
                    proxy = undefined;

                } else if (proxy && !jQuery.isFunction(proxy)) {
                    thisObject = proxy;
                    proxy = undefined;
                }
            }

            if (!proxy && fn) {
                proxy = function () {
                    return fn.apply(thisObject || this, arguments);
                };
            }

            // Set the guid of unique handler to the same of original handler, so it can be removed
            if (fn) {
                proxy.guid = fn.guid = fn.guid || proxy.guid || jQuery.guid++;
            }

            // So proxy can be declared as an argument
            return proxy;
        },

        // Use of jQuery.browser is frowned upon.
        // More details: http://docs.jquery.com/Utilities/jQuery.browser
        uaMatch: function (ua) {
            ua = ua.toLowerCase();

            var match = /(webkit)[ \/]([\w.]+)/.exec(ua) ||
			/(opera)(?:.*version)?[ \/]([\w.]+)/.exec(ua) ||
			/(msie) ([\w.]+)/.exec(ua) ||
			!/compatible/.test(ua) && /(mozilla)(?:.*? rv:([\w.]+))?/.exec(ua) ||
		  	[];

            return { browser: match[1] || "", version: match[2] || "0" };
        },

        browser: {}
    });

    browserMatch = jQuery.uaMatch(userAgent);
    if (browserMatch.browser) {
        jQuery.browser[browserMatch.browser] = true;
        jQuery.browser.version = browserMatch.version;
    }

    // Deprecated, use jQuery.browser.webkit instead
    if (jQuery.browser.webkit) {
        jQuery.browser.safari = true;
    }

    if (indexOf) {
        jQuery.inArray = function (elem, array) {
            return indexOf.call(array, elem);
        };
    }

    // All jQuery objects should point back to these
    rootjQuery = jQuery(document);

    // Cleanup functions for the document ready method
    if (document.addEventListener) {
        DOMContentLoaded = function () {
            document.removeEventListener("DOMContentLoaded", DOMContentLoaded, false);
            jQuery.ready();
        };

    } else if (document.attachEvent) {
        DOMContentLoaded = function () {
            // Make sure body exists, at least, in case IE gets a little overzealous (ticket #5443).
            if (document.readyState === "complete") {
                document.detachEvent("onreadystatechange", DOMContentLoaded);
                jQuery.ready();
            }
        };
    }

    // The DOM ready check for Internet Explorer
    function doScrollCheck() {
        if (jQuery.isReady) {
            return;
        }

        try {
            // If IE is used, use the trick by Diego Perini
            // http://javascript.nwbox.com/IEContentLoaded/
            document.documentElement.doScroll("left");
        } catch (error) {
            setTimeout(doScrollCheck, 1);
            return;
        }

        // and execute any waiting functions
        jQuery.ready();
    }

    function evalScript(i, elem) {
        if (elem.src) {
            jQuery.ajax({
                url: elem.src,
                async: false,
                dataType: "script"
            });
        } else {
            jQuery.globalEval(elem.text || elem.textContent || elem.innerHTML || "");
        }

        if (elem.parentNode) {
            elem.parentNode.removeChild(elem);
        }
    }

    // Mutifunctional method to get and set values to a collection
    // The value/s can be optionally by executed if its a function
    function access(elems, key, value, exec, fn, pass) {
        var length = elems.length;

        // Setting many attributes
        if (typeof key === "object") {
            for (var k in key) {
                access(elems, k, key[k], exec, fn, value);
            }
            return elems;
        }

        // Setting one attribute
        if (value !== undefined) {
            // Optionally, function values get executed if exec is true
            exec = !pass && exec && jQuery.isFunction(value);

            for (var i = 0; i < length; i++) {
                fn(elems[i], key, exec ? value.call(elems[i], i, fn(elems[i], key)) : value, pass);
            }

            return elems;
        }

        // Getting an attribute
        return length ? fn(elems[0], key) : null;
    }

    function now() {
        return (new Date).getTime();
    }
    (function () {

        jQuery.support = {};

        var root = document.documentElement,
		script = document.createElement("script"),
		div = document.createElement("div"),
		id = "script" + now();

        div.style.display = "none";
        div.innerHTML = "   <link/><table></table><a href='/a' style='color:red;float:left;opacity:.55;'>a</a><input type='checkbox'/>";

        var all = div.getElementsByTagName("*"),
		a = div.getElementsByTagName("a")[0];

        // Can't get basic test support
        if (!all || !all.length || !a) {
            return;
        }

        jQuery.support = {
            // IE strips leading whitespace when .innerHTML is used
            leadingWhitespace: div.firstChild.nodeType === 3,

            // Make sure that tbody elements aren't automatically inserted
            // IE will insert them into empty tables
            tbody: !div.getElementsByTagName("tbody").length,

            // Make sure that link elements get serialized correctly by innerHTML
            // This requires a wrapper element in IE
            htmlSerialize: !!div.getElementsByTagName("link").length,

            // Get the style information from getAttribute
            // (IE uses .cssText insted)
            style: /red/.test(a.getAttribute("style")),

            // Make sure that URLs aren't manipulated
            // (IE normalizes it by default)
            hrefNormalized: a.getAttribute("href") === "/a",

            // Make sure that element opacity exists
            // (IE uses filter instead)
            // Use a regex to work around a WebKit issue. See #5145
            opacity: /^0.55$/.test(a.style.opacity),

            // Verify style float existence
            // (IE uses styleFloat instead of cssFloat)
            cssFloat: !!a.style.cssFloat,

            // Make sure that if no value is specified for a checkbox
            // that it defaults to "on".
            // (WebKit defaults to "" instead)
            checkOn: div.getElementsByTagName("input")[0].value === "on",

            // Make sure that a selected-by-default option has a working selected property.
            // (WebKit defaults to false instead of true, IE too, if it's in an optgroup)
            optSelected: document.createElement("select").appendChild(document.createElement("option")).selected,

            // Will be defined later
            checkClone: false,
            scriptEval: false,
            noCloneEvent: true,
            boxModel: null
        };

        script.type = "text/javascript";
        try {
            script.appendChild(document.createTextNode("window." + id + "=1;"));
        } catch (e) { }

        root.insertBefore(script, root.firstChild);

        // Make sure that the execution of code works by injecting a script
        // tag with appendChild/createTextNode
        // (IE doesn't support this, fails, and uses .text instead)
        if (window[id]) {
            jQuery.support.scriptEval = true;
            delete window[id];
        }

        root.removeChild(script);

        if (div.attachEvent && div.fireEvent) {
            div.attachEvent("onclick", function click() {
                // Cloning a node shouldn't copy over any
                // bound event handlers (IE does this)
                jQuery.support.noCloneEvent = false;
                div.detachEvent("onclick", click);
            });
            div.cloneNode(true).fireEvent("onclick");
        }

        div = document.createElement("div");
        div.innerHTML = "<input type='radio' name='radiotest' checked='checked'/>";

        var fragment = document.createDocumentFragment();
        fragment.appendChild(div.firstChild);

        // WebKit doesn't clone checked state correctly in fragments
        jQuery.support.checkClone = fragment.cloneNode(true).cloneNode(true).lastChild.checked;

        // Figure out if the W3C box model works as expected
        // document.body must exist before we can do this
        jQuery(function () {
            var div = document.createElement("div");
            div.style.width = div.style.paddingLeft = "1px";

            document.body.appendChild(div);
            jQuery.boxModel = jQuery.support.boxModel = div.offsetWidth === 2;
            document.body.removeChild(div).style.display = 'none';
            div = null;
        });

        // Technique from Juriy Zaytsev
        // http://thinkweb2.com/projects/prototype/detecting-event-support-without-browser-sniffing/
        var eventSupported = function (eventName) {
            var el = document.createElement("div");
            eventName = "on" + eventName;

            var isSupported = (eventName in el);
            if (!isSupported) {
                el.setAttribute(eventName, "return;");
                isSupported = typeof el[eventName] === "function";
            }
            el = null;

            return isSupported;
        };

        jQuery.support.submitBubbles = eventSupported("submit");
        jQuery.support.changeBubbles = eventSupported("change");

        // release memory in IE
        root = script = div = all = a = null;
    })();

    jQuery.props = {
        "for": "htmlFor",
        "class": "className",
        readonly: "readOnly",
        maxlength: "maxLength",
        cellspacing: "cellSpacing",
        rowspan: "rowSpan",
        colspan: "colSpan",
        tabindex: "tabIndex",
        usemap: "useMap",
        frameborder: "frameBorder"
    };
    var expando = "jQuery" + now(), uuid = 0, windowData = {};
    var emptyObject = {};

    jQuery.extend({
        cache: {},

        expando: expando,

        // The following elements throw uncatchable exceptions if you
        // attempt to add expando properties to them.
        noData: {
            "embed": true,
            "object": true,
            "applet": true
        },

        data: function (elem, name, data) {
            if (elem.nodeName && jQuery.noData[elem.nodeName.toLowerCase()]) {
                return;
            }

            elem = elem == window ?
			windowData :
			elem;

            var id = elem[expando], cache = jQuery.cache, thisCache;

            // Handle the case where there's no name immediately
            if (!name && !id) {
                return null;
            }

            // Compute a unique ID for the element
            if (!id) {
                id = ++uuid;
            }

            // Avoid generating a new cache unless none exists and we
            // want to manipulate it.
            if (typeof name === "object") {
                elem[expando] = id;
                thisCache = cache[id] = jQuery.extend(true, {}, name);
            } else if (cache[id]) {
                thisCache = cache[id];
            } else if (typeof data === "undefined") {
                thisCache = emptyObject;
            } else {
                thisCache = cache[id] = {};
            }

            // Prevent overriding the named cache with undefined values
            if (data !== undefined) {
                elem[expando] = id;
                thisCache[name] = data;
            }

            return typeof name === "string" ? thisCache[name] : thisCache;
        },

        removeData: function (elem, name) {
            if (elem.nodeName && jQuery.noData[elem.nodeName.toLowerCase()]) {
                return;
            }

            elem = elem == window ?
			windowData :
			elem;

            var id = elem[expando], cache = jQuery.cache, thisCache = cache[id];

            // If we want to remove a specific section of the element's data
            if (name) {
                if (thisCache) {
                    // Remove the section of cache data
                    delete thisCache[name];

                    // If we've removed all the data, remove the element's cache
                    if (jQuery.isEmptyObject(thisCache)) {
                        jQuery.removeData(elem);
                    }
                }

                // Otherwise, we want to remove all of the element's data
            } else {
                // Clean up the element expando
                try {
                    delete elem[expando];
                } catch (e) {
                    // IE has trouble directly removing the expando
                    // but it's ok with using removeAttribute
                    if (elem.removeAttribute) {
                        elem.removeAttribute(expando);
                    }
                }

                // Completely remove the data cache
                delete cache[id];
            }
        }
    });

    jQuery.fn.extend({
        data: function (key, value) {
            if (typeof key === "undefined" && this.length) {
                return jQuery.data(this[0]);

            } else if (typeof key === "object") {
                return this.each(function () {
                    jQuery.data(this, key);
                });
            }

            var parts = key.split(".");
            parts[1] = parts[1] ? "." + parts[1] : "";

            if (value === undefined) {
                var data = this.triggerHandler("getData" + parts[1] + "!", [parts[0]]);

                if (data === undefined && this.length) {
                    data = jQuery.data(this[0], key);
                }
                return data === undefined && parts[1] ?
				this.data(parts[0]) :
				data;
            } else {
                return this.trigger("setData" + parts[1] + "!", [parts[0], value]).each(function () {
                    jQuery.data(this, key, value);
                });
            }
        },

        removeData: function (key) {
            return this.each(function () {
                jQuery.removeData(this, key);
            });
        }
    });
    jQuery.extend({
        queue: function (elem, type, data) {
            if (!elem) {
                return;
            }

            type = (type || "fx") + "queue";
            var q = jQuery.data(elem, type);

            // Speed up dequeue by getting out quickly if this is just a lookup
            if (!data) {
                return q || [];
            }

            if (!q || jQuery.isArray(data)) {
                q = jQuery.data(elem, type, jQuery.makeArray(data));

            } else {
                q.push(data);
            }

            return q;
        },

        dequeue: function (elem, type) {
            type = type || "fx";

            var queue = jQuery.queue(elem, type), fn = queue.shift();

            // If the fx queue is dequeued, always remove the progress sentinel
            if (fn === "inprogress") {
                fn = queue.shift();
            }

            if (fn) {
                // Add a progress sentinel to prevent the fx queue from being
                // automatically dequeued
                if (type === "fx") {
                    queue.unshift("inprogress");
                }

                fn.call(elem, function () {
                    jQuery.dequeue(elem, type);
                });
            }
        }
    });

    jQuery.fn.extend({
        queue: function (type, data) {
            if (typeof type !== "string") {
                data = type;
                type = "fx";
            }

            if (data === undefined) {
                return jQuery.queue(this[0], type);
            }
            return this.each(function (i, elem) {
                var queue = jQuery.queue(this, type, data);

                if (type === "fx" && queue[0] !== "inprogress") {
                    jQuery.dequeue(this, type);
                }
            });
        },
        dequeue: function (type) {
            return this.each(function () {
                jQuery.dequeue(this, type);
            });
        },

        // Based off of the plugin by Clint Helfers, with permission.
        // http://blindsignals.com/index.php/2009/07/jquery-delay/
        delay: function (time, type) {
            time = jQuery.fx ? jQuery.fx.speeds[time] || time : time;
            type = type || "fx";

            return this.queue(type, function () {
                var elem = this;
                setTimeout(function () {
                    jQuery.dequeue(elem, type);
                }, time);
            });
        },

        clearQueue: function (type) {
            return this.queue(type || "fx", []);
        }
    });
    var rclass = /[\n\t]/g,
	rspace = /\s+/,
	rreturn = /\r/g,
	rspecialurl = /href|src|style/,
	rtype = /(button|input)/i,
	rfocusable = /(button|input|object|select|textarea)/i,
	rclickable = /^(a|area)$/i,
	rradiocheck = /radio|checkbox/;

    jQuery.fn.extend({
        attr: function (name, value) {
            return access(this, name, value, true, jQuery.attr);
        },

        removeAttr: function (name, fn) {
            return this.each(function () {
                jQuery.attr(this, name, "");
                if (this.nodeType === 1) {
                    this.removeAttribute(name);
                }
            });
        },

        addClass: function (value) {
            if (jQuery.isFunction(value)) {
                return this.each(function (i) {
                    var self = jQuery(this);
                    self.addClass(value.call(this, i, self.attr("class")));
                });
            }

            if (value && typeof value === "string") {
                var classNames = (value || "").split(rspace);

                for (var i = 0, l = this.length; i < l; i++) {
                    var elem = this[i];

                    if (elem.nodeType === 1) {
                        if (!elem.className) {
                            elem.className = value;

                        } else {
                            var className = " " + elem.className + " ";
                            for (var c = 0, cl = classNames.length; c < cl; c++) {
                                if (className.indexOf(" " + classNames[c] + " ") < 0) {
                                    elem.className += " " + classNames[c];
                                }
                            }
                        }
                    }
                }
            }

            return this;
        },

        removeClass: function (value) {
            if (jQuery.isFunction(value)) {
                return this.each(function (i) {
                    var self = jQuery(this);
                    self.removeClass(value.call(this, i, self.attr("class")));
                });
            }

            if ((value && typeof value === "string") || value === undefined) {
                var classNames = (value || "").split(rspace);

                for (var i = 0, l = this.length; i < l; i++) {
                    var elem = this[i];

                    if (elem.nodeType === 1 && elem.className) {
                        if (value) {
                            var className = (" " + elem.className + " ").replace(rclass, " ");
                            for (var c = 0, cl = classNames.length; c < cl; c++) {
                                className = className.replace(" " + classNames[c] + " ", " ");
                            }
                            elem.className = className.substring(1, className.length - 1);

                        } else {
                            elem.className = "";
                        }
                    }
                }
            }

            return this;
        },

        toggleClass: function (value, stateVal) {
            var type = typeof value, isBool = typeof stateVal === "boolean";

            if (jQuery.isFunction(value)) {
                return this.each(function (i) {
                    var self = jQuery(this);
                    self.toggleClass(value.call(this, i, self.attr("class"), stateVal), stateVal);
                });
            }

            return this.each(function () {
                if (type === "string") {
                    // toggle individual class names
                    var className, i = 0, self = jQuery(this),
					state = stateVal,
					classNames = value.split(rspace);

                    while ((className = classNames[i++])) {
                        // check each className given, space seperated list
                        state = isBool ? state : !self.hasClass(className);
                        self[state ? "addClass" : "removeClass"](className);
                    }

                } else if (type === "undefined" || type === "boolean") {
                    if (this.className) {
                        // store className if set
                        jQuery.data(this, "__className__", this.className);
                    }

                    // toggle whole className
                    this.className = this.className || value === false ? "" : jQuery.data(this, "__className__") || "";
                }
            });
        },

        hasClass: function (selector) {
            var className = " " + selector + " ";
            for (var i = 0, l = this.length; i < l; i++) {
                if ((" " + this[i].className + " ").replace(rclass, " ").indexOf(className) > -1) {
                    return true;
                }
            }

            return false;
        },

        val: function (value) {
            if (value === undefined) {
                var elem = this[0];

                if (elem) {
                    if (jQuery.nodeName(elem, "option")) {
                        return (elem.attributes.value || {}).specified ? elem.value : elem.text;
                    }

                    // We need to handle select boxes special
                    if (jQuery.nodeName(elem, "select")) {
                        var index = elem.selectedIndex,
						values = [],
						options = elem.options,
						one = elem.type === "select-one";

                        // Nothing was selected
                        if (index < 0) {
                            return null;
                        }

                        // Loop through all the selected options
                        for (var i = one ? index : 0, max = one ? index + 1 : options.length; i < max; i++) {
                            var option = options[i];

                            if (option.selected) {
                                // Get the specifc value for the option
                                value = jQuery(option).val();

                                // We don't need an array for one selects
                                if (one) {
                                    return value;
                                }

                                // Multi-Selects return an array
                                values.push(value);
                            }
                        }

                        return values;
                    }

                    // Handle the case where in Webkit "" is returned instead of "on" if a value isn't specified
                    if (rradiocheck.test(elem.type) && !jQuery.support.checkOn) {
                        return elem.getAttribute("value") === null ? "on" : elem.value;
                    }


                    // Everything else, we just grab the value
                    return (elem.value || "").replace(rreturn, "");

                }

                return undefined;
            }

            var isFunction = jQuery.isFunction(value);

            return this.each(function (i) {
                var self = jQuery(this), val = value;

                if (this.nodeType !== 1) {
                    return;
                }

                if (isFunction) {
                    val = value.call(this, i, self.val());
                }

                // Typecast each time if the value is a Function and the appended
                // value is therefore different each time.
                if (typeof val === "number") {
                    val += "";
                }

                if (jQuery.isArray(val) && rradiocheck.test(this.type)) {
                    this.checked = jQuery.inArray(self.val(), val) >= 0;

                } else if (jQuery.nodeName(this, "select")) {
                    var values = jQuery.makeArray(val);

                    jQuery("option", this).each(function () {
                        this.selected = jQuery.inArray(jQuery(this).val(), values) >= 0;
                    });

                    if (!values.length) {
                        this.selectedIndex = -1;
                    }

                } else {
                    this.value = val;
                }
            });
        }
    });

    jQuery.extend({
        attrFn: {
            val: true,
            css: true,
            html: true,
            text: true,
            data: true,
            width: true,
            height: true,
            offset: true
        },

        attr: function (elem, name, value, pass) {
            // don't set attributes on text and comment nodes
            if (!elem || elem.nodeType === 3 || elem.nodeType === 8) {
                return undefined;
            }

            if (pass && name in jQuery.attrFn) {
                return jQuery(elem)[name](value);
            }

            var notxml = elem.nodeType !== 1 || !jQuery.isXMLDoc(elem),
            // Whether we are setting (or getting)
			set = value !== undefined;

            // Try to normalize/fix the name
            name = notxml && jQuery.props[name] || name;

            // Only do all the following if this is a node (faster for style)
            if (elem.nodeType === 1) {
                // These attributes require special treatment
                var special = rspecialurl.test(name);

                // Safari mis-reports the default selected property of an option
                // Accessing the parent's selectedIndex property fixes it
                if (name === "selected" && !jQuery.support.optSelected) {
                    var parent = elem.parentNode;
                    if (parent) {
                        parent.selectedIndex;

                        // Make sure that it also works with optgroups, see #5701
                        if (parent.parentNode) {
                            parent.parentNode.selectedIndex;
                        }
                    }
                }

                // If applicable, access the attribute via the DOM 0 way
                if (name in elem && notxml && !special) {
                    if (set) {
                        // We can't allow the type property to be changed (since it causes problems in IE)
                        if (name === "type" && rtype.test(elem.nodeName) && elem.parentNode) {
                            jQuery.error("type property can't be changed");
                        }

                        elem[name] = value;
                    }

                    // browsers index elements by id/name on forms, give priority to attributes.
                    if (jQuery.nodeName(elem, "form") && elem.getAttributeNode(name)) {
                        return elem.getAttributeNode(name).nodeValue;
                    }

                    // elem.tabIndex doesn't always return the correct value when it hasn't been explicitly set
                    // http://fluidproject.org/blog/2008/01/09/getting-setting-and-removing-tabindex-values-with-javascript/
                    if (name === "tabIndex") {
                        var attributeNode = elem.getAttributeNode("tabIndex");

                        return attributeNode && attributeNode.specified ?
						attributeNode.value :
						rfocusable.test(elem.nodeName) || rclickable.test(elem.nodeName) && elem.href ?
							0 :
							undefined;
                    }

                    return elem[name];
                }

                if (!jQuery.support.style && notxml && name === "style") {
                    if (set) {
                        elem.style.cssText = "" + value;
                    }

                    return elem.style.cssText;
                }

                if (set) {
                    // convert the value to a string (all browsers do this but IE) see #1070
                    elem.setAttribute(name, "" + value);
                }

                var attr = !jQuery.support.hrefNormalized && notxml && special ?
                // Some attributes require a special call on IE
					elem.getAttribute(name, 2) :
					elem.getAttribute(name);

                // Non-existent attributes return null, we normalize to undefined
                return attr === null ? undefined : attr;
            }

            // elem is actually elem.style ... set the style
            // Using attr for specific style information is now deprecated. Use style insead.
            return jQuery.style(elem, name, value);
        }
    });
    var fcleanup = function (nm) {
        return nm.replace(/[^\w\s\.\|`]/g, function (ch) {
            return "\\" + ch;
        });
    };

    /*
    * A number of helper functions used for managing events.
    * Many of the ideas behind this code originated from
    * Dean Edwards' addEvent library.
    */
    jQuery.event = {

        // Bind an event to an element
        // Original by Dean Edwards
        add: function (elem, types, handler, data) {
            if (elem.nodeType === 3 || elem.nodeType === 8) {
                return;
            }

            // For whatever reason, IE has trouble passing the window object
            // around, causing it to be cloned in the process
            if (elem.setInterval && (elem !== window && !elem.frameElement)) {
                elem = window;
            }

            // Make sure that the function being executed has a unique ID
            if (!handler.guid) {
                handler.guid = jQuery.guid++;
            }

            // if data is passed, bind to handler
            if (data !== undefined) {
                // Create temporary function pointer to original handler
                var fn = handler;

                // Create unique handler function, wrapped around original handler
                handler = jQuery.proxy(fn);

                // Store data in unique handler
                handler.data = data;
            }

            // Init the element's event structure
            var events = jQuery.data(elem, "events") || jQuery.data(elem, "events", {}),
			handle = jQuery.data(elem, "handle"), eventHandle;

            if (!handle) {
                eventHandle = function () {
                    // Handle the second event of a trigger and when
                    // an event is called after a page has unloaded
                    return typeof jQuery !== "undefined" && !jQuery.event.triggered ?
					jQuery.event.handle.apply(eventHandle.elem, arguments) :
					undefined;
                };

                handle = jQuery.data(elem, "handle", eventHandle);
            }

            // If no handle is found then we must be trying to bind to one of the
            // banned noData elements
            if (!handle) {
                return;
            }

            // Add elem as a property of the handle function
            // This is to prevent a memory leak with non-native
            // event in IE.
            handle.elem = elem;

            // Handle multiple events separated by a space
            // jQuery(...).bind("mouseover mouseout", fn);
            types = types.split(/\s+/);

            var type, i = 0;

            while ((type = types[i++])) {
                // Namespaced event handlers
                var namespaces = type.split(".");
                type = namespaces.shift();

                if (i > 1) {
                    handler = jQuery.proxy(handler);

                    if (data !== undefined) {
                        handler.data = data;
                    }
                }

                handler.type = namespaces.slice(0).sort().join(".");

                // Get the current list of functions bound to this event
                var handlers = events[type],
				special = this.special[type] || {};

                // Init the event handler queue
                if (!handlers) {
                    handlers = events[type] = {};

                    // Check for a special event handler
                    // Only use addEventListener/attachEvent if the special
                    // events handler returns false
                    if (!special.setup || special.setup.call(elem, data, namespaces, handler) === false) {
                        // Bind the global event handler to the element
                        if (elem.addEventListener) {
                            elem.addEventListener(type, handle, false);
                        } else if (elem.attachEvent) {
                            elem.attachEvent("on" + type, handle);
                        }
                    }
                }

                if (special.add) {
                    var modifiedHandler = special.add.call(elem, handler, data, namespaces, handlers);
                    if (modifiedHandler && jQuery.isFunction(modifiedHandler)) {
                        modifiedHandler.guid = modifiedHandler.guid || handler.guid;
                        modifiedHandler.data = modifiedHandler.data || handler.data;
                        modifiedHandler.type = modifiedHandler.type || handler.type;
                        handler = modifiedHandler;
                    }
                }

                // Add the function to the element's handler list
                handlers[handler.guid] = handler;

                // Keep track of which events have been used, for global triggering
                this.global[type] = true;
            }

            // Nullify elem to prevent memory leaks in IE
            elem = null;
        },

        global: {},

        // Detach an event or set of events from an element
        remove: function (elem, types, handler) {
            // don't do events on text and comment nodes
            if (elem.nodeType === 3 || elem.nodeType === 8) {
                return;
            }

            var events = jQuery.data(elem, "events"), ret, type, fn;

            if (events) {
                // Unbind all events for the element
                if (types === undefined || (typeof types === "string" && types.charAt(0) === ".")) {
                    for (type in events) {
                        this.remove(elem, type + (types || ""));
                    }
                } else {
                    // types is actually an event object here
                    if (types.type) {
                        handler = types.handler;
                        types = types.type;
                    }

                    // Handle multiple events separated by a space
                    // jQuery(...).unbind("mouseover mouseout", fn);
                    types = types.split(/\s+/);
                    var i = 0;
                    while ((type = types[i++])) {
                        // Namespaced event handlers
                        var namespaces = type.split(".");
                        type = namespaces.shift();
                        var all = !namespaces.length,
						cleaned = jQuery.map(namespaces.slice(0).sort(), fcleanup),
						namespace = new RegExp("(^|\\.)" + cleaned.join("\\.(?:.*\\.)?") + "(\\.|$)"),
						special = this.special[type] || {};

                        if (events[type]) {
                            // remove the given handler for the given type
                            if (handler) {
                                fn = events[type][handler.guid];
                                delete events[type][handler.guid];

                                // remove all handlers for the given type
                            } else {
                                for (var handle in events[type]) {
                                    // Handle the removal of namespaced events
                                    if (all || namespace.test(events[type][handle].type)) {
                                        delete events[type][handle];
                                    }
                                }
                            }

                            if (special.remove) {
                                special.remove.call(elem, namespaces, fn);
                            }

                            // remove generic event handler if no more handlers exist
                            for (ret in events[type]) {
                                break;
                            }
                            if (!ret) {
                                if (!special.teardown || special.teardown.call(elem, namespaces) === false) {
                                    if (elem.removeEventListener) {
                                        elem.removeEventListener(type, jQuery.data(elem, "handle"), false);
                                    } else if (elem.detachEvent) {
                                        elem.detachEvent("on" + type, jQuery.data(elem, "handle"));
                                    }
                                }
                                ret = null;
                                delete events[type];
                            }
                        }
                    }
                }

                // Remove the expando if it's no longer used
                for (ret in events) {
                    break;
                }
                if (!ret) {
                    var handle = jQuery.data(elem, "handle");
                    if (handle) {
                        handle.elem = null;
                    }
                    jQuery.removeData(elem, "events");
                    jQuery.removeData(elem, "handle");
                }
            }
        },

        // bubbling is internal
        trigger: function (event, data, elem /*, bubbling */) {
            // Event object or event type
            var type = event.type || event,
			bubbling = arguments[3];

            if (!bubbling) {
                event = typeof event === "object" ?
                // jQuery.Event object
				event[expando] ? event :
                // Object literal
				jQuery.extend(jQuery.Event(type), event) :
                // Just the event type (string)
				jQuery.Event(type);

                if (type.indexOf("!") >= 0) {
                    event.type = type = type.slice(0, -1);
                    event.exclusive = true;
                }

                // Handle a global trigger
                if (!elem) {
                    // Don't bubble custom events when global (to avoid too much overhead)
                    event.stopPropagation();

                    // Only trigger if we've ever bound an event for it
                    if (this.global[type]) {
                        jQuery.each(jQuery.cache, function () {
                            if (this.events && this.events[type]) {
                                jQuery.event.trigger(event, data, this.handle.elem);
                            }
                        });
                    }
                }

                // Handle triggering a single element

                // don't do events on text and comment nodes
                if (!elem || elem.nodeType === 3 || elem.nodeType === 8) {
                    return undefined;
                }

                // Clean up in case it is reused
                event.result = undefined;
                event.target = elem;

                // Clone the incoming data, if any
                data = jQuery.makeArray(data);
                data.unshift(event);
            }

            event.currentTarget = elem;

            // Trigger the event, it is assumed that "handle" is a function
            var handle = jQuery.data(elem, "handle");
            if (handle) {
                handle.apply(elem, data);
            }

            var parent = elem.parentNode || elem.ownerDocument;

            // Trigger an inline bound script
            try {
                if (!(elem && elem.nodeName && jQuery.noData[elem.nodeName.toLowerCase()])) {
                    if (elem["on" + type] && elem["on" + type].apply(elem, data) === false) {
                        event.result = false;
                    }
                }

                // prevent IE from throwing an error for some elements with some event types, see #3533
            } catch (e) { }

            if (!event.isPropagationStopped() && parent) {
                jQuery.event.trigger(event, data, parent, true);

            } else if (!event.isDefaultPrevented()) {
                var target = event.target, old,
				isClick = jQuery.nodeName(target, "a") && type === "click";

                if (!isClick && !(target && target.nodeName && jQuery.noData[target.nodeName.toLowerCase()])) {
                    try {
                        if (target[type]) {
                            // Make sure that we don't accidentally re-trigger the onFOO events
                            old = target["on" + type];

                            if (old) {
                                target["on" + type] = null;
                            }

                            this.triggered = true;
                            target[type]();
                        }

                        // prevent IE from throwing an error for some elements with some event types, see #3533
                    } catch (e) { }

                    if (old) {
                        target["on" + type] = old;
                    }

                    this.triggered = false;
                }
            }
        },

        handle: function (event) {
            // returned undefined or false
            var all, handlers;

            event = arguments[0] = jQuery.event.fix(event || window.event);
            event.currentTarget = this;

            // Namespaced event handlers
            var namespaces = event.type.split(".");
            event.type = namespaces.shift();

            // Cache this now, all = true means, any handler
            all = !namespaces.length && !event.exclusive;

            var namespace = new RegExp("(^|\\.)" + namespaces.slice(0).sort().join("\\.(?:.*\\.)?") + "(\\.|$)");

            handlers = (jQuery.data(this, "events") || {})[event.type];

            for (var j in handlers) {
                var handler = handlers[j];

                // Filter the functions by class
                if (all || namespace.test(handler.type)) {
                    // Pass in a reference to the handler function itself
                    // So that we can later remove it
                    event.handler = handler;
                    event.data = handler.data;

                    var ret = handler.apply(this, arguments);

                    if (ret !== undefined) {
                        event.result = ret;
                        if (ret === false) {
                            event.preventDefault();
                            event.stopPropagation();
                        }
                    }

                    if (event.isImmediatePropagationStopped()) {
                        break;
                    }

                }
            }

            return event.result;
        },

        props: "altKey attrChange attrName bubbles button cancelable charCode clientX clientY ctrlKey currentTarget data detail eventPhase fromElement handler keyCode layerX layerY metaKey newValue offsetX offsetY originalTarget pageX pageY prevValue relatedNode relatedTarget screenX screenY shiftKey srcElement target toElement view wheelDelta which".split(" "),

        fix: function (event) {
            if (event[expando]) {
                return event;
            }

            // store a copy of the original event object
            // and "clone" to set read-only properties
            var originalEvent = event;
            event = jQuery.Event(originalEvent);

            for (var i = this.props.length, prop; i; ) {
                prop = this.props[--i];
                event[prop] = originalEvent[prop];
            }

            // Fix target property, if necessary
            if (!event.target) {
                event.target = event.srcElement || document; // Fixes #1925 where srcElement might not be defined either
            }

            // check if target is a textnode (safari)
            if (event.target.nodeType === 3) {
                event.target = event.target.parentNode;
            }

            // Add relatedTarget, if necessary
            if (!event.relatedTarget && event.fromElement) {
                event.relatedTarget = event.fromElement === event.target ? event.toElement : event.fromElement;
            }

            // Calculate pageX/Y if missing and clientX/Y available
            if (event.pageX == null && event.clientX != null) {
                var doc = document.documentElement, body = document.body;
                event.pageX = event.clientX + (doc && doc.scrollLeft || body && body.scrollLeft || 0) - (doc && doc.clientLeft || body && body.clientLeft || 0);
                event.pageY = event.clientY + (doc && doc.scrollTop || body && body.scrollTop || 0) - (doc && doc.clientTop || body && body.clientTop || 0);
            }

            // Add which for key events
            if (!event.which && ((event.charCode || event.charCode === 0) ? event.charCode : event.keyCode)) {
                event.which = event.charCode || event.keyCode;
            }

            // Add metaKey to non-Mac browsers (use ctrl for PC's and Meta for Macs)
            if (!event.metaKey && event.ctrlKey) {
                event.metaKey = event.ctrlKey;
            }

            // Add which for click: 1 === left; 2 === middle; 3 === right
            // Note: button is not normalized, so don't use it
            if (!event.which && event.button !== undefined) {
                event.which = (event.button & 1 ? 1 : (event.button & 2 ? 3 : (event.button & 4 ? 2 : 0)));
            }

            return event;
        },

        // Deprecated, use jQuery.guid instead
        guid: 1E8,

        // Deprecated, use jQuery.proxy instead
        proxy: jQuery.proxy,

        special: {
            ready: {
                // Make sure the ready event is setup
                setup: jQuery.bindReady,
                teardown: jQuery.noop
            },

            live: {
                add: function (proxy, data, namespaces, live) {
                    jQuery.extend(proxy, data || {});

                    proxy.guid += data.selector + data.live;
                    data.liveProxy = proxy;

                    jQuery.event.add(this, data.live, liveHandler, data);

                },

                remove: function (namespaces) {
                    if (namespaces.length) {
                        var remove = 0, name = new RegExp("(^|\\.)" + namespaces[0] + "(\\.|$)");

                        jQuery.each((jQuery.data(this, "events").live || {}), function () {
                            if (name.test(this.type)) {
                                remove++;
                            }
                        });

                        if (remove < 1) {
                            jQuery.event.remove(this, namespaces[0], liveHandler);
                        }
                    }
                },
                special: {}
            },
            beforeunload: {
                setup: function (data, namespaces, fn) {
                    // We only want to do this special case on windows
                    if (this.setInterval) {
                        this.onbeforeunload = fn;
                    }

                    return false;
                },
                teardown: function (namespaces, fn) {
                    if (this.onbeforeunload === fn) {
                        this.onbeforeunload = null;
                    }
                }
            }
        }
    };

    jQuery.Event = function (src) {
        // Allow instantiation without the 'new' keyword
        if (!this.preventDefault) {
            return new jQuery.Event(src);
        }

        // Event object
        if (src && src.type) {
            this.originalEvent = src;
            this.type = src.type;
            // Event type
        } else {
            this.type = src;
        }

        // timeStamp is buggy for some events on Firefox(#3843)
        // So we won't rely on the native value
        this.timeStamp = now();

        // Mark it as fixed
        this[expando] = true;
    };

    function returnFalse() {
        return false;
    }
    function returnTrue() {
        return true;
    }

    // jQuery.Event is based on DOM3 Events as specified by the ECMAScript Language Binding
    // http://www.w3.org/TR/2003/WD-DOM-Level-3-Events-20030331/ecma-script-binding.html
    jQuery.Event.prototype = {
        preventDefault: function () {
            this.isDefaultPrevented = returnTrue;

            var e = this.originalEvent;
            if (!e) {
                return;
            }

            // if preventDefault exists run it on the original event
            if (e.preventDefault) {
                e.preventDefault();
            }
            // otherwise set the returnValue property of the original event to false (IE)
            e.returnValue = false;
        },
        stopPropagation: function () {
            this.isPropagationStopped = returnTrue;

            var e = this.originalEvent;
            if (!e) {
                return;
            }
            // if stopPropagation exists run it on the original event
            if (e.stopPropagation) {
                e.stopPropagation();
            }
            // otherwise set the cancelBubble property of the original event to true (IE)
            e.cancelBubble = true;
        },
        stopImmediatePropagation: function () {
            this.isImmediatePropagationStopped = returnTrue;
            this.stopPropagation();
        },
        isDefaultPrevented: returnFalse,
        isPropagationStopped: returnFalse,
        isImmediatePropagationStopped: returnFalse
    };

    // Checks if an event happened on an element within another element
    // Used in jQuery.event.special.mouseenter and mouseleave handlers
    var withinElement = function (event) {
        // Check if mouse(over|out) are still within the same parent element
        var parent = event.relatedTarget;

        // Traverse up the tree
        while (parent && parent !== this) {
            // Firefox sometimes assigns relatedTarget a XUL element
            // which we cannot access the parentNode property of
            try {
                parent = parent.parentNode;

                // assuming we've left the element since we most likely mousedover a xul element
            } catch (e) {
                break;
            }
        }

        if (parent !== this) {
            // set the correct event type
            event.type = event.data;

            // handle event if we actually just moused on to a non sub-element
            jQuery.event.handle.apply(this, arguments);
        }

    },

    // In case of event delegation, we only need to rename the event.type,
    // liveHandler will take care of the rest.
delegate = function (event) {
    event.type = event.data;
    jQuery.event.handle.apply(this, arguments);
};

    // Create mouseenter and mouseleave events
    jQuery.each({
        mouseenter: "mouseover",
        mouseleave: "mouseout"
    }, function (orig, fix) {
        jQuery.event.special[orig] = {
            setup: function (data) {
                jQuery.event.add(this, fix, data && data.selector ? delegate : withinElement, orig);
            },
            teardown: function (data) {
                jQuery.event.remove(this, fix, data && data.selector ? delegate : withinElement);
            }
        };
    });

    // submit delegation
    if (!jQuery.support.submitBubbles) {

        jQuery.event.special.submit = {
            setup: function (data, namespaces, fn) {
                if (this.nodeName.toLowerCase() !== "form") {
                    jQuery.event.add(this, "click.specialSubmit." + fn.guid, function (e) {
                        var elem = e.target, type = elem.type;

                        if ((type === "submit" || type === "image") && jQuery(elem).closest("form").length) {
                            return trigger("submit", this, arguments);
                        }
                    });

                    jQuery.event.add(this, "keypress.specialSubmit." + fn.guid, function (e) {
                        var elem = e.target, type = elem.type;

                        if ((type === "text" || type === "password") && jQuery(elem).closest("form").length && e.keyCode === 13) {
                            return trigger("submit", this, arguments);
                        }
                    });

                } else {
                    return false;
                }
            },

            remove: function (namespaces, fn) {
                jQuery.event.remove(this, "click.specialSubmit" + (fn ? "." + fn.guid : ""));
                jQuery.event.remove(this, "keypress.specialSubmit" + (fn ? "." + fn.guid : ""));
            }
        };

    }

    // change delegation, happens here so we have bind.
    if (!jQuery.support.changeBubbles) {

        var formElems = /textarea|input|select/i;

        function getVal(elem) {
            var type = elem.type, val = elem.value;

            if (type === "radio" || type === "checkbox") {
                val = elem.checked;

            } else if (type === "select-multiple") {
                val = elem.selectedIndex > -1 ?
			jQuery.map(elem.options, function (elem) {
			    return elem.selected;
			}).join("-") :
			"";

            } else if (elem.nodeName.toLowerCase() === "select") {
                val = elem.selectedIndex;
            }

            return val;
        }

        function testChange(e) {
            var elem = e.target, data, val;

            if (!formElems.test(elem.nodeName) || elem.readOnly) {
                return;
            }

            data = jQuery.data(elem, "_change_data");
            val = getVal(elem);

            // the current data will be also retrieved by beforeactivate
            if (e.type !== "focusout" || elem.type !== "radio") {
                jQuery.data(elem, "_change_data", val);
            }

            if (data === undefined || val === data) {
                return;
            }

            if (data != null || val) {
                e.type = "change";
                return jQuery.event.trigger(e, arguments[1], elem);
            }
        }

        jQuery.event.special.change = {
            filters: {
                focusout: testChange,

                click: function (e) {
                    var elem = e.target, type = elem.type;

                    if (type === "radio" || type === "checkbox" || elem.nodeName.toLowerCase() === "select") {
                        return testChange.call(this, e);
                    }
                },

                // Change has to be called before submit
                // Keydown will be called before keypress, which is used in submit-event delegation
                keydown: function (e) {
                    var elem = e.target, type = elem.type;

                    if ((e.keyCode === 13 && elem.nodeName.toLowerCase() !== "textarea") ||
				(e.keyCode === 32 && (type === "checkbox" || type === "radio")) ||
				type === "select-multiple") {
                        return testChange.call(this, e);
                    }
                },

                // Beforeactivate happens also before the previous element is blurred
                // with this event you can't trigger a change event, but you can store
                // information/focus[in] is not needed anymore
                beforeactivate: function (e) {
                    var elem = e.target;

                    if (elem.nodeName.toLowerCase() === "input" && elem.type === "radio") {
                        jQuery.data(elem, "_change_data", getVal(elem));
                    }
                }
            },
            setup: function (data, namespaces, fn) {
                for (var type in changeFilters) {
                    jQuery.event.add(this, type + ".specialChange." + fn.guid, changeFilters[type]);
                }

                return formElems.test(this.nodeName);
            },
            remove: function (namespaces, fn) {
                for (var type in changeFilters) {
                    jQuery.event.remove(this, type + ".specialChange" + (fn ? "." + fn.guid : ""), changeFilters[type]);
                }

                return formElems.test(this.nodeName);
            }
        };

        var changeFilters = jQuery.event.special.change.filters;

    }

    function trigger(type, elem, args) {
        args[0].type = type;
        return jQuery.event.handle.apply(elem, args);
    }

    // Create "bubbling" focus and blur events
    if (document.addEventListener) {
        jQuery.each({ focus: "focusin", blur: "focusout" }, function (orig, fix) {
            jQuery.event.special[fix] = {
                setup: function () {
                    this.addEventListener(orig, handler, true);
                },
                teardown: function () {
                    this.removeEventListener(orig, handler, true);
                }
            };

            function handler(e) {
                e = jQuery.event.fix(e);
                e.type = fix;
                return jQuery.event.handle.call(this, e);
            }
        });
    }

    jQuery.each(["bind", "one"], function (i, name) {
        jQuery.fn[name] = function (type, data, fn) {
            // Handle object literals
            if (typeof type === "object") {
                for (var key in type) {
                    this[name](key, data, type[key], fn);
                }
                return this;
            }

            if (jQuery.isFunction(data)) {
                fn = data;
                data = undefined;
            }

            var handler = name === "one" ? jQuery.proxy(fn, function (event) {
                jQuery(this).unbind(event, handler);
                return fn.apply(this, arguments);
            }) : fn;

            return type === "unload" && name !== "one" ?
			this.one(type, data, fn) :
			this.each(function () {
			    jQuery.event.add(this, type, handler, data);
			});
        };
    });

    jQuery.fn.extend({
        unbind: function (type, fn) {
            // Handle object literals
            if (typeof type === "object" && !type.preventDefault) {
                for (var key in type) {
                    this.unbind(key, type[key]);
                }
                return this;
            }

            return this.each(function () {
                jQuery.event.remove(this, type, fn);
            });
        },
        trigger: function (type, data) {
            return this.each(function () {
                jQuery.event.trigger(type, data, this);
            });
        },

        triggerHandler: function (type, data) {
            if (this[0]) {
                var event = jQuery.Event(type);
                event.preventDefault();
                event.stopPropagation();
                jQuery.event.trigger(event, data, this[0]);
                return event.result;
            }
        },

        toggle: function (fn) {
            // Save reference to arguments for access in closure
            var args = arguments, i = 1;

            // link all the functions, so any of them can unbind this click handler
            while (i < args.length) {
                jQuery.proxy(fn, args[i++]);
            }

            return this.click(jQuery.proxy(fn, function (event) {
                // Figure out which function to execute
                var lastToggle = (jQuery.data(this, "lastToggle" + fn.guid) || 0) % i;
                jQuery.data(this, "lastToggle" + fn.guid, lastToggle + 1);

                // Make sure that clicks stop
                event.preventDefault();

                // and execute the function
                return args[lastToggle].apply(this, arguments) || false;
            }));
        },

        hover: function (fnOver, fnOut) {
            return this.mouseenter(fnOver).mouseleave(fnOut || fnOver);
        }
    });

    jQuery.each(["live", "die"], function (i, name) {
        jQuery.fn[name] = function (types, data, fn) {
            var type, i = 0;

            if (jQuery.isFunction(data)) {
                fn = data;
                data = undefined;
            }

            types = (types || "").split(/\s+/);

            while ((type = types[i++]) != null) {
                type = type === "focus" ? "focusin" : // focus --> focusin
					type === "blur" ? "focusout" : // blur --> focusout
					type === "hover" ? types.push("mouseleave") && "mouseenter" : // hover support
					type;

                if (name === "live") {
                    // bind live handler
                    jQuery(this.context).bind(liveConvert(type, this.selector), {
                        data: data, selector: this.selector, live: type
                    }, fn);

                } else {
                    // unbind live handler
                    jQuery(this.context).unbind(liveConvert(type, this.selector), fn ? { guid: fn.guid + this.selector + type} : null);
                }
            }

            return this;
        }
    });

    function liveHandler(event) {
        var stop, elems = [], selectors = [], args = arguments,
		related, match, fn, elem, j, i, l, data,
		live = jQuery.extend({}, jQuery.data(this, "events").live);

        // Make sure we avoid non-left-click bubbling in Firefox (#3861)
        if (event.button && event.type === "click") {
            return;
        }

        for (j in live) {
            fn = live[j];
            if (fn.live === event.type ||
				fn.altLive && jQuery.inArray(event.type, fn.altLive) > -1) {

                data = fn.data;
                if (!(data.beforeFilter && data.beforeFilter[event.type] &&
					!data.beforeFilter[event.type](event))) {
                    selectors.push(fn.selector);
                }
            } else {
                delete live[j];
            }
        }

        match = jQuery(event.target).closest(selectors, event.currentTarget);

        for (i = 0, l = match.length; i < l; i++) {
            for (j in live) {
                fn = live[j];
                elem = match[i].elem;
                related = null;

                if (match[i].selector === fn.selector) {
                    // Those two events require additional checking
                    if (fn.live === "mouseenter" || fn.live === "mouseleave") {
                        related = jQuery(event.relatedTarget).closest(fn.selector)[0];
                    }

                    if (!related || related !== elem) {
                        elems.push({ elem: elem, fn: fn });
                    }
                }
            }
        }

        for (i = 0, l = elems.length; i < l; i++) {
            match = elems[i];
            event.currentTarget = match.elem;
            event.data = match.fn.data;
            if (match.fn.apply(match.elem, args) === false) {
                stop = false;
                break;
            }
        }

        return stop;
    }

    function liveConvert(type, selector) {
        return "live." + (type ? type + "." : "") + selector.replace(/\./g, "`").replace(/ /g, "&");
    }

    jQuery.each(("blur focus focusin focusout load resize scroll unload click dblclick " +
	"mousedown mouseup mousemove mouseover mouseout mouseenter mouseleave " +
	"change select submit keydown keypress keyup error").split(" "), function (i, name) {

	    // Handle event binding
	    jQuery.fn[name] = function (fn) {
	        return fn ? this.bind(name, fn) : this.trigger(name);
	    };

	    if (jQuery.attrFn) {
	        jQuery.attrFn[name] = true;
	    }
	});

    // Prevent memory leaks in IE
    // Window isn't included so as not to unbind existing unload events
    // More info:
    //  - http://isaacschlueter.com/2006/10/msie-memory-leaks/
    if (window.attachEvent && !window.addEventListener) {
        window.attachEvent("onunload", function () {
            for (var id in jQuery.cache) {
                if (jQuery.cache[id].handle) {
                    // Try/Catch is to handle iframes being unloaded, see #4280
                    try {
                        jQuery.event.remove(jQuery.cache[id].handle.elem);
                    } catch (e) { }
                }
            }
        });
    }
    /*!
    * Sizzle CSS Selector Engine - v1.0
    *  Copyright 2009, The Dojo Foundation
    *  More information: http://sizzlejs.com/
    *
    * Permission is hereby granted, free of charge, to any person obtaining
    * a copy of this software and associated documentation files (the
    * "Software"), to deal in the Software without restriction, including
    * without limitation the rights to use, copy, modify, merge, publish,
    * distribute, sublicense, and/or sell copies of the Software, and to
    * permit persons to whom the Software is furnished to do so, subject to
    * the following conditions:
    * 
    * The above copyright notice and this permission notice shall be
    * included in all copies or substantial portions of the Software.
    * 
    * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
    * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
    * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
    * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
    * WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    */
    (function () {

        var chunker = /((?:\((?:\([^()]+\)|[^()]+)+\)|\[(?:\[[^[\]]*\]|['"][^'"]*['"]|[^[\]'"]+)+\]|\\.|[^ >+~,(\[\\]+)+|[>+~])(\s*,\s*)?((?:.|\r|\n)*)/g,
	done = 0,
	toString = Object.prototype.toString,
	hasDuplicate = false,
	baseHasDuplicate = true;

        // Here we check if the JavaScript engine is using some sort of
        // optimization where it does not always call our comparision
        // function. If that is the case, discard the hasDuplicate value.
        //   Thus far that includes Google Chrome.
        [0, 0].sort(function () {
            baseHasDuplicate = false;
            return 0;
        });

        var Sizzle = function (selector, context, results, seed) {
            results = results || [];
            var origContext = context = context || document;

            if (context.nodeType !== 1 && context.nodeType !== 9) {
                return [];
            }

            if (!selector || typeof selector !== "string") {
                return results;
            }

            var parts = [], m, set, checkSet, extra, prune = true, contextXML = isXML(context),
		soFar = selector;

            // Reset the position of the chunker regexp (start from head)
            while ((chunker.exec(""), m = chunker.exec(soFar)) !== null) {
                soFar = m[3];

                parts.push(m[1]);

                if (m[2]) {
                    extra = m[3];
                    break;
                }
            }

            if (parts.length > 1 && origPOS.exec(selector)) {
                if (parts.length === 2 && Expr.relative[parts[0]]) {
                    set = posProcess(parts[0] + parts[1], context);
                } else {
                    set = Expr.relative[parts[0]] ?
				[context] :
				Sizzle(parts.shift(), context);

                    while (parts.length) {
                        selector = parts.shift();

                        if (Expr.relative[selector]) {
                            selector += parts.shift();
                        }

                        set = posProcess(selector, set);
                    }
                }
            } else {
                // Take a shortcut and set the context if the root selector is an ID
                // (but not if it'll be faster if the inner selector is an ID)
                if (!seed && parts.length > 1 && context.nodeType === 9 && !contextXML &&
				Expr.match.ID.test(parts[0]) && !Expr.match.ID.test(parts[parts.length - 1])) {
                    var ret = Sizzle.find(parts.shift(), context, contextXML);
                    context = ret.expr ? Sizzle.filter(ret.expr, ret.set)[0] : ret.set[0];
                }

                if (context) {
                    var ret = seed ?
				{ expr: parts.pop(), set: makeArray(seed)} :
				Sizzle.find(parts.pop(), parts.length === 1 && (parts[0] === "~" || parts[0] === "+") && context.parentNode ? context.parentNode : context, contextXML);
                    set = ret.expr ? Sizzle.filter(ret.expr, ret.set) : ret.set;

                    if (parts.length > 0) {
                        checkSet = makeArray(set);
                    } else {
                        prune = false;
                    }

                    while (parts.length) {
                        var cur = parts.pop(), pop = cur;

                        if (!Expr.relative[cur]) {
                            cur = "";
                        } else {
                            pop = parts.pop();
                        }

                        if (pop == null) {
                            pop = context;
                        }

                        Expr.relative[cur](checkSet, pop, contextXML);
                    }
                } else {
                    checkSet = parts = [];
                }
            }

            if (!checkSet) {
                checkSet = set;
            }

            if (!checkSet) {
                Sizzle.error(cur || selector);
            }

            if (toString.call(checkSet) === "[object Array]") {
                if (!prune) {
                    results.push.apply(results, checkSet);
                } else if (context && context.nodeType === 1) {
                    for (var i = 0; checkSet[i] != null; i++) {
                        if (checkSet[i] && (checkSet[i] === true || checkSet[i].nodeType === 1 && contains(context, checkSet[i]))) {
                            results.push(set[i]);
                        }
                    }
                } else {
                    for (var i = 0; checkSet[i] != null; i++) {
                        if (checkSet[i] && checkSet[i].nodeType === 1) {
                            results.push(set[i]);
                        }
                    }
                }
            } else {
                makeArray(checkSet, results);
            }

            if (extra) {
                Sizzle(extra, origContext, results, seed);
                Sizzle.uniqueSort(results);
            }

            return results;
        };

        Sizzle.uniqueSort = function (results) {
            if (sortOrder) {
                hasDuplicate = baseHasDuplicate;
                results.sort(sortOrder);

                if (hasDuplicate) {
                    for (var i = 1; i < results.length; i++) {
                        if (results[i] === results[i - 1]) {
                            results.splice(i--, 1);
                        }
                    }
                }
            }

            return results;
        };

        Sizzle.matches = function (expr, set) {
            return Sizzle(expr, null, null, set);
        };

        Sizzle.find = function (expr, context, isXML) {
            var set, match;

            if (!expr) {
                return [];
            }

            for (var i = 0, l = Expr.order.length; i < l; i++) {
                var type = Expr.order[i], match;

                if ((match = Expr.leftMatch[type].exec(expr))) {
                    var left = match[1];
                    match.splice(1, 1);

                    if (left.substr(left.length - 1) !== "\\") {
                        match[1] = (match[1] || "").replace(/\\/g, "");
                        set = Expr.find[type](match, context, isXML);
                        if (set != null) {
                            expr = expr.replace(Expr.match[type], "");
                            break;
                        }
                    }
                }
            }

            if (!set) {
                set = context.getElementsByTagName("*");
            }

            return { set: set, expr: expr };
        };

        Sizzle.filter = function (expr, set, inplace, not) {
            var old = expr, result = [], curLoop = set, match, anyFound,
		isXMLFilter = set && set[0] && isXML(set[0]);

            while (expr && set.length) {
                for (var type in Expr.filter) {
                    if ((match = Expr.leftMatch[type].exec(expr)) != null && match[2]) {
                        var filter = Expr.filter[type], found, item, left = match[1];
                        anyFound = false;

                        match.splice(1, 1);

                        if (left.substr(left.length - 1) === "\\") {
                            continue;
                        }

                        if (curLoop === result) {
                            result = [];
                        }

                        if (Expr.preFilter[type]) {
                            match = Expr.preFilter[type](match, curLoop, inplace, result, not, isXMLFilter);

                            if (!match) {
                                anyFound = found = true;
                            } else if (match === true) {
                                continue;
                            }
                        }

                        if (match) {
                            for (var i = 0; (item = curLoop[i]) != null; i++) {
                                if (item) {
                                    found = filter(item, match, i, curLoop);
                                    var pass = not ^ !!found;

                                    if (inplace && found != null) {
                                        if (pass) {
                                            anyFound = true;
                                        } else {
                                            curLoop[i] = false;
                                        }
                                    } else if (pass) {
                                        result.push(item);
                                        anyFound = true;
                                    }
                                }
                            }
                        }

                        if (found !== undefined) {
                            if (!inplace) {
                                curLoop = result;
                            }

                            expr = expr.replace(Expr.match[type], "");

                            if (!anyFound) {
                                return [];
                            }

                            break;
                        }
                    }
                }

                // Improper expression
                if (expr === old) {
                    if (anyFound == null) {
                        Sizzle.error(expr);
                    } else {
                        break;
                    }
                }

                old = expr;
            }

            return curLoop;
        };

        Sizzle.error = function (msg) {
            throw "Syntax error, unrecognized expression: " + msg;
        };

        var Expr = Sizzle.selectors = {
            order: ["ID", "NAME", "TAG"],
            match: {
                ID: /#((?:[\w\u00c0-\uFFFF-]|\\.)+)/,
                CLASS: /\.((?:[\w\u00c0-\uFFFF-]|\\.)+)/,
                NAME: /\[name=['"]*((?:[\w\u00c0-\uFFFF-]|\\.)+)['"]*\]/,
                ATTR: /\[\s*((?:[\w\u00c0-\uFFFF-]|\\.)+)\s*(?:(\S?=)\s*(['"]*)(.*?)\3|)\s*\]/,
                TAG: /^((?:[\w\u00c0-\uFFFF\*-]|\\.)+)/,
                CHILD: /:(only|nth|last|first)-child(?:\((even|odd|[\dn+-]*)\))?/,
                POS: /:(nth|eq|gt|lt|first|last|even|odd)(?:\((\d*)\))?(?=[^-]|$)/,
                PSEUDO: /:((?:[\w\u00c0-\uFFFF-]|\\.)+)(?:\((['"]?)((?:\([^\)]+\)|[^\(\)]*)+)\2\))?/
            },
            leftMatch: {},
            attrMap: {
                "class": "className",
                "for": "htmlFor"
            },
            attrHandle: {
                href: function (elem) {
                    return elem.getAttribute("href");
                }
            },
            relative: {
                "+": function (checkSet, part) {
                    var isPartStr = typeof part === "string",
				isTag = isPartStr && !/\W/.test(part),
				isPartStrNotTag = isPartStr && !isTag;

                    if (isTag) {
                        part = part.toLowerCase();
                    }

                    for (var i = 0, l = checkSet.length, elem; i < l; i++) {
                        if ((elem = checkSet[i])) {
                            while ((elem = elem.previousSibling) && elem.nodeType !== 1) { }

                            checkSet[i] = isPartStrNotTag || elem && elem.nodeName.toLowerCase() === part ?
						elem || false :
						elem === part;
                        }
                    }

                    if (isPartStrNotTag) {
                        Sizzle.filter(part, checkSet, true);
                    }
                },
                ">": function (checkSet, part) {
                    var isPartStr = typeof part === "string";

                    if (isPartStr && !/\W/.test(part)) {
                        part = part.toLowerCase();

                        for (var i = 0, l = checkSet.length; i < l; i++) {
                            var elem = checkSet[i];
                            if (elem) {
                                var parent = elem.parentNode;
                                checkSet[i] = parent.nodeName.toLowerCase() === part ? parent : false;
                            }
                        }
                    } else {
                        for (var i = 0, l = checkSet.length; i < l; i++) {
                            var elem = checkSet[i];
                            if (elem) {
                                checkSet[i] = isPartStr ?
							elem.parentNode :
							elem.parentNode === part;
                            }
                        }

                        if (isPartStr) {
                            Sizzle.filter(part, checkSet, true);
                        }
                    }
                },
                "": function (checkSet, part, isXML) {
                    var doneName = done++, checkFn = dirCheck;

                    if (typeof part === "string" && !/\W/.test(part)) {
                        var nodeCheck = part = part.toLowerCase();
                        checkFn = dirNodeCheck;
                    }

                    checkFn("parentNode", part, doneName, checkSet, nodeCheck, isXML);
                },
                "~": function (checkSet, part, isXML) {
                    var doneName = done++, checkFn = dirCheck;

                    if (typeof part === "string" && !/\W/.test(part)) {
                        var nodeCheck = part = part.toLowerCase();
                        checkFn = dirNodeCheck;
                    }

                    checkFn("previousSibling", part, doneName, checkSet, nodeCheck, isXML);
                }
            },
            find: {
                ID: function (match, context, isXML) {
                    if (typeof context.getElementById !== "undefined" && !isXML) {
                        var m = context.getElementById(match[1]);
                        return m ? [m] : [];
                    }
                },
                NAME: function (match, context) {
                    if (typeof context.getElementsByName !== "undefined") {
                        var ret = [], results = context.getElementsByName(match[1]);

                        for (var i = 0, l = results.length; i < l; i++) {
                            if (results[i].getAttribute("name") === match[1]) {
                                ret.push(results[i]);
                            }
                        }

                        return ret.length === 0 ? null : ret;
                    }
                },
                TAG: function (match, context) {
                    return context.getElementsByTagName(match[1]);
                }
            },
            preFilter: {
                CLASS: function (match, curLoop, inplace, result, not, isXML) {
                    match = " " + match[1].replace(/\\/g, "") + " ";

                    if (isXML) {
                        return match;
                    }

                    for (var i = 0, elem; (elem = curLoop[i]) != null; i++) {
                        if (elem) {
                            if (not ^ (elem.className && (" " + elem.className + " ").replace(/[\t\n]/g, " ").indexOf(match) >= 0)) {
                                if (!inplace) {
                                    result.push(elem);
                                }
                            } else if (inplace) {
                                curLoop[i] = false;
                            }
                        }
                    }

                    return false;
                },
                ID: function (match) {
                    return match[1].replace(/\\/g, "");
                },
                TAG: function (match, curLoop) {
                    return match[1].toLowerCase();
                },
                CHILD: function (match) {
                    if (match[1] === "nth") {
                        // parse equations like 'even', 'odd', '5', '2n', '3n+2', '4n-1', '-n+6'
                        var test = /(-?)(\d*)n((?:\+|-)?\d*)/.exec(
					match[2] === "even" && "2n" || match[2] === "odd" && "2n+1" ||
					!/\D/.test(match[2]) && "0n+" + match[2] || match[2]);

                        // calculate the numbers (first)n+(last) including if they are negative
                        match[2] = (test[1] + (test[2] || 1)) - 0;
                        match[3] = test[3] - 0;
                    }

                    // TODO: Move to normal caching system
                    match[0] = done++;

                    return match;
                },
                ATTR: function (match, curLoop, inplace, result, not, isXML) {
                    var name = match[1].replace(/\\/g, "");

                    if (!isXML && Expr.attrMap[name]) {
                        match[1] = Expr.attrMap[name];
                    }

                    if (match[2] === "~=") {
                        match[4] = " " + match[4] + " ";
                    }

                    return match;
                },
                PSEUDO: function (match, curLoop, inplace, result, not) {
                    if (match[1] === "not") {
                        // If we're dealing with a complex expression, or a simple one
                        if ((chunker.exec(match[3]) || "").length > 1 || /^\w/.test(match[3])) {
                            match[3] = Sizzle(match[3], null, null, curLoop);
                        } else {
                            var ret = Sizzle.filter(match[3], curLoop, inplace, true ^ not);
                            if (!inplace) {
                                result.push.apply(result, ret);
                            }
                            return false;
                        }
                    } else if (Expr.match.POS.test(match[0]) || Expr.match.CHILD.test(match[0])) {
                        return true;
                    }

                    return match;
                },
                POS: function (match) {
                    match.unshift(true);
                    return match;
                }
            },
            filters: {
                enabled: function (elem) {
                    return elem.disabled === false && elem.type !== "hidden";
                },
                disabled: function (elem) {
                    return elem.disabled === true;
                },
                checked: function (elem) {
                    return elem.checked === true;
                },
                selected: function (elem) {
                    // Accessing this property makes selected-by-default
                    // options in Safari work properly
                    elem.parentNode.selectedIndex;
                    return elem.selected === true;
                },
                parent: function (elem) {
                    return !!elem.firstChild;
                },
                empty: function (elem) {
                    return !elem.firstChild;
                },
                has: function (elem, i, match) {
                    return !!Sizzle(match[3], elem).length;
                },
                header: function (elem) {
                    return /h\d/i.test(elem.nodeName);
                },
                text: function (elem) {
                    return "text" === elem.type;
                },
                radio: function (elem) {
                    return "radio" === elem.type;
                },
                checkbox: function (elem) {
                    return "checkbox" === elem.type;
                },
                file: function (elem) {
                    return "file" === elem.type;
                },
                password: function (elem) {
                    return "password" === elem.type;
                },
                submit: function (elem) {
                    return "submit" === elem.type;
                },
                image: function (elem) {
                    return "image" === elem.type;
                },
                reset: function (elem) {
                    return "reset" === elem.type;
                },
                button: function (elem) {
                    return "button" === elem.type || elem.nodeName.toLowerCase() === "button";
                },
                input: function (elem) {
                    return /input|select|textarea|button/i.test(elem.nodeName);
                }
            },
            setFilters: {
                first: function (elem, i) {
                    return i === 0;
                },
                last: function (elem, i, match, array) {
                    return i === array.length - 1;
                },
                even: function (elem, i) {
                    return i % 2 === 0;
                },
                odd: function (elem, i) {
                    return i % 2 === 1;
                },
                lt: function (elem, i, match) {
                    return i < match[3] - 0;
                },
                gt: function (elem, i, match) {
                    return i > match[3] - 0;
                },
                nth: function (elem, i, match) {
                    return match[3] - 0 === i;
                },
                eq: function (elem, i, match) {
                    return match[3] - 0 === i;
                }
            },
            filter: {
                PSEUDO: function (elem, match, i, array) {
                    var name = match[1], filter = Expr.filters[name];

                    if (filter) {
                        return filter(elem, i, match, array);
                    } else if (name === "contains") {
                        return (elem.textContent || elem.innerText || getText([elem]) || "").indexOf(match[3]) >= 0;
                    } else if (name === "not") {
                        var not = match[3];

                        for (var i = 0, l = not.length; i < l; i++) {
                            if (not[i] === elem) {
                                return false;
                            }
                        }

                        return true;
                    } else {
                        Sizzle.error("Syntax error, unrecognized expression: " + name);
                    }
                },
                CHILD: function (elem, match) {
                    var type = match[1], node = elem;
                    switch (type) {
                        case 'only':
                        case 'first':
                            while ((node = node.previousSibling)) {
                                if (node.nodeType === 1) {
                                    return false;
                                }
                            }
                            if (type === "first") {
                                return true;
                            }
                            node = elem;
                        case 'last':
                            while ((node = node.nextSibling)) {
                                if (node.nodeType === 1) {
                                    return false;
                                }
                            }
                            return true;
                        case 'nth':
                            var first = match[2], last = match[3];

                            if (first === 1 && last === 0) {
                                return true;
                            }

                            var doneName = match[0],
						parent = elem.parentNode;

                            if (parent && (parent.sizcache !== doneName || !elem.nodeIndex)) {
                                var count = 0;
                                for (node = parent.firstChild; node; node = node.nextSibling) {
                                    if (node.nodeType === 1) {
                                        node.nodeIndex = ++count;
                                    }
                                }
                                parent.sizcache = doneName;
                            }

                            var diff = elem.nodeIndex - last;
                            if (first === 0) {
                                return diff === 0;
                            } else {
                                return (diff % first === 0 && diff / first >= 0);
                            }
                    }
                },
                ID: function (elem, match) {
                    return elem.nodeType === 1 && elem.getAttribute("id") === match;
                },
                TAG: function (elem, match) {
                    return (match === "*" && elem.nodeType === 1) || elem.nodeName.toLowerCase() === match;
                },
                CLASS: function (elem, match) {
                    return (" " + (elem.className || elem.getAttribute("class")) + " ")
				.indexOf(match) > -1;
                },
                ATTR: function (elem, match) {
                    var name = match[1],
				result = Expr.attrHandle[name] ?
					Expr.attrHandle[name](elem) :
					elem[name] != null ?
						elem[name] :
						elem.getAttribute(name),
				value = result + "",
				type = match[2],
				check = match[4];

                    return result == null ?
				type === "!=" :
				type === "=" ?
				value === check :
				type === "*=" ?
				value.indexOf(check) >= 0 :
				type === "~=" ?
				(" " + value + " ").indexOf(check) >= 0 :
				!check ?
				value && result !== false :
				type === "!=" ?
				value !== check :
				type === "^=" ?
				value.indexOf(check) === 0 :
				type === "$=" ?
				value.substr(value.length - check.length) === check :
				type === "|=" ?
				value === check || value.substr(0, check.length + 1) === check + "-" :
				false;
                },
                POS: function (elem, match, i, array) {
                    var name = match[2], filter = Expr.setFilters[name];

                    if (filter) {
                        return filter(elem, i, match, array);
                    }
                }
            }
        };

        var origPOS = Expr.match.POS;

        for (var type in Expr.match) {
            Expr.match[type] = new RegExp(Expr.match[type].source + /(?![^\[]*\])(?![^\(]*\))/.source);
            Expr.leftMatch[type] = new RegExp(/(^(?:.|\r|\n)*?)/.source + Expr.match[type].source.replace(/\\(\d+)/g, function (all, num) {
                return "\\" + (num - 0 + 1);
            }));
        }

        var makeArray = function (array, results) {
            array = Array.prototype.slice.call(array, 0);

            if (results) {
                results.push.apply(results, array);
                return results;
            }

            return array;
        };

        // Perform a simple check to determine if the browser is capable of
        // converting a NodeList to an array using builtin methods.
        try {
            Array.prototype.slice.call(document.documentElement.childNodes, 0);

            // Provide a fallback method if it does not work
        } catch (e) {
            makeArray = function (array, results) {
                var ret = results || [];

                if (toString.call(array) === "[object Array]") {
                    Array.prototype.push.apply(ret, array);
                } else {
                    if (typeof array.length === "number") {
                        for (var i = 0, l = array.length; i < l; i++) {
                            ret.push(array[i]);
                        }
                    } else {
                        for (var i = 0; array[i]; i++) {
                            ret.push(array[i]);
                        }
                    }
                }

                return ret;
            };
        }

        var sortOrder;

        if (document.documentElement.compareDocumentPosition) {
            sortOrder = function (a, b) {
                if (!a.compareDocumentPosition || !b.compareDocumentPosition) {
                    if (a == b) {
                        hasDuplicate = true;
                    }
                    return a.compareDocumentPosition ? -1 : 1;
                }

                var ret = a.compareDocumentPosition(b) & 4 ? -1 : a === b ? 0 : 1;
                if (ret === 0) {
                    hasDuplicate = true;
                }
                return ret;
            };
        } else if ("sourceIndex" in document.documentElement) {
            sortOrder = function (a, b) {
                if (!a.sourceIndex || !b.sourceIndex) {
                    if (a == b) {
                        hasDuplicate = true;
                    }
                    return a.sourceIndex ? -1 : 1;
                }

                var ret = a.sourceIndex - b.sourceIndex;
                if (ret === 0) {
                    hasDuplicate = true;
                }
                return ret;
            };
        } else if (document.createRange) {
            sortOrder = function (a, b) {
                if (!a.ownerDocument || !b.ownerDocument) {
                    if (a == b) {
                        hasDuplicate = true;
                    }
                    return a.ownerDocument ? -1 : 1;
                }

                var aRange = a.ownerDocument.createRange(), bRange = b.ownerDocument.createRange();
                aRange.setStart(a, 0);
                aRange.setEnd(a, 0);
                bRange.setStart(b, 0);
                bRange.setEnd(b, 0);
                var ret = aRange.compareBoundaryPoints(Range.START_TO_END, bRange);
                if (ret === 0) {
                    hasDuplicate = true;
                }
                return ret;
            };
        }

        // Utility function for retreiving the text value of an array of DOM nodes
        function getText(elems) {
            var ret = "", elem;

            for (var i = 0; elems[i]; i++) {
                elem = elems[i];

                // Get the text from text nodes and CDATA nodes
                if (elem.nodeType === 3 || elem.nodeType === 4) {
                    ret += elem.nodeValue;

                    // Traverse everything else, except comment nodes
                } else if (elem.nodeType !== 8) {
                    ret += getText(elem.childNodes);
                }
            }

            return ret;
        }

        // Check to see if the browser returns elements by name when
        // querying by getElementById (and provide a workaround)
        (function () {
            // We're going to inject a fake input element with a specified name
            var form = document.createElement("div"),
		id = "script" + (new Date).getTime();
            form.innerHTML = "<a name='" + id + "'/>";

            // Inject it into the root element, check its status, and remove it quickly
            var root = document.documentElement;
            root.insertBefore(form, root.firstChild);

            // The workaround has to do additional checks after a getElementById
            // Which slows things down for other browsers (hence the branching)
            if (document.getElementById(id)) {
                Expr.find.ID = function (match, context, isXML) {
                    if (typeof context.getElementById !== "undefined" && !isXML) {
                        var m = context.getElementById(match[1]);
                        return m ? m.id === match[1] || typeof m.getAttributeNode !== "undefined" && m.getAttributeNode("id").nodeValue === match[1] ? [m] : undefined : [];
                    }
                };

                Expr.filter.ID = function (elem, match) {
                    var node = typeof elem.getAttributeNode !== "undefined" && elem.getAttributeNode("id");
                    return elem.nodeType === 1 && node && node.nodeValue === match;
                };
            }

            root.removeChild(form);
            root = form = null; // release memory in IE
        })();

        (function () {
            // Check to see if the browser returns only elements
            // when doing getElementsByTagName("*")

            // Create a fake element
            var div = document.createElement("div");
            div.appendChild(document.createComment(""));

            // Make sure no comments are found
            if (div.getElementsByTagName("*").length > 0) {
                Expr.find.TAG = function (match, context) {
                    var results = context.getElementsByTagName(match[1]);

                    // Filter out possible comments
                    if (match[1] === "*") {
                        var tmp = [];

                        for (var i = 0; results[i]; i++) {
                            if (results[i].nodeType === 1) {
                                tmp.push(results[i]);
                            }
                        }

                        results = tmp;
                    }

                    return results;
                };
            }

            // Check to see if an attribute returns normalized href attributes
            div.innerHTML = "<a href='#'></a>";
            if (div.firstChild && typeof div.firstChild.getAttribute !== "undefined" &&
			div.firstChild.getAttribute("href") !== "#") {
                Expr.attrHandle.href = function (elem) {
                    return elem.getAttribute("href", 2);
                };
            }

            div = null; // release memory in IE
        })();

        if (document.querySelectorAll) {
            (function () {
                var oldSizzle = Sizzle, div = document.createElement("div");
                div.innerHTML = "<p class='TEST'></p>";

                // Safari can't handle uppercase or unicode characters when
                // in quirks mode.
                if (div.querySelectorAll && div.querySelectorAll(".TEST").length === 0) {
                    return;
                }

                Sizzle = function (query, context, extra, seed) {
                    context = context || document;

                    // Only use querySelectorAll on non-XML documents
                    // (ID selectors don't work in non-HTML documents)
                    if (!seed && context.nodeType === 9 && !isXML(context)) {
                        try {
                            return makeArray(context.querySelectorAll(query), extra);
                        } catch (e) { }
                    }

                    return oldSizzle(query, context, extra, seed);
                };

                for (var prop in oldSizzle) {
                    Sizzle[prop] = oldSizzle[prop];
                }

                div = null; // release memory in IE
            })();
        }

        (function () {
            var div = document.createElement("div");

            div.innerHTML = "<div class='test e'></div><div class='test'></div>";

            // Opera can't find a second classname (in 9.6)
            // Also, make sure that getElementsByClassName actually exists
            if (!div.getElementsByClassName || div.getElementsByClassName("e").length === 0) {
                return;
            }

            // Safari caches class attributes, doesn't catch changes (in 3.2)
            div.lastChild.className = "e";

            if (div.getElementsByClassName("e").length === 1) {
                return;
            }

            Expr.order.splice(1, 0, "CLASS");
            Expr.find.CLASS = function (match, context, isXML) {
                if (typeof context.getElementsByClassName !== "undefined" && !isXML) {
                    return context.getElementsByClassName(match[1]);
                }
            };

            div = null; // release memory in IE
        })();

        function dirNodeCheck(dir, cur, doneName, checkSet, nodeCheck, isXML) {
            for (var i = 0, l = checkSet.length; i < l; i++) {
                var elem = checkSet[i];
                if (elem) {
                    elem = elem[dir];
                    var match = false;

                    while (elem) {
                        if (elem.sizcache === doneName) {
                            match = checkSet[elem.sizset];
                            break;
                        }

                        if (elem.nodeType === 1 && !isXML) {
                            elem.sizcache = doneName;
                            elem.sizset = i;
                        }

                        if (elem.nodeName.toLowerCase() === cur) {
                            match = elem;
                            break;
                        }

                        elem = elem[dir];
                    }

                    checkSet[i] = match;
                }
            }
        }

        function dirCheck(dir, cur, doneName, checkSet, nodeCheck, isXML) {
            for (var i = 0, l = checkSet.length; i < l; i++) {
                var elem = checkSet[i];
                if (elem) {
                    elem = elem[dir];
                    var match = false;

                    while (elem) {
                        if (elem.sizcache === doneName) {
                            match = checkSet[elem.sizset];
                            break;
                        }

                        if (elem.nodeType === 1) {
                            if (!isXML) {
                                elem.sizcache = doneName;
                                elem.sizset = i;
                            }
                            if (typeof cur !== "string") {
                                if (elem === cur) {
                                    match = true;
                                    break;
                                }

                            } else if (Sizzle.filter(cur, [elem]).length > 0) {
                                match = elem;
                                break;
                            }
                        }

                        elem = elem[dir];
                    }

                    checkSet[i] = match;
                }
            }
        }

        var contains = document.compareDocumentPosition ? function (a, b) {
            return a.compareDocumentPosition(b) & 16;
        } : function (a, b) {
            return a !== b && (a.contains ? a.contains(b) : true);
        };

        var isXML = function (elem) {
            // documentElement is verified for cases where it doesn't yet exist
            // (such as loading iframes in IE - #4833) 
            var documentElement = (elem ? elem.ownerDocument || elem : 0).documentElement;
            return documentElement ? documentElement.nodeName !== "HTML" : false;
        };

        var posProcess = function (selector, context) {
            var tmpSet = [], later = "", match,
		root = context.nodeType ? [context] : context;

            // Position selectors must be done after the filter
            // And so must :not(positional) so we move all PSEUDOs to the end
            while ((match = Expr.match.PSEUDO.exec(selector))) {
                later += match[0];
                selector = selector.replace(Expr.match.PSEUDO, "");
            }

            selector = Expr.relative[selector] ? selector + "*" : selector;

            for (var i = 0, l = root.length; i < l; i++) {
                Sizzle(selector, root[i], tmpSet);
            }

            return Sizzle.filter(later, tmpSet);
        };

        // EXPOSE
        jQuery.find = Sizzle;
        jQuery.expr = Sizzle.selectors;
        jQuery.expr[":"] = jQuery.expr.filters;
        jQuery.unique = Sizzle.uniqueSort;
        jQuery.getText = getText;
        jQuery.isXMLDoc = isXML;
        jQuery.contains = contains;

        return;

        window.Sizzle = Sizzle;

    })();
    var runtil = /Until$/,
	rparentsprev = /^(?:parents|prevUntil|prevAll)/,
    // Note: This RegExp should be improved, or likely pulled from Sizzle
	rmultiselector = /,/,
	slice = Array.prototype.slice;

    // Implement the identical functionality for filter and not
    var winnow = function (elements, qualifier, keep) {
        if (jQuery.isFunction(qualifier)) {
            return jQuery.grep(elements, function (elem, i) {
                return !!qualifier.call(elem, i, elem) === keep;
            });

        } else if (qualifier.nodeType) {
            return jQuery.grep(elements, function (elem, i) {
                return (elem === qualifier) === keep;
            });

        } else if (typeof qualifier === "string") {
            var filtered = jQuery.grep(elements, function (elem) {
                return elem.nodeType === 1;
            });

            if (isSimple.test(qualifier)) {
                return jQuery.filter(qualifier, filtered, !keep);
            } else {
                qualifier = jQuery.filter(qualifier, filtered);
            }
        }

        return jQuery.grep(elements, function (elem, i) {
            return (jQuery.inArray(elem, qualifier) >= 0) === keep;
        });
    };

    jQuery.fn.extend({
        find: function (selector) {
            var ret = this.pushStack("", "find", selector), length = 0;

            for (var i = 0, l = this.length; i < l; i++) {
                length = ret.length;
                jQuery.find(selector, this[i], ret);

                if (i > 0) {
                    // Make sure that the results are unique
                    for (var n = length; n < ret.length; n++) {
                        for (var r = 0; r < length; r++) {
                            if (ret[r] === ret[n]) {
                                ret.splice(n--, 1);
                                break;
                            }
                        }
                    }
                }
            }

            return ret;
        },

        has: function (target) {
            var targets = jQuery(target);
            return this.filter(function () {
                for (var i = 0, l = targets.length; i < l; i++) {
                    if (jQuery.contains(this, targets[i])) {
                        return true;
                    }
                }
            });
        },

        not: function (selector) {
            return this.pushStack(winnow(this, selector, false), "not", selector);
        },

        filter: function (selector) {
            return this.pushStack(winnow(this, selector, true), "filter", selector);
        },

        is: function (selector) {
            return !!selector && jQuery.filter(selector, this).length > 0;
        },

        closest: function (selectors, context) {
            if (jQuery.isArray(selectors)) {
                var ret = [], cur = this[0], match, matches = {}, selector;

                if (cur && selectors.length) {
                    for (var i = 0, l = selectors.length; i < l; i++) {
                        selector = selectors[i];

                        if (!matches[selector]) {
                            matches[selector] = jQuery.expr.match.POS.test(selector) ?
							jQuery(selector, context || this.context) :
							selector;
                        }
                    }

                    while (cur && cur.ownerDocument && cur !== context) {
                        for (selector in matches) {
                            match = matches[selector];

                            if (match.jquery ? match.index(cur) > -1 : jQuery(cur).is(match)) {
                                ret.push({ selector: selector, elem: cur });
                                delete matches[selector];
                            }
                        }
                        cur = cur.parentNode;
                    }
                }

                return ret;
            }

            var pos = jQuery.expr.match.POS.test(selectors) ?
			jQuery(selectors, context || this.context) : null;

            return this.map(function (i, cur) {
                while (cur && cur.ownerDocument && cur !== context) {
                    if (pos ? pos.index(cur) > -1 : jQuery(cur).is(selectors)) {
                        return cur;
                    }
                    cur = cur.parentNode;
                }
                return null;
            });
        },

        // Determine the position of an element within
        // the matched set of elements
        index: function (elem) {
            if (!elem || typeof elem === "string") {
                return jQuery.inArray(this[0],
                // If it receives a string, the selector is used
                // If it receives nothing, the siblings are used
				elem ? jQuery(elem) : this.parent().children());
            }
            // Locate the position of the desired element
            return jQuery.inArray(
            // If it receives a jQuery object, the first element is used
			elem.jquery ? elem[0] : elem, this);
        },

        add: function (selector, context) {
            var set = typeof selector === "string" ?
				jQuery(selector, context || this.context) :
				jQuery.makeArray(selector),
			all = jQuery.merge(this.get(), set);

            return this.pushStack(isDisconnected(set[0]) || isDisconnected(all[0]) ?
			all :
			jQuery.unique(all));
        },

        andSelf: function () {
            return this.add(this.prevObject);
        }
    });

    // A painfully simple check to see if an element is disconnected
    // from a document (should be improved, where feasible).
    function isDisconnected(node) {
        return !node || !node.parentNode || node.parentNode.nodeType === 11;
    }

    jQuery.each({
        parent: function (elem) {
            var parent = elem.parentNode;
            return parent && parent.nodeType !== 11 ? parent : null;
        },
        parents: function (elem) {
            return jQuery.dir(elem, "parentNode");
        },
        parentsUntil: function (elem, i, until) {
            return jQuery.dir(elem, "parentNode", until);
        },
        next: function (elem) {
            return jQuery.nth(elem, 2, "nextSibling");
        },
        prev: function (elem) {
            return jQuery.nth(elem, 2, "previousSibling");
        },
        nextAll: function (elem) {
            return jQuery.dir(elem, "nextSibling");
        },
        prevAll: function (elem) {
            return jQuery.dir(elem, "previousSibling");
        },
        nextUntil: function (elem, i, until) {
            return jQuery.dir(elem, "nextSibling", until);
        },
        prevUntil: function (elem, i, until) {
            return jQuery.dir(elem, "previousSibling", until);
        },
        siblings: function (elem) {
            return jQuery.sibling(elem.parentNode.firstChild, elem);
        },
        children: function (elem) {
            return jQuery.sibling(elem.firstChild);
        },
        contents: function (elem) {
            return jQuery.nodeName(elem, "iframe") ?
			elem.contentDocument || elem.contentWindow.document :
			jQuery.makeArray(elem.childNodes);
        }
    }, function (name, fn) {
        jQuery.fn[name] = function (until, selector) {
            var ret = jQuery.map(this, fn, until);

            if (!runtil.test(name)) {
                selector = until;
            }

            if (selector && typeof selector === "string") {
                ret = jQuery.filter(selector, ret);
            }

            ret = this.length > 1 ? jQuery.unique(ret) : ret;

            if ((this.length > 1 || rmultiselector.test(selector)) && rparentsprev.test(name)) {
                ret = ret.reverse();
            }

            return this.pushStack(ret, name, slice.call(arguments).join(","));
        };
    });

    jQuery.extend({
        filter: function (expr, elems, not) {
            if (not) {
                expr = ":not(" + expr + ")";
            }

            return jQuery.find.matches(expr, elems);
        },

        dir: function (elem, dir, until) {
            var matched = [], cur = elem[dir];
            while (cur && cur.nodeType !== 9 && (until === undefined || cur.nodeType !== 1 || !jQuery(cur).is(until))) {
                if (cur.nodeType === 1) {
                    matched.push(cur);
                }
                cur = cur[dir];
            }
            return matched;
        },

        nth: function (cur, result, dir, elem) {
            result = result || 1;
            var num = 0;

            for (; cur; cur = cur[dir]) {
                if (cur.nodeType === 1 && ++num === result) {
                    break;
                }
            }

            return cur;
        },

        sibling: function (n, elem) {
            var r = [];

            for (; n; n = n.nextSibling) {
                if (n.nodeType === 1 && n !== elem) {
                    r.push(n);
                }
            }

            return r;
        }
    });
    var rinlinejQuery = / jQuery\d+="(?:\d+|null)"/g,
	rleadingWhitespace = /^\s+/,
	rxhtmlTag = /(<([\w:]+)[^>]*?)\/>/g,
	rselfClosing = /^(?:area|br|col|embed|hr|img|input|link|meta|param)$/i,
	rtagName = /<([\w:]+)/,
	rtbody = /<tbody/i,
	rhtml = /<|&\w+;/,
	rchecked = /checked\s*(?:[^=]|=\s*.checked.)/i,  // checked="checked" or checked (html5)
	fcloseTag = function (all, front, tag) {
	    return rselfClosing.test(tag) ?
			all :
			front + "></" + tag + ">";
	},
	wrapMap = {
	    option: [1, "<select multiple='multiple'>", "</select>"],
	    legend: [1, "<fieldset>", "</fieldset>"],
	    thead: [1, "<table>", "</table>"],
	    tr: [2, "<table><tbody>", "</tbody></table>"],
	    td: [3, "<table><tbody><tr>", "</tr></tbody></table>"],
	    col: [2, "<table><tbody></tbody><colgroup>", "</colgroup></table>"],
	    area: [1, "<map>", "</map>"],
	    _default: [0, "", ""]
	};

    wrapMap.optgroup = wrapMap.option;
    wrapMap.tbody = wrapMap.tfoot = wrapMap.colgroup = wrapMap.caption = wrapMap.thead;
    wrapMap.th = wrapMap.td;

    // IE can't serialize <link> and <script> tags normally
    if (!jQuery.support.htmlSerialize) {
        wrapMap._default = [1, "div<div>", "</div>"];
    }

    jQuery.fn.extend({
        text: function (text) {
            if (jQuery.isFunction(text)) {
                return this.each(function (i) {
                    var self = jQuery(this);
                    self.text(text.call(this, i, self.text()));
                });
            }

            if (typeof text !== "object" && text !== undefined) {
                return this.empty().append((this[0] && this[0].ownerDocument || document).createTextNode(text));
            }

            return jQuery.getText(this);
        },

        wrapAll: function (html) {
            if (jQuery.isFunction(html)) {
                return this.each(function (i) {
                    jQuery(this).wrapAll(html.call(this, i));
                });
            }

            if (this[0]) {
                // The elements to wrap the target around
                var wrap = jQuery(html, this[0].ownerDocument).eq(0).clone(true);

                if (this[0].parentNode) {
                    wrap.insertBefore(this[0]);
                }

                wrap.map(function () {
                    var elem = this;

                    while (elem.firstChild && elem.firstChild.nodeType === 1) {
                        elem = elem.firstChild;
                    }

                    return elem;
                }).append(this);
            }

            return this;
        },

        wrapInner: function (html) {
            if (jQuery.isFunction(html)) {
                return this.each(function (i) {
                    jQuery(this).wrapInner(html.call(this, i));
                });
            }

            return this.each(function () {
                var self = jQuery(this), contents = self.contents();

                if (contents.length) {
                    contents.wrapAll(html);

                } else {
                    self.append(html);
                }
            });
        },

        wrap: function (html) {
            return this.each(function () {
                jQuery(this).wrapAll(html);
            });
        },

        unwrap: function () {
            return this.parent().each(function () {
                if (!jQuery.nodeName(this, "body")) {
                    jQuery(this).replaceWith(this.childNodes);
                }
            }).end();
        },

        append: function () {
            return this.domManip(arguments, true, function (elem) {
                if (this.nodeType === 1) {
                    this.appendChild(elem);
                }
            });
        },

        prepend: function () {
            return this.domManip(arguments, true, function (elem) {
                if (this.nodeType === 1) {
                    this.insertBefore(elem, this.firstChild);
                }
            });
        },

        before: function () {
            if (this[0] && this[0].parentNode) {
                return this.domManip(arguments, false, function (elem) {
                    this.parentNode.insertBefore(elem, this);
                });
            } else if (arguments.length) {
                var set = jQuery(arguments[0]);
                set.push.apply(set, this.toArray());
                return this.pushStack(set, "before", arguments);
            }
        },

        after: function () {
            if (this[0] && this[0].parentNode) {
                return this.domManip(arguments, false, function (elem) {
                    this.parentNode.insertBefore(elem, this.nextSibling);
                });
            } else if (arguments.length) {
                var set = this.pushStack(this, "after", arguments);
                set.push.apply(set, jQuery(arguments[0]).toArray());
                return set;
            }
        },

        clone: function (events) {
            // Do the clone
            var ret = this.map(function () {
                if (!jQuery.support.noCloneEvent && !jQuery.isXMLDoc(this)) {
                    // IE copies events bound via attachEvent when
                    // using cloneNode. Calling detachEvent on the
                    // clone will also remove the events from the orignal
                    // In order to get around this, we use innerHTML.
                    // Unfortunately, this means some modifications to
                    // attributes in IE that are actually only stored
                    // as properties will not be copied (such as the
                    // the name attribute on an input).
                    var html = this.outerHTML, ownerDocument = this.ownerDocument;
                    if (!html) {
                        var div = ownerDocument.createElement("div");
                        div.appendChild(this.cloneNode(true));
                        html = div.innerHTML;
                    }

                    return jQuery.clean([html.replace(rinlinejQuery, "")
					.replace(rleadingWhitespace, "")], ownerDocument)[0];
                } else {
                    return this.cloneNode(true);
                }
            });

            // Copy the events from the original to the clone
            if (events === true) {
                cloneCopyEvent(this, ret);
                cloneCopyEvent(this.find("*"), ret.find("*"));
            }

            // Return the cloned set
            return ret;
        },

        html: function (value) {
            if (value === undefined) {
                return this[0] && this[0].nodeType === 1 ?
				this[0].innerHTML.replace(rinlinejQuery, "") :
				null;

                // See if we can take a shortcut and just use innerHTML
            } else if (typeof value === "string" && !/<script/i.test(value) &&
			(jQuery.support.leadingWhitespace || !rleadingWhitespace.test(value)) &&
			!wrapMap[(rtagName.exec(value) || ["", ""])[1].toLowerCase()]) {

                value = value.replace(rxhtmlTag, fcloseTag);

                try {
                    for (var i = 0, l = this.length; i < l; i++) {
                        // Remove element nodes and prevent memory leaks
                        if (this[i].nodeType === 1) {
                            jQuery.cleanData(this[i].getElementsByTagName("*"));
                            this[i].innerHTML = value;
                        }
                    }

                    // If using innerHTML throws an exception, use the fallback method
                } catch (e) {
                    this.empty().append(value);
                }

            } else if (jQuery.isFunction(value)) {
                this.each(function (i) {
                    var self = jQuery(this), old = self.html();
                    self.empty().append(function () {
                        return value.call(this, i, old);
                    });
                });

            } else {
                this.empty().append(value);
            }

            return this;
        },

        replaceWith: function (value) {
            if (this[0] && this[0].parentNode) {
                // Make sure that the elements are removed from the DOM before they are inserted
                // this can help fix replacing a parent with child elements
                if (!jQuery.isFunction(value)) {
                    value = jQuery(value).detach();

                } else {
                    return this.each(function (i) {
                        var self = jQuery(this), old = self.html();
                        self.replaceWith(value.call(this, i, old));
                    });
                }

                return this.each(function () {
                    var next = this.nextSibling, parent = this.parentNode;

                    jQuery(this).remove();

                    if (next) {
                        jQuery(next).before(value);
                    } else {
                        jQuery(parent).append(value);
                    }
                });
            } else {
                return this.pushStack(jQuery(jQuery.isFunction(value) ? value() : value), "replaceWith", value);
            }
        },

        detach: function (selector) {
            return this.remove(selector, true);
        },

        domManip: function (args, table, callback) {
            var results, first, value = args[0], scripts = [];

            // We can't cloneNode fragments that contain checked, in WebKit
            if (!jQuery.support.checkClone && arguments.length === 3 && typeof value === "string" && rchecked.test(value)) {
                return this.each(function () {
                    jQuery(this).domManip(args, table, callback, true);
                });
            }

            if (jQuery.isFunction(value)) {
                return this.each(function (i) {
                    var self = jQuery(this);
                    args[0] = value.call(this, i, table ? self.html() : undefined);
                    self.domManip(args, table, callback);
                });
            }

            if (this[0]) {
                // If we're in a fragment, just use that instead of building a new one
                if (args[0] && args[0].parentNode && args[0].parentNode.nodeType === 11) {
                    results = { fragment: args[0].parentNode };
                } else {
                    results = buildFragment(args, this, scripts);
                }

                first = results.fragment.firstChild;

                if (first) {
                    table = table && jQuery.nodeName(first, "tr");

                    for (var i = 0, l = this.length; i < l; i++) {
                        callback.call(
						table ?
							root(this[i], first) :
							this[i],
						results.cacheable || this.length > 1 || i > 0 ?
							results.fragment.cloneNode(true) :
							results.fragment
					);
                    }
                }

                if (scripts) {
                    jQuery.each(scripts, evalScript);
                }
            }

            return this;

            function root(elem, cur) {
                return jQuery.nodeName(elem, "table") ?
				(elem.getElementsByTagName("tbody")[0] ||
				elem.appendChild(elem.ownerDocument.createElement("tbody"))) :
				elem;
            }
        }
    });

    function cloneCopyEvent(orig, ret) {
        var i = 0;

        ret.each(function () {
            if (this.nodeName !== (orig[i] && orig[i].nodeName)) {
                return;
            }

            var oldData = jQuery.data(orig[i++]), curData = jQuery.data(this, oldData), events = oldData && oldData.events;

            if (events) {
                delete curData.handle;
                curData.events = {};

                for (var type in events) {
                    for (var handler in events[type]) {
                        jQuery.event.add(this, type, events[type][handler], events[type][handler].data);
                    }
                }
            }
        });
    }

    function buildFragment(args, nodes, scripts) {
        var fragment, cacheable, cacheresults, doc;

        // webkit does not clone 'checked' attribute of radio inputs on cloneNode, so don't cache if string has a checked
        if (args.length === 1 && typeof args[0] === "string" && args[0].length < 512 && args[0].indexOf("<option") < 0 && (jQuery.support.checkClone || !rchecked.test(args[0]))) {
            cacheable = true;
            cacheresults = jQuery.fragments[args[0]];
            if (cacheresults) {
                if (cacheresults !== 1) {
                    fragment = cacheresults;
                }
            }
        }

        if (!fragment) {
            doc = (nodes && nodes[0] ? nodes[0].ownerDocument || nodes[0] : document);
            fragment = doc.createDocumentFragment();
            jQuery.clean(args, doc, fragment, scripts);
        }

        if (cacheable) {
            jQuery.fragments[args[0]] = cacheresults ? fragment : 1;
        }

        return { fragment: fragment, cacheable: cacheable };
    }

    jQuery.fragments = {};

    jQuery.each({
        appendTo: "append",
        prependTo: "prepend",
        insertBefore: "before",
        insertAfter: "after",
        replaceAll: "replaceWith"
    }, function (name, original) {
        jQuery.fn[name] = function (selector) {
            var ret = [], insert = jQuery(selector);

            for (var i = 0, l = insert.length; i < l; i++) {
                var elems = (i > 0 ? this.clone(true) : this).get();
                jQuery.fn[original].apply(jQuery(insert[i]), elems);
                ret = ret.concat(elems);
            }
            return this.pushStack(ret, name, insert.selector);
        };
    });

    jQuery.each({
        // keepData is for internal use only--do not document
        remove: function (selector, keepData) {
            if (!selector || jQuery.filter(selector, [this]).length) {
                if (!keepData && this.nodeType === 1) {
                    jQuery.cleanData(this.getElementsByTagName("*"));
                    jQuery.cleanData([this]);
                }

                if (this.parentNode) {
                    this.parentNode.removeChild(this);
                }
            }
        },

        empty: function () {
            // Remove element nodes and prevent memory leaks
            if (this.nodeType === 1) {
                jQuery.cleanData(this.getElementsByTagName("*"));
            }

            // Remove any remaining nodes
            while (this.firstChild) {
                this.removeChild(this.firstChild);
            }
        }
    }, function (name, fn) {
        jQuery.fn[name] = function () {
            return this.each(fn, arguments);
        };
    });

    jQuery.extend({
        clean: function (elems, context, fragment, scripts) {
            context = context || document;

            // !context.createElement fails in IE with an error but returns typeof 'object'
            if (typeof context.createElement === "undefined") {
                context = context.ownerDocument || context[0] && context[0].ownerDocument || document;
            }

            var ret = [];

            jQuery.each(elems, function (i, elem) {
                if (typeof elem === "number") {
                    elem += "";
                }

                if (!elem) {
                    return;
                }

                // Convert html string into DOM nodes
                if (typeof elem === "string" && !rhtml.test(elem)) {
                    elem = context.createTextNode(elem);

                } else if (typeof elem === "string") {
                    // Fix "XHTML"-style tags in all browsers
                    elem = elem.replace(rxhtmlTag, fcloseTag);

                    // Trim whitespace, otherwise indexOf won't work as expected
                    var tag = (rtagName.exec(elem) || ["", ""])[1].toLowerCase(),
					wrap = wrapMap[tag] || wrapMap._default,
					depth = wrap[0],
					div = context.createElement("div");

                    // Go to html and back, then peel off extra wrappers
                    div.innerHTML = wrap[1] + elem + wrap[2];

                    // Move to the right depth
                    while (depth--) {
                        div = div.lastChild;
                    }

                    // Remove IE's autoinserted <tbody> from table fragments
                    if (!jQuery.support.tbody) {

                        // String was a <table>, *may* have spurious <tbody>
                        var hasBody = rtbody.test(elem),
						tbody = tag === "table" && !hasBody ?
							div.firstChild && div.firstChild.childNodes :

                        // String was a bare <thead> or <tfoot>
							wrap[1] === "<table>" && !hasBody ?
								div.childNodes :
								[];

                        for (var j = tbody.length - 1; j >= 0; --j) {
                            if (jQuery.nodeName(tbody[j], "tbody") && !tbody[j].childNodes.length) {
                                tbody[j].parentNode.removeChild(tbody[j]);
                            }
                        }

                    }

                    // IE completely kills leading whitespace when innerHTML is used
                    if (!jQuery.support.leadingWhitespace && rleadingWhitespace.test(elem)) {
                        div.insertBefore(context.createTextNode(rleadingWhitespace.exec(elem)[0]), div.firstChild);
                    }

                    elem = jQuery.makeArray(div.childNodes);
                }

                if (elem.nodeType) {
                    ret.push(elem);
                } else {
                    ret = jQuery.merge(ret, elem);
                }

            });

            if (fragment) {
                for (var i = 0; ret[i]; i++) {
                    if (scripts && jQuery.nodeName(ret[i], "script") && (!ret[i].type || ret[i].type.toLowerCase() === "text/javascript")) {
                        scripts.push(ret[i].parentNode ? ret[i].parentNode.removeChild(ret[i]) : ret[i]);
                    } else {
                        if (ret[i].nodeType === 1) {
                            ret.splice.apply(ret, [i + 1, 0].concat(jQuery.makeArray(ret[i].getElementsByTagName("script"))));
                        }
                        fragment.appendChild(ret[i]);
                    }
                }
            }

            return ret;
        },

        cleanData: function (elems) {
            for (var i = 0, elem, id; (elem = elems[i]) != null; i++) {
                jQuery.event.remove(elem);
                jQuery.removeData(elem);
            }
        }
    });
    // exclude the following css properties to add px
    var rexclude = /z-?index|font-?weight|opacity|zoom|line-?height/i,
	ralpha = /alpha\([^)]*\)/,
	ropacity = /opacity=([^)]*)/,
	rfloat = /float/i,
	rdashAlpha = /-([a-z])/ig,
	rupper = /([A-Z])/g,
	rnumpx = /^-?\d+(?:px)?$/i,
	rnum = /^-?\d/,

	cssShow = { position: "absolute", visibility: "hidden", display: "block" },
	cssWidth = ["Left", "Right"],
	cssHeight = ["Top", "Bottom"],

    // cache check for defaultView.getComputedStyle
	getComputedStyle = document.defaultView && document.defaultView.getComputedStyle,
    // normalize float css property
	styleFloat = jQuery.support.cssFloat ? "cssFloat" : "styleFloat",
	fcamelCase = function (all, letter) {
	    return letter.toUpperCase();
	};

    jQuery.fn.css = function (name, value) {
        return access(this, name, value, true, function (elem, name, value) {
            if (value === undefined) {
                return jQuery.curCSS(elem, name);
            }

            if (typeof value === "number" && !rexclude.test(name)) {
                value += "px";
            }

            jQuery.style(elem, name, value);
        });
    };

    jQuery.extend({
        style: function (elem, name, value) {
            // don't set styles on text and comment nodes
            if (!elem || elem.nodeType === 3 || elem.nodeType === 8) {
                return undefined;
            }

            // ignore negative width and height values #1599
            if ((name === "width" || name === "height") && parseFloat(value) < 0) {
                value = undefined;
            }

            var style = elem.style || elem, set = value !== undefined;

            // IE uses filters for opacity
            if (!jQuery.support.opacity && name === "opacity") {
                if (set) {
                    // IE has trouble with opacity if it does not have layout
                    // Force it by setting the zoom level
                    style.zoom = 1;

                    // Set the alpha filter to set the opacity
                    var opacity = parseInt(value, 10) + "" === "NaN" ? "" : "alpha(opacity=" + value * 100 + ")";
                    var filter = style.filter || jQuery.curCSS(elem, "filter") || "";
                    style.filter = ralpha.test(filter) ? filter.replace(ralpha, opacity) : opacity;
                }

                return style.filter && style.filter.indexOf("opacity=") >= 0 ?
				(parseFloat(ropacity.exec(style.filter)[1]) / 100) + "" :
				"";
            }

            // Make sure we're using the right name for getting the float value
            if (rfloat.test(name)) {
                name = styleFloat;
            }

            name = name.replace(rdashAlpha, fcamelCase);

            if (set) {
                style[name] = value;
            }

            return style[name];
        },

        css: function (elem, name, force, extra) {
            if (name === "width" || name === "height") {
                var val, props = cssShow, which = name === "width" ? cssWidth : cssHeight;

                function getWH() {
                    val = name === "width" ? elem.offsetWidth : elem.offsetHeight;

                    if (extra === "border") {
                        return;
                    }

                    jQuery.each(which, function () {
                        if (!extra) {
                            val -= parseFloat(jQuery.curCSS(elem, "padding" + this, true)) || 0;
                        }

                        if (extra === "margin") {
                            val += parseFloat(jQuery.curCSS(elem, "margin" + this, true)) || 0;
                        } else {
                            val -= parseFloat(jQuery.curCSS(elem, "border" + this + "Width", true)) || 0;
                        }
                    });
                }

                if (elem.offsetWidth !== 0) {
                    getWH();
                } else {
                    jQuery.swap(elem, props, getWH);
                }

                return Math.max(0, Math.round(val));
            }

            return jQuery.curCSS(elem, name, force);
        },

        curCSS: function (elem, name, force) {
            var ret, style = elem.style, filter;

            // IE uses filters for opacity
            if (!jQuery.support.opacity && name === "opacity" && elem.currentStyle) {
                ret = ropacity.test(elem.currentStyle.filter || "") ?
				(parseFloat(RegExp.$1) / 100) + "" :
				"";

                return ret === "" ?
				"1" :
				ret;
            }

            // Make sure we're using the right name for getting the float value
            if (rfloat.test(name)) {
                name = styleFloat;
            }

            if (!force && style && style[name]) {
                ret = style[name];

            } else if (getComputedStyle) {

                // Only "float" is needed here
                if (rfloat.test(name)) {
                    name = "float";
                }

                name = name.replace(rupper, "-$1").toLowerCase();

                var defaultView = elem.ownerDocument.defaultView;

                if (!defaultView) {
                    return null;
                }

                var computedStyle = defaultView.getComputedStyle(elem, null);

                if (computedStyle) {
                    ret = computedStyle.getPropertyValue(name);
                }

                // We should always get a number back from opacity
                if (name === "opacity" && ret === "") {
                    ret = "1";
                }

            } else if (elem.currentStyle) {
                var camelCase = name.replace(rdashAlpha, fcamelCase);

                ret = elem.currentStyle[name] || elem.currentStyle[camelCase];

                // From the awesome hack by Dean Edwards
                // http://erik.eae.net/archives/2007/07/27/18.54.15/#comment-102291

                // If we're not dealing with a regular pixel number
                // but a number that has a weird ending, we need to convert it to pixels
                if (!rnumpx.test(ret) && rnum.test(ret)) {
                    // Remember the original values
                    var left = style.left, rsLeft = elem.runtimeStyle.left;

                    // Put in the new values to get a computed value out
                    elem.runtimeStyle.left = elem.currentStyle.left;
                    style.left = camelCase === "fontSize" ? "1em" : (ret || 0);
                    ret = style.pixelLeft + "px";

                    // Revert the changed values
                    style.left = left;
                    elem.runtimeStyle.left = rsLeft;
                }
            }

            return ret;
        },

        // A method for quickly swapping in/out CSS properties to get correct calculations
        swap: function (elem, options, callback) {
            var old = {};

            // Remember the old values, and insert the new ones
            for (var name in options) {
                old[name] = elem.style[name];
                elem.style[name] = options[name];
            }

            callback.call(elem);

            // Revert the old values
            for (var name in options) {
                elem.style[name] = old[name];
            }
        }
    });

    if (jQuery.expr && jQuery.expr.filters) {
        jQuery.expr.filters.hidden = function (elem) {
            var width = elem.offsetWidth, height = elem.offsetHeight,
			skip = elem.nodeName.toLowerCase() === "tr";

            return width === 0 && height === 0 && !skip ?
			true :
			width > 0 && height > 0 && !skip ?
				false :
				jQuery.curCSS(elem, "display") === "none";
        };

        jQuery.expr.filters.visible = function (elem) {
            return !jQuery.expr.filters.hidden(elem);
        };
    }
    var jsc = now(),
	rscript = /<script(.|\s)*?\/script>/gi,
	rselectTextarea = /select|textarea/i,
	rinput = /color|date|datetime|email|hidden|month|number|password|range|search|tel|text|time|url|week/i,
	jsre = /=\?(&|$)/,
	rquery = /\?/,
	rts = /(\?|&)_=.*?(&|$)/,
	rurl = /^(\w+:)?\/\/([^\/?#]+)/,
	r20 = /%20/g;

    jQuery.fn.extend({
        // Keep a copy of the old load
        _load: jQuery.fn.load,

        load: function (url, params, callback) {
            if (typeof url !== "string") {
                return this._load(url);

                // Don't do a request if no elements are being requested
            } else if (!this.length) {
                return this;
            }

            var off = url.indexOf(" ");
            if (off >= 0) {
                var selector = url.slice(off, url.length);
                url = url.slice(0, off);
            }

            // Default to a GET request
            var type = "GET";

            // If the second parameter was provided
            if (params) {
                // If it's a function
                if (jQuery.isFunction(params)) {
                    // We assume that it's the callback
                    callback = params;
                    params = null;

                    // Otherwise, build a param string
                } else if (typeof params === "object") {
                    params = jQuery.param(params, jQuery.ajaxSettings.traditional);
                    type = "POST";
                }
            }

            var self = this;

            // Request the remote document
            jQuery.ajax({
                url: url,
                type: type,
                dataType: "html",
                data: params,
                complete: function (res, status) {
                    // If successful, inject the HTML into all the matched elements
                    if (status === "success" || status === "notmodified") {
                        // See if a selector was specified
                        self.html(selector ?
                        // Create a dummy div to hold the results
						jQuery("<div />")
                        // inject the contents of the document in, removing the scripts
                        // to avoid any 'Permission Denied' errors in IE
							.append(res.responseText.replace(rscript, ""))

                        // Locate the specified elements
							.find(selector) :

                        // If not, just inject the full result
						res.responseText);
                    }

                    if (callback) {
                        self.each(callback, [res.responseText, status, res]);
                    }
                }
            });

            return this;
        },

        serialize: function () {
            return jQuery.param(this.serializeArray());
        },
        serializeArray: function () {
            return this.map(function () {
                return this.elements ? jQuery.makeArray(this.elements) : this;
            })
		.filter(function () {
		    return this.name && !this.disabled &&
				(this.checked || rselectTextarea.test(this.nodeName) ||
					rinput.test(this.type));
		})
		.map(function (i, elem) {
		    var val = jQuery(this).val();

		    return val == null ?
				null :
				jQuery.isArray(val) ?
					jQuery.map(val, function (val, i) {
					    return { name: elem.name, value: val };
					}) :
					{ name: elem.name, value: val };
		}).get();
        }
    });

    // Attach a bunch of functions for handling common AJAX events
    jQuery.each("ajaxStart ajaxStop ajaxComplete ajaxError ajaxSuccess ajaxSend".split(" "), function (i, o) {
        jQuery.fn[o] = function (f) {
            return this.bind(o, f);
        };
    });

    jQuery.extend({

        get: function (url, data, callback, type) {
            // shift arguments if data argument was omited
            if (jQuery.isFunction(data)) {
                type = type || callback;
                callback = data;
                data = null;
            }

            return jQuery.ajax({
                type: "GET",
                url: url,
                data: data,
                success: callback,
                dataType: type
            });
        },

        getScript: function (url, callback) {
            return jQuery.get(url, null, callback, "script");
        },

        getJSON: function (url, data, callback) {
            return jQuery.get(url, data, callback, "json");
        },

        post: function (url, data, callback, type) {
            // shift arguments if data argument was omited
            if (jQuery.isFunction(data)) {
                type = type || callback;
                callback = data;
                data = {};
            }

            return jQuery.ajax({
                type: "POST",
                url: url,
                data: data,
                success: callback,
                dataType: type
            });
        },

        ajaxSetup: function (settings) {
            jQuery.extend(jQuery.ajaxSettings, settings);
        },

        ajaxSettings: {
            url: location.href,
            global: true,
            type: "GET",
            contentType: "application/x-www-form-urlencoded",
            processData: true,
            async: true,
            /*
            timeout: 0,
            data: null,
            username: null,
            password: null,
            traditional: false,
            */
            // Create the request object; Microsoft failed to properly
            // implement the XMLHttpRequest in IE7 (can't request local files),
            // so we use the ActiveXObject when it is available
            // This function can be overriden by calling jQuery.ajaxSetup
            xhr: window.XMLHttpRequest && (window.location.protocol !== "file:" || !window.ActiveXObject) ?
			function () {
			    return new window.XMLHttpRequest();
			} :
			function () {
			    try {
			        return new window.ActiveXObject("Microsoft.XMLHTTP");
			    } catch (e) { }
			},
            accepts: {
                xml: "application/xml, text/xml",
                html: "text/html",
                script: "text/javascript, application/javascript",
                json: "application/json, text/javascript",
                text: "text/plain",
                _default: "*/*"
            }
        },

        // Last-Modified header cache for next request
        lastModified: {},
        etag: {},

        ajax: function (origSettings) {
            var s = jQuery.extend(true, {}, jQuery.ajaxSettings, origSettings);

            var jsonp, status, data,
			callbackContext = origSettings && origSettings.context || s,
			type = s.type.toUpperCase();

            // convert data if not already a string
            if (s.data && s.processData && typeof s.data !== "string") {
                s.data = jQuery.param(s.data, s.traditional);
            }

            // Handle JSONP Parameter Callbacks
            if (s.dataType === "jsonp") {
                if (type === "GET") {
                    if (!jsre.test(s.url)) {
                        s.url += (rquery.test(s.url) ? "&" : "?") + (s.jsonp || "callback") + "=?";
                    }
                } else if (!s.data || !jsre.test(s.data)) {
                    s.data = (s.data ? s.data + "&" : "") + (s.jsonp || "callback") + "=?";
                }
                s.dataType = "json";
            }

            // Build temporary JSONP function
            if (s.dataType === "json" && (s.data && jsre.test(s.data) || jsre.test(s.url))) {
                jsonp = s.jsonpCallback || ("jsonp" + jsc++);

                // Replace the =? sequence both in the query string and the data
                if (s.data) {
                    s.data = (s.data + "").replace(jsre, "=" + jsonp + "$1");
                }

                s.url = s.url.replace(jsre, "=" + jsonp + "$1");

                // We need to make sure
                // that a JSONP style response is executed properly
                s.dataType = "script";

                // Handle JSONP-style loading
                window[jsonp] = window[jsonp] || function (tmp) {
                    data = tmp;
                    success();
                    complete();
                    // Garbage collect
                    window[jsonp] = undefined;

                    try {
                        delete window[jsonp];
                    } catch (e) { }

                    if (head) {
                        head.removeChild(script);
                    }
                };
            }

            if (s.dataType === "script" && s.cache === null) {
                s.cache = false;
            }

            if (s.cache === false && type === "GET") {
                var ts = now();

                // try replacing _= if it is there
                var ret = s.url.replace(rts, "$1_=" + ts + "$2");

                // if nothing was replaced, add timestamp to the end
                s.url = ret + ((ret === s.url) ? (rquery.test(s.url) ? "&" : "?") + "_=" + ts : "");
            }

            // If data is available, append data to url for get requests
            if (s.data && type === "GET") {
                s.url += (rquery.test(s.url) ? "&" : "?") + s.data;
            }

            // Watch for a new set of requests
            if (s.global && !jQuery.active++) {
                jQuery.event.trigger("ajaxStart");
            }

            // Matches an absolute URL, and saves the domain
            var parts = rurl.exec(s.url),
			remote = parts && (parts[1] && parts[1] !== location.protocol || parts[2] !== location.host);

            // If we're requesting a remote document
            // and trying to load JSON or Script with a GET
            if (s.dataType === "script" && type === "GET" && remote) {
                var head = document.getElementsByTagName("head")[0] || document.documentElement;
                var script = document.createElement("script");
                script.src = s.url;
                if (s.scriptCharset) {
                    script.charset = s.scriptCharset;
                }

                // Handle Script loading
                if (!jsonp) {
                    var done = false;

                    // Attach handlers for all browsers
                    script.onload = script.onreadystatechange = function () {
                        if (!done && (!this.readyState ||
							this.readyState === "loaded" || this.readyState === "complete")) {
                            done = true;
                            success();
                            complete();

                            // Handle memory leak in IE
                            script.onload = script.onreadystatechange = null;
                            if (head && script.parentNode) {
                                head.removeChild(script);
                            }
                        }
                    };
                }

                // Use insertBefore instead of appendChild  to circumvent an IE6 bug.
                // This arises when a base node is used (#2709 and #4378).
                head.insertBefore(script, head.firstChild);

                // We handle everything using the script element injection
                return undefined;
            }

            var requestDone = false;

            // Create the request object
            var xhr = s.xhr();

            if (!xhr) {
                return;
            }

            // Open the socket
            // Passing null username, generates a login popup on Opera (#2865)
            if (s.username) {
                xhr.open(type, s.url, s.async, s.username, s.password);
            } else {
                xhr.open(type, s.url, s.async);
            }

            // Need an extra try/catch for cross domain requests in Firefox 3
            try {
                // Set the correct header, if data is being sent
                if (s.data || origSettings && origSettings.contentType) {
                    xhr.setRequestHeader("Content-Type", s.contentType);
                }

                // Set the If-Modified-Since and/or If-None-Match header, if in ifModified mode.
                if (s.ifModified) {
                    if (jQuery.lastModified[s.url]) {
                        xhr.setRequestHeader("If-Modified-Since", jQuery.lastModified[s.url]);
                    }

                    if (jQuery.etag[s.url]) {
                        xhr.setRequestHeader("If-None-Match", jQuery.etag[s.url]);
                    }
                }

                // Set header so the called script knows that it's an XMLHttpRequest
                // Only send the header if it's not a remote XHR
                if (!remote) {
                    xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
                }

                // Set the Accepts header for the server, depending on the dataType
                xhr.setRequestHeader("Accept", s.dataType && s.accepts[s.dataType] ?
				s.accepts[s.dataType] + ", */*" :
				s.accepts._default);
            } catch (e) { }

            // Allow custom headers/mimetypes and early abort
            if (s.beforeSend && s.beforeSend.call(callbackContext, xhr, s) === false) {
                // Handle the global AJAX counter
                if (s.global && ! --jQuery.active) {
                    jQuery.event.trigger("ajaxStop");
                }

                // close opended socket
                xhr.abort();
                return false;
            }

            if (s.global) {
                trigger("ajaxSend", [xhr, s]);
            }

            // Wait for a response to come back
            var onreadystatechange = xhr.onreadystatechange = function (isTimeout) {
                // The request was aborted
                if (!xhr || xhr.readyState === 0 || isTimeout === "abort") {
                    // Opera doesn't call onreadystatechange before this point
                    // so we simulate the call
                    if (!requestDone) {
                        complete();
                    }

                    requestDone = true;
                    if (xhr) {
                        xhr.onreadystatechange = jQuery.noop;
                    }

                    // The transfer is complete and the data is available, or the request timed out
                } else if (!requestDone && xhr && (xhr.readyState === 4 || isTimeout === "timeout")) {
                    requestDone = true;
                    xhr.onreadystatechange = jQuery.noop;

                    status = isTimeout === "timeout" ?
					"timeout" :
					!jQuery.httpSuccess(xhr) ?
						"error" :
						s.ifModified && jQuery.httpNotModified(xhr, s.url) ?
							"notmodified" :
							"success";

                    var errMsg;

                    if (status === "success") {
                        // Watch for, and catch, XML document parse errors
                        try {
                            // process the data (runs the xml through httpData regardless of callback)
                            data = jQuery.httpData(xhr, s.dataType, s);
                        } catch (err) {
                            status = "parsererror";
                            errMsg = err;
                        }
                    }

                    // Make sure that the request was successful or notmodified
                    if (status === "success" || status === "notmodified") {
                        // JSONP handles its own success callback
                        if (!jsonp) {
                            success();
                        }
                    } else {
                        jQuery.handleError(s, xhr, status, errMsg);
                    }

                    // Fire the complete handlers
                    complete();

                    if (isTimeout === "timeout") {
                        xhr.abort();
                    }

                    // Stop memory leaks
                    if (s.async) {
                        xhr = null;
                    }
                }
            };

            // Override the abort handler, if we can (IE doesn't allow it, but that's OK)
            // Opera doesn't fire onreadystatechange at all on abort
            try {
                var oldAbort = xhr.abort;
                xhr.abort = function () {
                    if (xhr) {
                        oldAbort.call(xhr);
                    }

                    onreadystatechange("abort");
                };
            } catch (e) { }

            // Timeout checker
            if (s.async && s.timeout > 0) {
                setTimeout(function () {
                    // Check to see if the request is still happening
                    if (xhr && !requestDone) {
                        onreadystatechange("timeout");
                    }
                }, s.timeout);
            }

            // Send the data
            try {
                xhr.send(type === "POST" || type === "PUT" || type === "DELETE" ? s.data : null);
            } catch (e) {
                jQuery.handleError(s, xhr, null, e);
                // Fire the complete handlers
                complete();
            }

            // firefox 1.5 doesn't fire statechange for sync requests
            if (!s.async) {
                onreadystatechange();
            }

            function success() {
                // If a local callback was specified, fire it and pass it the data
                if (s.success) {
                    s.success.call(callbackContext, data, status, xhr);
                }

                // Fire the global callback
                if (s.global) {
                    trigger("ajaxSuccess", [xhr, s]);
                }
            }

            function complete() {
                // Process result
                if (s.complete) {
                    s.complete.call(callbackContext, xhr, status);
                }

                // The request was completed
                if (s.global) {
                    trigger("ajaxComplete", [xhr, s]);
                }

                // Handle the global AJAX counter
                if (s.global && ! --jQuery.active) {
                    jQuery.event.trigger("ajaxStop");
                }
            }

            function trigger(type, args) {
                (s.context ? jQuery(s.context) : jQuery.event).trigger(type, args);
            }

            // return XMLHttpRequest to allow aborting the request etc.
            return xhr;
        },

        handleError: function (s, xhr, status, e) {
            // If a local callback was specified, fire it
            if (s.error) {
                s.error.call(s.context || s, xhr, status, e);
            }

            // Fire the global callback
            if (s.global) {
                (s.context ? jQuery(s.context) : jQuery.event).trigger("ajaxError", [xhr, s, e]);
            }
        },

        // Counter for holding the number of active queries
        active: 0,

        // Determines if an XMLHttpRequest was successful or not
        httpSuccess: function (xhr) {
            try {
                // IE error sometimes returns 1223 when it should be 204 so treat it as success, see #1450
                return !xhr.status && location.protocol === "file:" ||
                // Opera returns 0 when status is 304
				(xhr.status >= 200 && xhr.status < 300) ||
				xhr.status === 304 || xhr.status === 1223 || xhr.status === 0;
            } catch (e) { }

            return false;
        },

        // Determines if an XMLHttpRequest returns NotModified
        httpNotModified: function (xhr, url) {
            var lastModified = xhr.getResponseHeader("Last-Modified"),
			etag = xhr.getResponseHeader("Etag");

            if (lastModified) {
                jQuery.lastModified[url] = lastModified;
            }

            if (etag) {
                jQuery.etag[url] = etag;
            }

            // Opera returns 0 when status is 304
            return xhr.status === 304 || xhr.status === 0;
        },

        httpData: function (xhr, type, s) {
            var ct = xhr.getResponseHeader("content-type") || "",
			xml = type === "xml" || !type && ct.indexOf("xml") >= 0,
			data = xml ? xhr.responseXML : xhr.responseText;

            if (xml && data.documentElement.nodeName === "parsererror") {
                jQuery.error("parsererror");
            }

            // Allow a pre-filtering function to sanitize the response
            // s is checked to keep backwards compatibility
            if (s && s.dataFilter) {
                data = s.dataFilter(data, type);
            }

            // The filter can actually parse the response
            if (typeof data === "string") {
                // Get the JavaScript object, if JSON is used.
                if (type === "json" || !type && ct.indexOf("json") >= 0) {
                    data = jQuery.parseJSON(data);

                    // If the type is "script", eval it in global context
                } else if (type === "script" || !type && ct.indexOf("javascript") >= 0) {
                    jQuery.globalEval(data);
                }
            }

            return data;
        },

        // Serialize an array of form elements or a set of
        // key/values into a query string
        param: function (a, traditional) {
            var s = [];

            // Set traditional to true for jQuery <= 1.3.2 behavior.
            if (traditional === undefined) {
                traditional = jQuery.ajaxSettings.traditional;
            }

            // If an array was passed in, assume that it is an array of form elements.
            if (jQuery.isArray(a) || a.jquery) {
                // Serialize the form elements
                jQuery.each(a, function () {
                    add(this.name, this.value);
                });

            } else {
                // If traditional, encode the "old" way (the way 1.3.2 or older
                // did it), otherwise encode params recursively.
                for (var prefix in a) {
                    buildParams(prefix, a[prefix]);
                }
            }

            // Return the resulting serialization
            return s.join("&").replace(r20, "+");

            function buildParams(prefix, obj) {
                if (jQuery.isArray(obj)) {
                    // Serialize array item.
                    jQuery.each(obj, function (i, v) {
                        if (traditional) {
                            // Treat each array item as a scalar.
                            add(prefix, v);
                        } else {
                            // If array item is non-scalar (array or object), encode its
                            // numeric index to resolve deserialization ambiguity issues.
                            // Note that rack (as of 1.0.0) can't currently deserialize
                            // nested arrays properly, and attempting to do so may cause
                            // a server error. Possible fixes are to modify rack's
                            // deserialization algorithm or to provide an option or flag
                            // to force array serialization to be shallow.
                            buildParams(prefix + "[" + (typeof v === "object" || jQuery.isArray(v) ? i : "") + "]", v);
                        }
                    });

                } else if (!traditional && obj != null && typeof obj === "object") {
                    // Serialize object item.
                    jQuery.each(obj, function (k, v) {
                        buildParams(prefix + "[" + k + "]", v);
                    });

                } else {
                    // Serialize scalar item.
                    add(prefix, obj);
                }
            }

            function add(key, value) {
                // If value is a function, invoke it and return its value
                value = jQuery.isFunction(value) ? value() : value;
                s[s.length] = encodeURIComponent(key) + "=" + encodeURIComponent(value);
            }
        }
    });
    var elemdisplay = {},
	rfxtypes = /toggle|show|hide/,
	rfxnum = /^([+-]=)?([\d+-.]+)(.*)$/,
	timerId,
	fxAttrs = [
    // height animations
		["height", "marginTop", "marginBottom", "paddingTop", "paddingBottom"],
    // width animations
		["width", "marginLeft", "marginRight", "paddingLeft", "paddingRight"],
    // opacity animations
		["opacity"]
	];

    jQuery.fn.extend({
        show: function (speed, callback) {
            if (speed || speed === 0) {
                return this.animate(genFx("show", 3), speed, callback);

            } else {
                for (var i = 0, l = this.length; i < l; i++) {
                    var old = jQuery.data(this[i], "olddisplay");

                    this[i].style.display = old || "";

                    if (jQuery.css(this[i], "display") === "none") {
                        var nodeName = this[i].nodeName, display;

                        if (elemdisplay[nodeName]) {
                            display = elemdisplay[nodeName];

                        } else {
                            var elem = jQuery("<" + nodeName + " />").appendTo("body");

                            display = elem.css("display");

                            if (display === "none") {
                                display = "block";
                            }

                            elem.remove();

                            elemdisplay[nodeName] = display;
                        }

                        jQuery.data(this[i], "olddisplay", display);
                    }
                }

                // Set the display of the elements in a second loop
                // to avoid the constant reflow
                for (var j = 0, k = this.length; j < k; j++) {
                    this[j].style.display = jQuery.data(this[j], "olddisplay") || "";
                }

                return this;
            }
        },

        hide: function (speed, callback) {
            if (speed || speed === 0) {
                return this.animate(genFx("hide", 3), speed, callback);

            } else {
                for (var i = 0, l = this.length; i < l; i++) {
                    var old = jQuery.data(this[i], "olddisplay");
                    if (!old && old !== "none") {
                        jQuery.data(this[i], "olddisplay", jQuery.css(this[i], "display"));
                    }
                }

                // Set the display of the elements in a second loop
                // to avoid the constant reflow
                for (var j = 0, k = this.length; j < k; j++) {
                    this[j].style.display = "none";
                }

                return this;
            }
        },

        // Save the old toggle function
        _toggle: jQuery.fn.toggle,

        toggle: function (fn, fn2) {
            var bool = typeof fn === "boolean";

            if (jQuery.isFunction(fn) && jQuery.isFunction(fn2)) {
                this._toggle.apply(this, arguments);

            } else if (fn == null || bool) {
                this.each(function () {
                    var state = bool ? fn : jQuery(this).is(":hidden");
                    jQuery(this)[state ? "show" : "hide"]();
                });

            } else {
                this.animate(genFx("toggle", 3), fn, fn2);
            }

            return this;
        },

        fadeTo: function (speed, to, callback) {
            return this.filter(":hidden").css("opacity", 0).show().end()
					.animate({ opacity: to }, speed, callback);
        },

        animate: function (prop, speed, easing, callback) {
            var optall = jQuery.speed(speed, easing, callback);

            if (jQuery.isEmptyObject(prop)) {
                return this.each(optall.complete);
            }

            return this[optall.queue === false ? "each" : "queue"](function () {
                var opt = jQuery.extend({}, optall), p,
				hidden = this.nodeType === 1 && jQuery(this).is(":hidden"),
				self = this;

                for (p in prop) {
                    var name = p.replace(rdashAlpha, fcamelCase);

                    if (p !== name) {
                        prop[name] = prop[p];
                        delete prop[p];
                        p = name;
                    }

                    if (prop[p] === "hide" && hidden || prop[p] === "show" && !hidden) {
                        return opt.complete.call(this);
                    }

                    if ((p === "height" || p === "width") && this.style) {
                        // Store display property
                        opt.display = jQuery.css(this, "display");

                        // Make sure that nothing sneaks out
                        opt.overflow = this.style.overflow;
                    }

                    if (jQuery.isArray(prop[p])) {
                        // Create (if needed) and add to specialEasing
                        (opt.specialEasing = opt.specialEasing || {})[p] = prop[p][1];
                        prop[p] = prop[p][0];
                    }
                }

                if (opt.overflow != null) {
                    this.style.overflow = "hidden";
                }

                opt.curAnim = jQuery.extend({}, prop);

                jQuery.each(prop, function (name, val) {
                    var e = new jQuery.fx(self, opt, name);

                    if (rfxtypes.test(val)) {
                        e[val === "toggle" ? hidden ? "show" : "hide" : val](prop);

                    } else {
                        var parts = rfxnum.exec(val),
						start = e.cur(true) || 0;

                        if (parts) {
                            var end = parseFloat(parts[2]),
							unit = parts[3] || "px";

                            // We need to compute starting value
                            if (unit !== "px") {
                                self.style[name] = (end || 1) + unit;
                                start = ((end || 1) / e.cur(true)) * start;
                                self.style[name] = start + unit;
                            }

                            // If a +=/-= token was provided, we're doing a relative animation
                            if (parts[1]) {
                                end = ((parts[1] === "-=" ? -1 : 1) * end) + start;
                            }

                            e.custom(start, end, unit);

                        } else {
                            e.custom(start, val, "");
                        }
                    }
                });

                // For JS strict compliance
                return true;
            });
        },

        stop: function (clearQueue, gotoEnd) {
            var timers = jQuery.timers;

            if (clearQueue) {
                this.queue([]);
            }

            this.each(function () {
                // go in reverse order so anything added to the queue during the loop is ignored
                for (var i = timers.length - 1; i >= 0; i--) {
                    if (timers[i].elem === this) {
                        if (gotoEnd) {
                            // force the next step to be the last
                            timers[i](true);
                        }

                        timers.splice(i, 1);
                    }
                }
            });

            // start the next in the queue if the last step wasn't forced
            if (!gotoEnd) {
                this.dequeue();
            }

            return this;
        }

    });

    // Generate shortcuts for custom animations
    jQuery.each({
        slideDown: genFx("show", 1),
        slideUp: genFx("hide", 1),
        slideToggle: genFx("toggle", 1),
        fadeIn: { opacity: "show" },
        fadeOut: { opacity: "hide" }
    }, function (name, props) {
        jQuery.fn[name] = function (speed, callback) {
            return this.animate(props, speed, callback);
        };
    });

    jQuery.extend({
        speed: function (speed, easing, fn) {
            var opt = speed && typeof speed === "object" ? speed : {
                complete: fn || !fn && easing ||
				jQuery.isFunction(speed) && speed,
                duration: speed,
                easing: fn && easing || easing && !jQuery.isFunction(easing) && easing
            };

            opt.duration = jQuery.fx.off ? 0 : typeof opt.duration === "number" ? opt.duration :
			jQuery.fx.speeds[opt.duration] || jQuery.fx.speeds._default;

            // Queueing
            opt.old = opt.complete;
            opt.complete = function () {
                if (opt.queue !== false) {
                    jQuery(this).dequeue();
                }
                if (jQuery.isFunction(opt.old)) {
                    opt.old.call(this);
                }
            };

            return opt;
        },

        easing: {
            linear: function (p, n, firstNum, diff) {
                return firstNum + diff * p;
            },
            swing: function (p, n, firstNum, diff) {
                return ((-Math.cos(p * Math.PI) / 2) + 0.5) * diff + firstNum;
            }
        },

        timers: [],

        fx: function (elem, options, prop) {
            this.options = options;
            this.elem = elem;
            this.prop = prop;

            if (!options.orig) {
                options.orig = {};
            }
        }

    });

    jQuery.fx.prototype = {
        // Simple function for setting a style value
        update: function () {
            if (this.options.step) {
                this.options.step.call(this.elem, this.now, this);
            }

            (jQuery.fx.step[this.prop] || jQuery.fx.step._default)(this);

            // Set display property to block for height/width animations
            if ((this.prop === "height" || this.prop === "width") && this.elem.style) {
                this.elem.style.display = "block";
            }
        },

        // Get the current size
        cur: function (force) {
            if (this.elem[this.prop] != null && (!this.elem.style || this.elem.style[this.prop] == null)) {
                return this.elem[this.prop];
            }

            var r = parseFloat(jQuery.css(this.elem, this.prop, force));
            return r && r > -10000 ? r : parseFloat(jQuery.curCSS(this.elem, this.prop)) || 0;
        },

        // Start an animation from one number to another
        custom: function (from, to, unit) {
            this.startTime = now();
            this.start = from;
            this.end = to;
            this.unit = unit || this.unit || "px";
            this.now = this.start;
            this.pos = this.state = 0;

            var self = this;
            function t(gotoEnd) {
                return self.step(gotoEnd);
            }

            t.elem = this.elem;

            if (t() && jQuery.timers.push(t) && !timerId) {
                timerId = setInterval(jQuery.fx.tick, 13);
            }
        },

        // Simple 'show' function
        show: function () {
            // Remember where we started, so that we can go back to it later
            this.options.orig[this.prop] = jQuery.style(this.elem, this.prop);
            this.options.show = true;

            // Begin the animation
            // Make sure that we start at a small width/height to avoid any
            // flash of content
            this.custom(this.prop === "width" || this.prop === "height" ? 1 : 0, this.cur());

            // Start by showing the element
            jQuery(this.elem).show();
        },

        // Simple 'hide' function
        hide: function () {
            // Remember where we started, so that we can go back to it later
            this.options.orig[this.prop] = jQuery.style(this.elem, this.prop);
            this.options.hide = true;

            // Begin the animation
            this.custom(this.cur(), 0);
        },

        // Each step of an animation
        step: function (gotoEnd) {
            var t = now(), done = true;

            if (gotoEnd || t >= this.options.duration + this.startTime) {
                this.now = this.end;
                this.pos = this.state = 1;
                this.update();

                this.options.curAnim[this.prop] = true;

                for (var i in this.options.curAnim) {
                    if (this.options.curAnim[i] !== true) {
                        done = false;
                    }
                }

                if (done) {
                    if (this.options.display != null) {
                        // Reset the overflow
                        this.elem.style.overflow = this.options.overflow;

                        // Reset the display
                        var old = jQuery.data(this.elem, "olddisplay");
                        this.elem.style.display = old ? old : this.options.display;

                        if (jQuery.css(this.elem, "display") === "none") {
                            this.elem.style.display = "block";
                        }
                    }

                    // Hide the element if the "hide" operation was done
                    if (this.options.hide) {
                        jQuery(this.elem).hide();
                    }

                    // Reset the properties, if the item has been hidden or shown
                    if (this.options.hide || this.options.show) {
                        for (var p in this.options.curAnim) {
                            jQuery.style(this.elem, p, this.options.orig[p]);
                        }
                    }

                    // Execute the complete function
                    this.options.complete.call(this.elem);
                }

                return false;

            } else {
                var n = t - this.startTime;
                this.state = n / this.options.duration;

                // Perform the easing function, defaults to swing
                var specialEasing = this.options.specialEasing && this.options.specialEasing[this.prop];
                var defaultEasing = this.options.easing || (jQuery.easing.swing ? "swing" : "linear");
                this.pos = jQuery.easing[specialEasing || defaultEasing](this.state, n, 0, 1, this.options.duration);
                this.now = this.start + ((this.end - this.start) * this.pos);

                // Perform the next step of the animation
                this.update();
            }

            return true;
        }
    };

    jQuery.extend(jQuery.fx, {
        tick: function () {
            var timers = jQuery.timers;

            for (var i = 0; i < timers.length; i++) {
                if (!timers[i]()) {
                    timers.splice(i--, 1);
                }
            }

            if (!timers.length) {
                jQuery.fx.stop();
            }
        },

        stop: function () {
            clearInterval(timerId);
            timerId = null;
        },

        speeds: {
            slow: 600,
            fast: 200,
            // Default speed
            _default: 400
        },

        step: {
            opacity: function (fx) {
                jQuery.style(fx.elem, "opacity", fx.now);
            },

            _default: function (fx) {
                if (fx.elem.style && fx.elem.style[fx.prop] != null) {
                    fx.elem.style[fx.prop] = (fx.prop === "width" || fx.prop === "height" ? Math.max(0, fx.now) : fx.now) + fx.unit;
                } else {
                    fx.elem[fx.prop] = fx.now;
                }
            }
        }
    });

    if (jQuery.expr && jQuery.expr.filters) {
        jQuery.expr.filters.animated = function (elem) {
            return jQuery.grep(jQuery.timers, function (fn) {
                return elem === fn.elem;
            }).length;
        };
    }

    function genFx(type, num) {
        var obj = {};

        jQuery.each(fxAttrs.concat.apply([], fxAttrs.slice(0, num)), function () {
            obj[this] = type;
        });

        return obj;
    }
    if ("getBoundingClientRect" in document.documentElement) {
        jQuery.fn.offset = function (options) {
            var elem = this[0];

            if (options) {
                return this.each(function (i) {
                    jQuery.offset.setOffset(this, options, i);
                });
            }

            if (!elem || !elem.ownerDocument) {
                return null;
            }

            if (elem === elem.ownerDocument.body) {
                return jQuery.offset.bodyOffset(elem);
            }

            var box = elem.getBoundingClientRect(), doc = elem.ownerDocument, body = doc.body, docElem = doc.documentElement,
			clientTop = docElem.clientTop || body.clientTop || 0, clientLeft = docElem.clientLeft || body.clientLeft || 0,
			top = box.top + (self.pageYOffset || jQuery.support.boxModel && docElem.scrollTop || body.scrollTop) - clientTop,
			left = box.left + (self.pageXOffset || jQuery.support.boxModel && docElem.scrollLeft || body.scrollLeft) - clientLeft;

            return { top: top, left: left };
        };

    } else {
        jQuery.fn.offset = function (options) {
            var elem = this[0];

            if (options) {
                return this.each(function (i) {
                    jQuery.offset.setOffset(this, options, i);
                });
            }

            if (!elem || !elem.ownerDocument) {
                return null;
            }

            if (elem === elem.ownerDocument.body) {
                return jQuery.offset.bodyOffset(elem);
            }

            jQuery.offset.initialize();

            var offsetParent = elem.offsetParent, prevOffsetParent = elem,
			doc = elem.ownerDocument, computedStyle, docElem = doc.documentElement,
			body = doc.body, defaultView = doc.defaultView,
			prevComputedStyle = defaultView ? defaultView.getComputedStyle(elem, null) : elem.currentStyle,
			top = elem.offsetTop, left = elem.offsetLeft;

            while ((elem = elem.parentNode) && elem !== body && elem !== docElem) {
                if (jQuery.offset.supportsFixedPosition && prevComputedStyle.position === "fixed") {
                    break;
                }

                computedStyle = defaultView ? defaultView.getComputedStyle(elem, null) : elem.currentStyle;
                top -= elem.scrollTop;
                left -= elem.scrollLeft;

                if (elem === offsetParent) {
                    top += elem.offsetTop;
                    left += elem.offsetLeft;

                    if (jQuery.offset.doesNotAddBorder && !(jQuery.offset.doesAddBorderForTableAndCells && /^t(able|d|h)$/i.test(elem.nodeName))) {
                        top += parseFloat(computedStyle.borderTopWidth) || 0;
                        left += parseFloat(computedStyle.borderLeftWidth) || 0;
                    }

                    prevOffsetParent = offsetParent, offsetParent = elem.offsetParent;
                }

                if (jQuery.offset.subtractsBorderForOverflowNotVisible && computedStyle.overflow !== "visible") {
                    top += parseFloat(computedStyle.borderTopWidth) || 0;
                    left += parseFloat(computedStyle.borderLeftWidth) || 0;
                }

                prevComputedStyle = computedStyle;
            }

            if (prevComputedStyle.position === "relative" || prevComputedStyle.position === "static") {
                top += body.offsetTop;
                left += body.offsetLeft;
            }

            if (jQuery.offset.supportsFixedPosition && prevComputedStyle.position === "fixed") {
                top += Math.max(docElem.scrollTop, body.scrollTop);
                left += Math.max(docElem.scrollLeft, body.scrollLeft);
            }

            return { top: top, left: left };
        };
    }

    jQuery.offset = {
        initialize: function () {
            var body = document.body, container = document.createElement("div"), innerDiv, checkDiv, table, td, bodyMarginTop = parseFloat(jQuery.curCSS(body, "marginTop", true)) || 0,
			html = "<div style='position:absolute;top:0;left:0;margin:0;border:5px solid #000;padding:0;width:1px;height:1px;'><div></div></div><table style='position:absolute;top:0;left:0;margin:0;border:5px solid #000;padding:0;width:1px;height:1px;' cellpadding='0' cellspacing='0'><tr><td></td></tr></table>";

            jQuery.extend(container.style, { position: "absolute", top: 0, left: 0, margin: 0, border: 0, width: "1px", height: "1px", visibility: "hidden" });

            container.innerHTML = html;
            body.insertBefore(container, body.firstChild);
            innerDiv = container.firstChild;
            checkDiv = innerDiv.firstChild;
            td = innerDiv.nextSibling.firstChild.firstChild;

            this.doesNotAddBorder = (checkDiv.offsetTop !== 5);
            this.doesAddBorderForTableAndCells = (td.offsetTop === 5);

            checkDiv.style.position = "fixed", checkDiv.style.top = "20px";
            // safari subtracts parent border width here which is 5px
            this.supportsFixedPosition = (checkDiv.offsetTop === 20 || checkDiv.offsetTop === 15);
            checkDiv.style.position = checkDiv.style.top = "";

            innerDiv.style.overflow = "hidden", innerDiv.style.position = "relative";
            this.subtractsBorderForOverflowNotVisible = (checkDiv.offsetTop === -5);

            this.doesNotIncludeMarginInBodyOffset = (body.offsetTop !== bodyMarginTop);

            body.removeChild(container);
            body = container = innerDiv = checkDiv = table = td = null;
            jQuery.offset.initialize = jQuery.noop;
        },

        bodyOffset: function (body) {
            var top = body.offsetTop, left = body.offsetLeft;

            jQuery.offset.initialize();

            if (jQuery.offset.doesNotIncludeMarginInBodyOffset) {
                top += parseFloat(jQuery.curCSS(body, "marginTop", true)) || 0;
                left += parseFloat(jQuery.curCSS(body, "marginLeft", true)) || 0;
            }

            return { top: top, left: left };
        },

        setOffset: function (elem, options, i) {
            // set position first, in-case top/left are set even on static elem
            if (/static/.test(jQuery.curCSS(elem, "position"))) {
                elem.style.position = "relative";
            }
            var curElem = jQuery(elem),
			curOffset = curElem.offset(),
			curTop = parseInt(jQuery.curCSS(elem, "top", true), 10) || 0,
			curLeft = parseInt(jQuery.curCSS(elem, "left", true), 10) || 0;

            if (jQuery.isFunction(options)) {
                options = options.call(elem, i, curOffset);
            }

            var props = {
                top: (options.top - curOffset.top) + curTop,
                left: (options.left - curOffset.left) + curLeft
            };

            if ("using" in options) {
                options.using.call(elem, props);
            } else {
                curElem.css(props);
            }
        }
    };


    jQuery.fn.extend({
        position: function () {
            if (!this[0]) {
                return null;
            }

            var elem = this[0],

            // Get *real* offsetParent
		offsetParent = this.offsetParent(),

            // Get correct offsets
		offset = this.offset(),
		parentOffset = /^body|html$/i.test(offsetParent[0].nodeName) ? { top: 0, left: 0} : offsetParent.offset();

            // Subtract element margins
            // note: when an element has margin: auto the offsetLeft and marginLeft
            // are the same in Safari causing offset.left to incorrectly be 0
            offset.top -= parseFloat(jQuery.curCSS(elem, "marginTop", true)) || 0;
            offset.left -= parseFloat(jQuery.curCSS(elem, "marginLeft", true)) || 0;

            // Add offsetParent borders
            parentOffset.top += parseFloat(jQuery.curCSS(offsetParent[0], "borderTopWidth", true)) || 0;
            parentOffset.left += parseFloat(jQuery.curCSS(offsetParent[0], "borderLeftWidth", true)) || 0;

            // Subtract the two offsets
            return {
                top: offset.top - parentOffset.top,
                left: offset.left - parentOffset.left
            };
        },

        offsetParent: function () {
            return this.map(function () {
                var offsetParent = this.offsetParent || document.body;
                while (offsetParent && (!/^body|html$/i.test(offsetParent.nodeName) && jQuery.css(offsetParent, "position") === "static")) {
                    offsetParent = offsetParent.offsetParent;
                }
                return offsetParent;
            });
        }
    });


    // Create scrollLeft and scrollTop methods
    jQuery.each(["Left", "Top"], function (i, name) {
        var method = "scroll" + name;

        jQuery.fn[method] = function (val) {
            var elem = this[0], win;

            if (!elem) {
                return null;
            }

            if (val !== undefined) {
                // Set the scroll offset
                return this.each(function () {
                    win = getWindow(this);

                    if (win) {
                        win.scrollTo(
						!i ? val : jQuery(win).scrollLeft(),
						 i ? val : jQuery(win).scrollTop()
					);

                    } else {
                        this[method] = val;
                    }
                });
            } else {
                win = getWindow(elem);

                // Return the scroll offset
                return win ? ("pageXOffset" in win) ? win[i ? "pageYOffset" : "pageXOffset"] :
				jQuery.support.boxModel && win.document.documentElement[method] ||
					win.document.body[method] :
				elem[method];
            }
        };
    });

    function getWindow(elem) {
        return ("scrollTo" in elem && elem.document) ?
		elem :
		elem.nodeType === 9 ?
			elem.defaultView || elem.parentWindow :
			false;
    }
    // Create innerHeight, innerWidth, outerHeight and outerWidth methods
    jQuery.each(["Height", "Width"], function (i, name) {

        var type = name.toLowerCase();

        // innerHeight and innerWidth
        jQuery.fn["inner" + name] = function () {
            return this[0] ?
			jQuery.css(this[0], type, false, "padding") :
			null;
        };

        // outerHeight and outerWidth
        jQuery.fn["outer" + name] = function (margin) {
            return this[0] ?
			jQuery.css(this[0], type, false, margin ? "margin" : "border") :
			null;
        };

        jQuery.fn[type] = function (size) {
            // Get window width or height
            var elem = this[0];
            if (!elem) {
                return size == null ? null : this;
            }

            if (jQuery.isFunction(size)) {
                return this.each(function (i) {
                    var self = jQuery(this);
                    self[type](size.call(this, i, self[type]()));
                });
            }

            return ("scrollTo" in elem && elem.document) ? // does it walk and quack like a window?
            // Everyone else use document.documentElement or document.body depending on Quirks vs Standards mode
			elem.document.compatMode === "CSS1Compat" && elem.document.documentElement["client" + name] ||
			elem.document.body["client" + name] :

            // Get document width or height
			(elem.nodeType === 9) ? // is it a document
            // Either scroll[Width/Height] or offset[Width/Height], whichever is greater
				Math.max(
					elem.documentElement["client" + name],
					elem.body["scroll" + name], elem.documentElement["scroll" + name],
					elem.body["offset" + name], elem.documentElement["offset" + name]
				) :

            // Get or set width or height on the element
				size === undefined ?
            // Get width or height on the element
					jQuery.css(elem, type) :

            // Set the width or height on the element (default to pixels if value is unitless)
					this.css(type, typeof size === "string" ? size : size + "px");
        };

    });
    // Expose jQuery to the global object
    window.jQuery = window.$ = jQuery;

})(window);


/*
* jQuery JavaScript Library v1.5.1
* http://jquery.com/
*
* Copyright 2011, John Resig
* Dual licensed under the MIT or GPL Version 2 licenses.
* http://jquery.org/license
*
* Includes Sizzle.js
* http://sizzlejs.com/
* Copyright 2011, The Dojo Foundation
* Released under the MIT, BSD, and GPL Licenses.
*
* Date: Wed Feb 23 13:55:29 2011 -0500
*/
(function (Ar, As) { function Bu(A) { return Au.isWindow(A) ? A : A.nodeType === 9 ? A.defaultView || A.parentWindow : !1 } function AI(A) { if (!BZ[A]) { var B = Au("<" + A + ">").appendTo("body"), C = B.css("display"); B.remove(); if (C === "none" || C === "") { C = "block" } BZ[A] = C } return BZ[A] } function A1(A, B) { var C = {}; Au.each(A4.concat.apply([], A4.slice(0, B)), function () { C[this] = A }); return C } function BA() { try { return new Ar.ActiveXObject("Microsoft.XMLHTTP") } catch (A) { } } function Be() { try { return new Ar.XMLHttpRequest } catch (A) { } } function Bp() { Au(Ar).unload(function () { for (var A in AT) { AT[A](0, 1) } }) } function A6(H, I) { H.dataFilter && (I = H.dataFilter(I, H.dataType)); var J = H.dataTypes, K = {}, L, N, A = J.length, B, C = J[0], D, E, F, G, M; for (L = 1; L < A; L++) { if (L === 1) { for (N in H.converters) { typeof N === "string" && (K[N.toLowerCase()] = H.converters[N]) } } D = C, C = J[L]; if (C === "*") { C = D } else { if (D !== "*" && D !== C) { E = D + " " + C, F = K[E] || K["* " + C]; if (!F) { M = As; for (G in K) { B = G.split(" "); if (B[0] === D || B[0] === "*") { M = K[B[1] + " " + C]; if (M) { G = K[G], G === !0 ? F = M : M === !0 && (F = G); break } } } } !F && !M && Au.error("No conversion from " + E.replace(" ", " to ")), F !== !0 && (I = F ? F(I) : M(G(I))) } } } return I } function AQ(D, E, F) { var G = D.contents, H = D.dataTypes, I = D.responseFields, J, A, B, C; for (A in I) { A in F && (E[I[A]] = F[A]) } while (H[0] === "*") { H.shift(), J === As && (J = D.mimeType || E.getResponseHeader("content-type")) } if (J) { for (A in G) { if (G[A] && G[A].test(J)) { H.unshift(A); break } } } if (H[0] in F) { B = H[0] } else { for (A in F) { if (!H[0] || D.converters[A + " " + H[0]]) { B = A; break } C || (C = A) } B = B || C } if (B) { B !== H[0] && H.unshift(B); return F[B] } } function A0(A, B, C, D) { if (Au.isArray(B) && B.length) { Au.each(B, function (F, G) { C || AJ.test(A) ? D(A, G) : A0(A + "[" + (typeof G === "object" || Au.isArray(G) ? F : "") + "]", G, C, D) }) } else { if (C || B == null || typeof B !== "object") { D(A, B) } else { if (Au.isArray(B) || Au.isEmptyObject(B)) { D(A, "") } else { for (var E in B) { A0(A + "[" + E + "]", B[E], C, D) } } } } } function AL(E, F, G, H, I, J) { I = I || F.dataTypes[0], J = J || {}, J[I] = !0; var K = E[I], A = 0, B = K ? K.length : 0, C = E === AZ, D; for (; A < B && (C || !D); A++) { D = K[A](F, G, H), typeof D === "string" && (!C || J[D] ? D = As : (F.dataTypes.unshift(D), D = AL(E, F, G, H, D, J))) } (C || !D) && !J["*"] && (D = AL(E, F, G, H, "*", J)); return D } function Ao(A) { return function (D, E) { typeof D !== "string" && (E = D, D = "*"); if (Au.isFunction(E)) { var F = D.toLowerCase().split(Ay), G = 0, H = F.length, I, B, C; for (; G < H; G++) { I = F[G], C = /^\+/.test(I), C && (I = I.substr(1) || "*"), B = A[I] = A[I] || [], B[C ? "unshift" : "push"](E) } } } } function AE(A, B, C) { var D = B === "width" ? BK : AB, E = B === "width" ? A.offsetWidth : A.offsetHeight; if (C === "border") { return E } Au.each(D, function () { C || (E -= parseFloat(Au.css(A, "padding" + this)) || 0), C === "margin" ? E += parseFloat(Au.css(A, "margin" + this)) || 0 : E -= parseFloat(Au.css(A, "border" + this + "Width")) || 0 }); return E } function A8(A, B) { B.src ? Au.ajax({ url: B.src, async: !1, dataType: "script" }) : Au.globalEval(B.text || B.textContent || B.innerHTML || ""), B.parentNode && B.parentNode.removeChild(B) } function Bx(A) { return "getElementsByTagName" in A ? A.getElementsByTagName("*") : "querySelectorAll" in A ? A.querySelectorAll("*") : [] } function AR(A, B) { if (B.nodeType === 1) { var C = B.nodeName.toLowerCase(); B.clearAttributes(), B.mergeAttributes(A); if (C === "object") { B.outerHTML = A.outerHTML } else { if (C !== "input" || A.type !== "checkbox" && A.type !== "radio") { if (C === "option") { B.selected = A.defaultSelected } else { if (C === "input" || C === "textarea") { B.defaultValue = A.defaultValue } } } else { A.checked && (B.defaultChecked = B.checked = A.checked), B.value !== A.value && (B.value = A.value) } } B.removeAttribute(Au.expando) } } function Bs(C, D) { if (D.nodeType === 1 && Au.hasData(C)) { var E = Au.expando, F = Au.data(C), G = Au.data(D, F); if (F = F[E]) { var H = F.events; G = G[E] = Au.extend({}, F); if (H) { delete G.handle, G.events = {}; for (var I in H) { for (var A = 0, B = H[I].length; A < B; A++) { Au.event.add(D, I + (H[I][A].namespace ? "." : "") + H[I][A].namespace, H[I][A], H[I][A].data) } } } } } } function Br(A, B) { return Au.nodeName(A, "table") ? A.getElementsByTagName("tbody")[0] || A.appendChild(A.ownerDocument.createElement("tbody")) : A } function BN(A, B, C) { if (Au.isFunction(B)) { return Au.grep(A, function (E, F) { var G = !!B.call(E, F, E); return G === C }) } if (B.nodeType) { return Au.grep(A, function (E, F) { return E === B === C }) } if (typeof B === "string") { var D = Au.grep(A, function (E) { return E.nodeType === 1 }); if (BI.test(B)) { return Au.filter(B, D, !C) } B = Au.filter(B, D) } return Au.grep(A, function (E, F) { return Au.inArray(E, B) >= 0 === C }) } function BM(A) { return !A || !A.parentNode || A.parentNode.nodeType === 11 } function BU(A, B) { return (A && A !== "*" ? A + "." : "") + B.replace(Al, "`").replace(Ad, "&") } function BT(M) { var N, O, P, Q, R, E, F, G, H, I, J, K, L, A = [], B = [], C = Au._data(this, "events"); if (M.liveFired !== this && C && C.live && !M.target.disabled && (!M.button || M.type !== "click")) { M.namespace && (K = new RegExp("(^|\\.)" + M.namespace.split(".").join("\\.(?:.*\\.)?") + "(\\.|$)")), M.liveFired = this; var D = C.live.slice(0); for (F = 0; F < D.length; F++) { R = D[F], R.origType.replace(Aa, "") === M.type ? B.push(R.selector) : D.splice(F--, 1) } Q = Au(M.target).closest(B, M.currentTarget); for (G = 0, H = Q.length; G < H; G++) { J = Q[G]; for (F = 0; F < D.length; F++) { R = D[F]; if (J.selector === R.selector && (!K || K.test(R.namespace)) && !J.elem.disabled) { E = J.elem, P = null; if (R.preType === "mouseenter" || R.preType === "mouseleave") { M.type = R.preType, P = Au(M.relatedTarget).closest(R.selector)[0] } (!P || P !== E) && A.push({ elem: E, handleObj: R, level: J.level }) } } } for (G = 0, H = A.length; G < H; G++) { Q = A[G]; if (O && Q.level > O) { break } M.currentTarget = Q.elem, M.data = Q.handleObj.data, M.handleObj = Q.handleObj, L = Q.handleObj.origHandler.apply(Q.elem, arguments); if (L === !1 || M.isPropagationStopped()) { O = Q.level, L === !1 && (N = !1); if (M.isImmediatePropagationStopped()) { break } } } return N } } function BR(A, B, C) { var D = Au.extend({}, C[0]); D.type = A, D.originalEvent = {}, D.liveFired = As, Au.event.handle.call(B, D), D.isDefaultPrevented() && C[0].preventDefault() } function Ah() { return !0 } function Ag() { return !1 } function Ax(A) { for (var B in A) { if (B !== "toJSON") { return !1 } } return !0 } function Aw(A, B, C) { if (C === As && A.nodeType === 1) { C = A.getAttribute("data-" + B); if (typeof C === "string") { try { C = C === "true" ? !0 : C === "false" ? !1 : C === "null" ? null : Au.isNaN(C) ? Av.test(C) ? Au.parseJSON(C) : C : parseFloat(C) } catch (D) { } Au.data(A, B, C) } else { C = As } } return C } var At = Ar.document, Au = function () { function B3() { if (!b.isReady) { try { At.documentElement.doScroll("left") } catch (A) { setTimeout(B3, 1); return } b.ready() } } var b = function (A, B) { return new b.fn.init(A, B, B2) }, c = Ar.jQuery, B1 = Ar.$, B2, T = /^(?:[^<]*(<[\w\W]+>)[^>]*$|#([\w\-]+)$)/, U = /\S/, V = /^\s+/, W = /\s+$/, X = /\d/, Y = /^<(\w+)\s*\/?>(?:<\/\1>)?$/, Z = /^[\],:{}\s]*$/, a = /\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, L = /"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, M = /(?:^|:|,)(?:\s*\[)+/g, N = /(webkit)[ \/]([\w.]+)/, O = /(opera)(?:.*version)?[ \/]([\w.]+)/, P = /(msie) ([\w.]+)/, Q = /(mozilla)(?:.*? rv:([\w.]+))?/, R = navigator.userAgent, S, Cc = !1, J, K = "then done fail isResolved isRejected promise".split(" "), B5, B6 = Object.prototype.toString, B7 = Object.prototype.hasOwnProperty, B8 = Array.prototype.push, B9 = Array.prototype.slice, Ca = String.prototype.trim, Cb = Array.prototype.indexOf, B4 = {}; b.fn = b.prototype = { constructor: b, init: function (B, E, F) { var G, D, A, C; if (!B) { return this } if (B.nodeType) { this.context = this[0] = B, this.length = 1; return this } if (B === "body" && !E && At.body) { this.context = At, this[0] = At.body, this.selector = "body", this.length = 1; return this } if (typeof B === "string") { G = T.exec(B); if (!G || !G[1] && E) { return !E || E.jquery ? (E || F).find(B) : this.constructor(E).find(B) } if (G[1]) { E = E instanceof b ? E[0] : E, C = E ? E.ownerDocument || E : At, A = Y.exec(B), A ? b.isPlainObject(E) ? (B = [At.createElement(A[1])], b.fn.attr.call(B, E, !0)) : B = [C.createElement(A[1])] : (A = b.buildFragment([G[1]], [C]), B = (A.cacheable ? b.clone(A.fragment) : A.fragment).childNodes); return b.merge(this, B) } D = At.getElementById(G[2]); if (D && D.parentNode) { if (D.id !== G[2]) { return F.find(B) } this.length = 1, this[0] = D } this.context = At, this.selector = B; return this } if (b.isFunction(B)) { return F.ready(B) } B.selector !== As && (this.selector = B.selector, this.context = B.context); return b.makeArray(B, this) }, selector: "", jquery: "1.5.1", length: 0, size: function () { return this.length }, toArray: function () { return B9.call(this, 0) }, get: function (A) { return A == null ? this.toArray() : A < 0 ? this[this.length + A] : this[A] }, pushStack: function (A, B, C) { var D = this.constructor(); b.isArray(A) ? B8.apply(D, A) : b.merge(D, A), D.prevObject = this, D.context = this.context, B === "find" ? D.selector = this.selector + (this.selector ? " " : "") + C : B && (D.selector = this.selector + "." + B + "(" + C + ")"); return D }, each: function (A, B) { return b.each(this, A, B) }, ready: function (A) { b.bindReady(), J.done(A); return this }, eq: function (A) { return A === -1 ? this.slice(A) : this.slice(A, +A + 1) }, first: function () { return this.eq(0) }, last: function () { return this.eq(-1) }, slice: function () { return this.pushStack(B9.apply(this, arguments), "slice", B9.call(arguments).join(",")) }, map: function (A) { return this.pushStack(b.map(this, function (B, C) { return A.call(B, C, B) })) }, end: function () { return this.prevObject || this.constructor(null) }, push: B8, sort: [].sort, splice: [].splice }, b.fn.init.prototype = b.fn, b.extend = b.fn.extend = function () { var E, F, G, H, I, d, A = arguments[0] || {}, B = 1, C = arguments.length, D = !1; typeof A === "boolean" && (D = A, A = arguments[1] || {}, B = 2), typeof A !== "object" && !b.isFunction(A) && (A = {}), C === B && (A = this, --B); for (; B < C; B++) { if ((E = arguments[B]) != null) { for (F in E) { G = A[F], H = E[F]; if (A === H) { continue } D && H && (b.isPlainObject(H) || (I = b.isArray(H))) ? (I ? (I = !1, d = G && b.isArray(G) ? G : []) : d = G && b.isPlainObject(G) ? G : {}, A[F] = b.extend(D, d, H)) : H !== As && (A[F] = H) } } } return A }, b.extend({ noConflict: function (A) { Ar.$ = B1, A && (Ar.jQuery = c); return b }, isReady: !1, readyWait: 1, ready: function (A) { A === !0 && b.readyWait--; if (!b.readyWait || A !== !0 && !b.isReady) { if (!At.body) { return setTimeout(b.ready, 1) } b.isReady = !0; if (A !== !0 && --b.readyWait > 0) { return } J.resolveWith(At, [b]), b.fn.trigger && b(At).trigger("ready").unbind("ready") } }, bindReady: function () { if (!Cc) { Cc = !0; if (At.readyState === "complete") { return setTimeout(b.ready, 1) } if (At.addEventListener) { At.addEventListener("DOMContentLoaded", B5, !1), Ar.addEventListener("load", b.ready, !1) } else { if (At.attachEvent) { At.attachEvent("onreadystatechange", B5), Ar.attachEvent("onload", b.ready); var A = !1; try { A = Ar.frameElement == null } catch (B) { } At.documentElement.doScroll && A && B3() } } } }, isFunction: function (A) { return b.type(A) === "function" }, isArray: Array.isArray || function (A) { return b.type(A) === "array" }, isWindow: function (A) { return A && typeof A === "object" && "setInterval" in A }, isNaN: function (A) { return A == null || !X.test(A) || isNaN(A) }, type: function (A) { return A == null ? String(A) : B4[B6.call(A)] || "object" }, isPlainObject: function (A) { if (!A || b.type(A) !== "object" || A.nodeType || b.isWindow(A)) { return !1 } if (A.constructor && !B7.call(A, "constructor") && !B7.call(A.constructor.prototype, "isPrototypeOf")) { return !1 } var B; for (B in A) { } return B === As || B7.call(A, B) }, isEmptyObject: function (A) { for (var B in A) { return !1 } return !0 }, error: function (A) { throw A }, parseJSON: function (A) { if (typeof A !== "string" || !A) { return null } A = b.trim(A); if (Z.test(A.replace(a, "@").replace(L, "]").replace(M, ""))) { return Ar.JSON && Ar.JSON.parse ? Ar.JSON.parse(A) : (new Function("return " + A))() } b.error("Invalid JSON: " + A) }, parseXML: function (A, B, C) { Ar.DOMParser ? (C = new DOMParser, B = C.parseFromString(A, "text/xml")) : (B = new ActiveXObject("Microsoft.XMLDOM"), B.async = "false", B.loadXML(A)), C = B.documentElement, (!C || !C.nodeName || C.nodeName === "parsererror") && b.error("Invalid XML: " + A); return B }, noop: function () { }, globalEval: function (A) { if (A && U.test(A)) { var B = At.head || At.getElementsByTagName("head")[0] || At.documentElement, C = At.createElement("script"); b.support.scriptEval() ? C.appendChild(At.createTextNode(A)) : C.text = A, B.insertBefore(C, B.firstChild), B.removeChild(C) } }, nodeName: function (A, B) { return A.nodeName && A.nodeName.toUpperCase() === B.toUpperCase() }, each: function (C, D, E) { var F, G = 0, H = C.length, A = H === As || b.isFunction(C); if (E) { if (A) { for (F in C) { if (D.apply(C[F], E) === !1) { break } } } else { for (; G < H; ) { if (D.apply(C[G++], E) === !1) { break } } } } else { if (A) { for (F in C) { if (D.call(C[F], F, C[F]) === !1) { break } } } else { for (var B = C[0]; G < H && D.call(B, G, B) !== !1; B = C[++G]) { } } } return C }, trim: Ca ? function (A) { return A == null ? "" : Ca.call(A) } : function (A) { return A == null ? "" : (A + "").replace(V, "").replace(W, "") }, makeArray: function (A, B) { var C = B || []; if (A != null) { var D = b.type(A); A.length == null || D === "string" || D === "function" || D === "regexp" || b.isWindow(A) ? B8.call(C, A) : b.merge(C, A) } return C }, inArray: function (A, B) { if (B.indexOf) { return B.indexOf(A) } for (var C = 0, D = B.length; C < D; C++) { if (B[C] === A) { return C } } return -1 }, merge: function (A, B) { var C = A.length, D = 0; if (typeof B.length === "number") { for (var E = B.length; D < E; D++) { A[C++] = B[D] } } else { while (B[D] !== As) { A[C++] = B[D++] } } A.length = C; return A }, grep: function (A, B, C) { var D = [], E; C = !!C; for (var F = 0, G = A.length; F < G; F++) { E = !!B(A[F], F), C !== E && D.push(A[F]) } return D }, map: function (A, B, C) { var D = [], E; for (var F = 0, G = A.length; F < G; F++) { E = B(A[F], F, C), E != null && (D[D.length] = E) } return D.concat.apply([], D) }, guid: 1, proxy: function (A, B, C) { arguments.length === 2 && (typeof B === "string" ? (C = A, A = C[B], B = As) : B && !b.isFunction(B) && (C = B, B = As)), !B && A && (B = function () { return A.apply(C || this, arguments) }), A && (B.guid = A.guid = A.guid || B.guid || b.guid++); return B }, access: function (D, E, F, G, H, I) { var A = D.length; if (typeof E === "object") { for (var B in E) { b.access(D, B, E[B], G, H, F) } return D } if (F !== As) { G = !I && G && b.isFunction(F); for (var C = 0; C < A; C++) { H(D[C], E, G ? F.call(D[C], C, H(D[C], E)) : F, I) } return D } return A ? H(D[0], E) : As }, now: function () { return (new Date).getTime() }, _Deferred: function () { var A = [], B, C, D, E = { done: function () { if (!D) { var G = arguments, e, F, H, I, d; B && (d = B, B = 0); for (e = 0, F = G.length; e < F; e++) { H = G[e], I = b.type(H), I === "array" ? E.done.apply(E, H) : I === "function" && A.push(H) } d && E.resolveWith(d[0], d[1]) } return this }, resolveWith: function (F, G) { if (!D && !B && !C) { C = 1; try { while (A[0]) { A.shift().apply(F, G) } } catch (H) { throw H } finally { B = [F, G], C = 0 } } return this }, resolve: function () { E.resolveWith(b.isFunction(this.promise) ? this.promise() : this, arguments); return this }, isResolved: function () { return C || B }, cancel: function () { D = 1, A = []; return this } }; return E }, Deferred: function (A) { var B = b._Deferred(), C = b._Deferred(), D; b.extend(B, { then: function (E, F) { B.done(E).fail(F); return this }, fail: C.done, rejectWith: C.resolveWith, reject: C.resolve, isRejected: C.isResolved, promise: function (E) { if (E == null) { if (D) { return D } D = E = {} } var F = K.length; while (F--) { E[K[F]] = B[K[F]] } return E } }), B.done(C.cancel).fail(B.cancel), delete B.cancel, A && A.call(B, B); return B }, when: function (B) { var C = arguments.length, D = C <= 1 && B && b.isFunction(B.promise) ? B : b.Deferred(), E = D.promise(); if (C > 1) { var F = B9.call(arguments, 0), G = C, A = function (H) { return function (I) { F[H] = arguments.length > 1 ? B9.call(arguments, 0) : I, --G || D.resolveWith(E, F) } }; while (C--) { B = F[C], B && b.isFunction(B.promise) ? B.promise().then(A(C), D.reject) : --G } G || D.resolveWith(E, F) } else { D !== B && D.resolve(B) } return E }, uaMatch: function (A) { A = A.toLowerCase(); var B = N.exec(A) || O.exec(A) || P.exec(A) || A.indexOf("compatible") < 0 && Q.exec(A) || []; return { browser: B[1] || "", version: B[2] || "0"} }, sub: function () { function A(D, E) { return new A.fn.init(D, E) } b.extend(!0, A, this), A.superclass = this, A.fn = A.prototype = this(), A.fn.constructor = A, A.subclass = this.subclass, A.fn.init = function B(D, E) { E && E instanceof b && !(E instanceof A) && (E = A(E)); return b.fn.init.call(this, D, E, C) }, A.fn.init.prototype = A.fn; var C = A(At); return A }, browser: {} }), J = b._Deferred(), b.each("Boolean Number String Function Array Date RegExp Object".split(" "), function (A, B) { B4["[object " + B + "]"] = B.toLowerCase() }), S = b.uaMatch(R), S.browser && (b.browser[S.browser] = !0, b.browser.version = S.version), b.browser.webkit && (b.browser.safari = !0), Cb && (b.inArray = function (A, B) { return Cb.call(B, A) }), U.test("�") && (V = /^[\s\xA0]+/, W = /[\s\xA0]+$/), B2 = b(At), At.addEventListener ? B5 = function () { At.removeEventListener("DOMContentLoaded", B5, !1), b.ready() } : At.attachEvent && (B5 = function () { At.readyState === "complete" && (At.detachEvent("onreadystatechange", B5), b.ready()) }); return b } (); (function () { Au.support = {}; var G = At.createElement("div"); G.style.display = "none", G.innerHTML = "   <link/><table></table><a href='/a' style='color:red;float:left;opacity:.55;'>a</a><input type='checkbox'/>"; var H = G.getElementsByTagName("*"), I = G.getElementsByTagName("a")[0], J = At.createElement("select"), K = J.appendChild(At.createElement("option")), A = G.getElementsByTagName("input")[0]; if (H && H.length && I) { Au.support = { leadingWhitespace: G.firstChild.nodeType === 3, tbody: !G.getElementsByTagName("tbody").length, htmlSerialize: !!G.getElementsByTagName("link").length, style: /red/.test(I.getAttribute("style")), hrefNormalized: I.getAttribute("href") === "/a", opacity: /^0.55$/.test(I.style.opacity), cssFloat: !!I.style.cssFloat, checkOn: A.value === "on", optSelected: K.selected, deleteExpando: !0, optDisabled: !1, checkClone: !1, noCloneEvent: !0, noCloneChecked: !0, boxModel: null, inlineBlockNeedsLayout: !1, shrinkWrapBlocks: !1, reliableHiddenOffsets: !0 }, A.checked = !0, Au.support.noCloneChecked = A.cloneNode(!0).checked, J.disabled = !0, Au.support.optDisabled = !K.disabled; var B = null; Au.support.scriptEval = function () { if (B === null) { var L = At.documentElement, M = At.createElement("script"), N = "script" + Au.now(); try { M.appendChild(At.createTextNode("window." + N + "=1;")) } catch (O) { } L.insertBefore(M, L.firstChild), Ar[N] ? (B = !0, delete Ar[N]) : B = !1, L.removeChild(M), L = M = N = null } return B }; try { delete G.test } catch (C) { Au.support.deleteExpando = !1 } !G.addEventListener && G.attachEvent && G.fireEvent && (G.attachEvent("onclick", function D() { Au.support.noCloneEvent = !1, G.detachEvent("onclick", D) }), G.cloneNode(!0).fireEvent("onclick")), G = At.createElement("div"), G.innerHTML = "<input type='radio' name='radiotest' checked='checked'/>"; var E = At.createDocumentFragment(); E.appendChild(G.firstChild), Au.support.checkClone = E.cloneNode(!0).cloneNode(!0).lastChild.checked, Au(function () { var L = At.createElement("div"), M = At.getElementsByTagName("body")[0]; if (M) { L.style.width = L.style.paddingLeft = "1px", M.appendChild(L), Au.boxModel = Au.support.boxModel = L.offsetWidth === 2, "zoom" in L.style && (L.style.display = "inline", L.style.zoom = 1, Au.support.inlineBlockNeedsLayout = L.offsetWidth === 2, L.style.display = "", L.innerHTML = "<div style='width:4px;'></div>", Au.support.shrinkWrapBlocks = L.offsetWidth !== 2), L.innerHTML = "<table><tr><td style='padding:0;border:0;display:none'></td><td>t</td></tr></table>"; var N = L.getElementsByTagName("td"); Au.support.reliableHiddenOffsets = N[0].offsetHeight === 0, N[0].style.display = "", N[1].style.display = "none", Au.support.reliableHiddenOffsets = Au.support.reliableHiddenOffsets && N[0].offsetHeight === 0, L.innerHTML = "", M.removeChild(L).style.display = "none", L = N = null } }); var F = function (L) { var M = At.createElement("div"); L = "on" + L; if (!M.attachEvent) { return !0 } var N = L in M; N || (M.setAttribute(L, "return;"), N = typeof M[L] === "function"), M = null; return N }; Au.support.submitBubbles = F("submit"), Au.support.changeBubbles = F("change"), G = H = I = null } })(); var Av = /^(?:\{.*\}|\[.*\])$/; Au.extend({ cache: {}, uuid: 0, expando: "jQuery" + (Au.fn.jquery + Math.random()).replace(/\D/g, ""), noData: { embed: !0, object: "clsid:D27CDB6E-AE6D-11cf-96B8-444553540000", applet: !0 }, hasData: function (A) { A = A.nodeType ? Au.cache[A[Au.expando]] : A[Au.expando]; return !!A && !Ax(A) }, data: function (E, F, G, H) { if (Au.acceptData(E)) { var I = Au.expando, J = typeof F === "string", A, B = E.nodeType, C = B ? Au.cache : E, D = B ? E[Au.expando] : E[Au.expando] && Au.expando; if ((!D || H && D && !C[D][I]) && J && G === As) { return } D || (B ? E[Au.expando] = D = ++Au.uuid : D = Au.expando), C[D] || (C[D] = {}, B || (C[D].toJSON = Au.noop)); if (typeof F === "object" || typeof F === "function") { H ? C[D][I] = Au.extend(C[D][I], F) : C[D] = Au.extend(C[D], F) } A = C[D], H && (A[I] || (A[I] = {}), A = A[I]), G !== As && (A[F] = G); if (F === "events" && !A[F]) { return A[I] && A[I].events } return J ? A[F] : A } }, removeData: function (E, F, G) { if (Au.acceptData(E)) { var H = Au.expando, I = E.nodeType, A = I ? Au.cache : E, B = I ? E[Au.expando] : Au.expando; if (!A[B]) { return } if (F) { var C = G ? A[B][H] : A[B]; if (C) { delete C[F]; if (!Ax(C)) { return } } } if (G) { delete A[B][H]; if (!Ax(A[B])) { return } } var D = A[B][H]; Au.support.deleteExpando || A != Ar ? delete A[B] : A[B] = null, D ? (A[B] = {}, I || (A[B].toJSON = Au.noop), A[B][H] = D) : I && (Au.support.deleteExpando ? delete E[Au.expando] : E.removeAttribute ? E.removeAttribute(Au.expando) : E[Au.expando] = null) } }, _data: function (A, B, C) { return Au.data(A, B, C, !0) }, acceptData: function (A) { if (A.nodeName) { var B = Au.noData[A.nodeName.toLowerCase()]; if (B) { return B !== !0 && A.getAttribute("classid") === B } } return !0 } }), Au.fn.extend({ data: function (D, E) { var F = null; if (typeof D === "undefined") { if (this.length) { F = Au.data(this[0]); if (this[0].nodeType === 1) { var G = this[0].attributes, H; for (var A = 0, B = G.length; A < B; A++) { H = G[A].name, H.indexOf("data-") === 0 && (H = H.substr(5), Aw(this[0], H, F[H])) } } } return F } if (typeof D === "object") { return this.each(function () { Au.data(this, D) }) } var C = D.split("."); C[1] = C[1] ? "." + C[1] : ""; if (E === As) { F = this.triggerHandler("getData" + C[1] + "!", [C[0]]), F === As && this.length && (F = Au.data(this[0], D), F = Aw(this[0], D, F)); return F === As && C[1] ? this.data(C[0]) : F } return this.each(function () { var I = Au(this), J = [C[0], E]; I.triggerHandler("setData" + C[1] + "!", J), Au.data(this, D, E), I.triggerHandler("changeData" + C[1] + "!", J) }) }, removeData: function (A) { return this.each(function () { Au.removeData(this, A) }) } }), Au.extend({ queue: function (A, B, C) { if (A) { B = (B || "fx") + "queue"; var D = Au._data(A, B); if (!C) { return D || [] } !D || Au.isArray(C) ? D = Au._data(A, B, Au.makeArray(C)) : D.push(C); return D } }, dequeue: function (A, B) { B = B || "fx"; var C = Au.queue(A, B), D = C.shift(); D === "inprogress" && (D = C.shift()), D && (B === "fx" && C.unshift("inprogress"), D.call(A, function () { Au.dequeue(A, B) })), C.length || Au.removeData(A, B + "queue", !0) } }), Au.fn.extend({ queue: function (A, B) { typeof A !== "string" && (B = A, A = "fx"); if (B === As) { return Au.queue(this[0], A) } return this.each(function (C) { var D = Au.queue(this, A, B); A === "fx" && D[0] !== "inprogress" && Au.dequeue(this, A) }) }, dequeue: function (A) { return this.each(function () { Au.dequeue(this, A) }) }, delay: function (A, B) { A = Au.fx ? Au.fx.speeds[A] || A : A, B = B || "fx"; return this.queue(B, function () { var C = this; setTimeout(function () { Au.dequeue(C, B) }, A) }) }, clearQueue: function (A) { return this.queue(A || "fx", []) } }); var Ai = /[\n\t\r]/g, Aj = /\s+/, Ak = /\r/g, AA = /^(?:href|src|style)$/, Am = /^(?:button|input)$/i, An = /^(?:button|input|object|select|textarea)$/i, Az = /^a(?:rea)?$/i, Ap = /^(?:radio|checkbox)$/i; Au.props = { "for": "htmlFor", "class": "className", readonly: "readOnly", maxlength: "maxLength", cellspacing: "cellSpacing", rowspan: "rowSpan", colspan: "colSpan", tabindex: "tabIndex", usemap: "useMap", frameborder: "frameBorder" }, Au.fn.extend({ attr: function (A, B) { return Au.access(this, A, B, !0, Au.attr) }, removeAttr: function (A, B) { return this.each(function () { Au.attr(this, A, ""), this.nodeType === 1 && this.removeAttribute(A) }) }, addClass: function (C) { if (Au.isFunction(C)) { return this.each(function (J) { var K = Au(this); K.addClass(C.call(this, J, K.attr("class"))) }) } if (C && typeof C === "string") { var D = (C || "").split(Aj); for (var E = 0, F = this.length; E < F; E++) { var G = this[E]; if (G.nodeType === 1) { if (G.className) { var H = " " + G.className + " ", I = G.className; for (var A = 0, B = D.length; A < B; A++) { H.indexOf(" " + D[A] + " ") < 0 && (I += " " + D[A]) } G.className = Au.trim(I) } else { G.className = C } } } } return this }, removeClass: function (D) { if (Au.isFunction(D)) { return this.each(function (I) { var J = Au(this); J.removeClass(D.call(this, I, J.attr("class"))) }) } if (D && typeof D === "string" || D === As) { var E = (D || "").split(Aj); for (var F = 0, G = this.length; F < G; F++) { var H = this[F]; if (H.nodeType === 1 && H.className) { if (D) { var A = (" " + H.className + " ").replace(Ai, " "); for (var B = 0, C = E.length; B < C; B++) { A = A.replace(" " + E[B] + " ", " ") } H.className = Au.trim(A) } else { H.className = "" } } } } return this }, toggleClass: function (A, B) { var C = typeof A, D = typeof B === "boolean"; if (Au.isFunction(A)) { return this.each(function (E) { var F = Au(this); F.toggleClass(A.call(this, E, F.attr("class"), B), B) }) } return this.each(function () { if (C === "string") { var H, I = 0, F = Au(this), G = B, E = A.split(Aj); while (H = E[I++]) { G = D ? G : !F.hasClass(H), F[G ? "addClass" : "removeClass"](H) } } else { if (C === "undefined" || C === "boolean") { this.className && Au._data(this, "__className__", this.className), this.className = this.className || A === !1 ? "" : Au._data(this, "__className__") || "" } } }) }, hasClass: function (A) { var B = " " + A + " "; for (var C = 0, D = this.length; C < D; C++) { if ((" " + this[C].className + " ").replace(Ai, " ").indexOf(B) > -1) { return !0 } } return !1 }, val: function (F) { if (!arguments.length) { var G = this[0]; if (G) { if (Au.nodeName(G, "option")) { var H = G.attributes.value; return !H || H.specified ? G.value : G.text } if (Au.nodeName(G, "select")) { var I = G.selectedIndex, J = [], K = G.options, A = G.type === "select-one"; if (I < 0) { return null } for (var B = A ? I : 0, C = A ? I + 1 : K.length; B < C; B++) { var D = K[B]; if (D.selected && (Au.support.optDisabled ? !D.disabled : D.getAttribute("disabled") === null) && (!D.parentNode.disabled || !Au.nodeName(D.parentNode, "optgroup"))) { F = Au(D).val(); if (A) { return F } J.push(F) } } if (A && !J.length && K.length) { return Au(K[I]).val() } return J } if (Ap.test(G.type) && !Au.support.checkOn) { return G.getAttribute("value") === null ? "on" : G.value } return (G.value || "").replace(Ak, "") } return As } var E = Au.isFunction(F); return this.each(function (L) { var M = Au(this), N = F; if (this.nodeType === 1) { E && (N = F.call(this, L, M.val())), N == null ? N = "" : typeof N === "number" ? N += "" : Au.isArray(N) && (N = Au.map(N, function (P) { return P == null ? "" : P + "" })); if (Au.isArray(N) && Ap.test(this.type)) { this.checked = Au.inArray(M.val(), N) >= 0 } else { if (Au.nodeName(this, "select")) { var O = Au.makeArray(N); Au("option", this).each(function () { this.selected = Au.inArray(Au(this).val(), O) >= 0 }), O.length || (this.selectedIndex = -1) } else { this.value = N } } } }) } }), Au.extend({ attrFn: { val: !0, css: !0, html: !0, text: !0, data: !0, width: !0, height: !0, offset: !0 }, attr: function (E, F, G, H) { if (!E || E.nodeType === 3 || E.nodeType === 8 || E.nodeType === 2) { return As } if (H && F in Au.attrFn) { return Au(E)[F](G) } var I = E.nodeType !== 1 || !Au.isXMLDoc(E), J = G !== As; F = I && Au.props[F] || F; if (E.nodeType === 1) { var A = AA.test(F); if (F === "selected" && !Au.support.optSelected) { var B = E.parentNode; B && (B.selectedIndex, B.parentNode && B.parentNode.selectedIndex) } if ((F in E || E[F] !== As) && I && !A) { J && (F === "type" && Am.test(E.nodeName) && E.parentNode && Au.error("type property can't be changed"), G === null ? E.nodeType === 1 && E.removeAttribute(F) : E[F] = G); if (Au.nodeName(E, "form") && E.getAttributeNode(F)) { return E.getAttributeNode(F).nodeValue } if (F === "tabIndex") { var D = E.getAttributeNode("tabIndex"); return D && D.specified ? D.value : An.test(E.nodeName) || Az.test(E.nodeName) && E.href ? 0 : As } return E[F] } if (!Au.support.style && I && F === "style") { J && (E.style.cssText = "" + G); return E.style.cssText } J && E.setAttribute(F, "" + G); if (!E.attributes[F] && (E.hasAttribute && !E.hasAttribute(F))) { return As } var C = !Au.support.hrefNormalized && I && A ? E.getAttribute(F, 2) : E.getAttribute(F); return C === null ? As : C } J && (E[F] = G); return E[F] } }); var Aa = /\.(.*)$/, Ab = /^(?:textarea|input|select)$/i, Al = /\./g, Ad = / /g, Ae = /[^\w\s.|`]/g, Af = function (A) { return A.replace(Ae, "\\$&") }; Au.event = { add: function (I, J, K, L) { if (I.nodeType !== 3 && I.nodeType !== 8) { try { Au.isWindow(I) && (I !== Ar && !I.frameElement) && (I = Ar) } catch (O) { } if (K === !1) { K = Ag } else { if (!K) { return } } var A, B; K.handler && (A = K, K = A.handler), K.guid || (K.guid = Au.guid++); var C = Au._data(I); if (!C) { return } var D = C.events, E = C.handle; D || (C.events = D = {}), E || (C.handle = E = function () { return typeof Au !== "undefined" && !Au.event.triggered ? Au.event.handle.apply(E.elem, arguments) : As }), E.elem = I, J = J.split(" "); var F, G = 0, H; while (F = J[G++]) { B = A ? Au.extend({}, A) : { handler: K, data: L }, F.indexOf(".") > -1 ? (H = F.split("."), F = H.shift(), B.namespace = H.slice(0).sort().join(".")) : (H = [], B.namespace = ""), B.type = F, B.guid || (B.guid = K.guid); var M = D[F], N = Au.event.special[F] || {}; if (!M) { M = D[F] = []; if (!N.setup || N.setup.call(I, L, H, E) === !1) { I.addEventListener ? I.addEventListener(F, E, !1) : I.attachEvent && I.attachEvent("on" + F, E) } } N.add && (N.add.call(I, B), B.handler.guid || (B.handler.guid = K.guid)), M.push(B), Au.event.global[F] = !0 } I = null } }, global: {}, remove: function (O, P, Q, R) { if (O.nodeType !== 3 && O.nodeType !== 8) { Q === !1 && (Q = Ag); var S, G, H, I, J = 0, K, L, M, N, A, B, C, D = Au.hasData(O) && Au._data(O), E = D && D.events; if (!D || !E) { return } P && P.type && (Q = P.handler, P = P.type); if (!P || typeof P === "string" && P.charAt(0) === ".") { P = P || ""; for (G in E) { Au.event.remove(O, G + P) } return } P = P.split(" "); while (G = P[J++]) { C = G, B = null, K = G.indexOf(".") < 0, L = [], K || (L = G.split("."), G = L.shift(), M = new RegExp("(^|\\.)" + Au.map(L.slice(0).sort(), Af).join("\\.(?:.*\\.)?") + "(\\.|$)")), A = E[G]; if (!A) { continue } if (!Q) { for (I = 0; I < A.length; I++) { B = A[I]; if (K || M.test(B.namespace)) { Au.event.remove(O, C, B.handler, I), A.splice(I--, 1) } } continue } N = Au.event.special[G] || {}; for (I = R || 0; I < A.length; I++) { B = A[I]; if (Q.guid === B.guid) { if (K || M.test(B.namespace)) { R == null && A.splice(I--, 1), N.remove && N.remove.call(O, B) } if (R != null) { break } } } if (A.length === 0 || R != null && A.length === 1) { (!N.teardown || N.teardown.call(O, L) === !1) && Au.removeEvent(O, G, D.handle), S = null, delete E[G] } } if (Au.isEmptyObject(E)) { var F = D.handle; F && (F.elem = null), delete D.events, delete D.handle, Au.isEmptyObject(D) && Au.removeData(O, As, !0) } } }, trigger: function (H, I, J) { var K = H.type || H, L = arguments[3]; if (!L) { H = typeof H === "object" ? H[Au.expando] ? H : Au.extend(Au.Event(K), H) : Au.Event(K), K.indexOf("!") >= 0 && (H.type = K = K.slice(0, -1), H.exclusive = !0), J || (H.stopPropagation(), Au.event.global[K] && Au.each(Au.cache, function () { var O = Au.expando, P = this[O]; P && P.events && P.events[K] && Au.event.trigger(H, I, P.handle.elem) })); if (!J || J.nodeType === 3 || J.nodeType === 8) { return As } H.result = As, H.target = J, I = Au.makeArray(I), I.unshift(H) } H.currentTarget = J; var N = Au._data(J, "handle"); N && N.apply(J, I); var A = J.parentNode || J.ownerDocument; try { J && J.nodeName && Au.noData[J.nodeName.toLowerCase()] || J["on" + K] && J["on" + K].apply(J, I) === !1 && (H.result = !1, H.preventDefault()) } catch (B) { } if (!H.isPropagationStopped() && A) { Au.event.trigger(H, I, A, !0) } else { if (!H.isDefaultPrevented()) { var C, D = H.target, E = K.replace(Aa, ""), F = Au.nodeName(D, "a") && E === "click", G = Au.event.special[E] || {}; if ((!G._default || G._default.call(J, H) === !1) && !F && !(D && D.nodeName && Au.noData[D.nodeName.toLowerCase()])) { try { D[E] && (C = D["on" + E], C && (D["on" + E] = null), Au.event.triggered = !0, D[E]()) } catch (M) { } C && (D["on" + E] = C), Au.event.triggered = !1 } } } }, handle: function (H) { var I, J, K, L, A, B = [], C = Au.makeArray(arguments); H = C[0] = Au.event.fix(H || Ar.event), H.currentTarget = this, I = H.type.indexOf(".") < 0 && !H.exclusive, I || (K = H.type.split("."), H.type = K.shift(), B = K.slice(0).sort(), L = new RegExp("(^|\\.)" + B.join("\\.(?:.*\\.)?") + "(\\.|$)")), H.namespace = H.namespace || B.join("."), A = Au._data(this, "events"), J = (A || {})[H.type]; if (A && J) { J = J.slice(0); for (var D = 0, E = J.length; D < E; D++) { var F = J[D]; if (I || L.test(F.namespace)) { H.handler = F.handler, H.data = F.data, H.handleObj = F; var G = F.handler.apply(this, C); G !== As && (H.result = G, G === !1 && (H.preventDefault(), H.stopPropagation())); if (H.isImmediatePropagationStopped()) { break } } } } return H.result }, props: "altKey attrChange attrName bubbles button cancelable charCode clientX clientY ctrlKey currentTarget data detail eventPhase fromElement handler keyCode layerX layerY metaKey newValue offsetX offsetY pageX pageY prevValue relatedNode relatedTarget screenX screenY shiftKey srcElement target toElement view wheelDelta which".split(" "), fix: function (A) { if (A[Au.expando]) { return A } var D = A; A = Au.Event(D); for (var E = this.props.length, F; E; ) { F = this.props[--E], A[F] = D[F] } A.target || (A.target = A.srcElement || At), A.target.nodeType === 3 && (A.target = A.target.parentNode), !A.relatedTarget && A.fromElement && (A.relatedTarget = A.fromElement === A.target ? A.toElement : A.fromElement); if (A.pageX == null && A.clientX != null) { var B = At.documentElement, C = At.body; A.pageX = A.clientX + (B && B.scrollLeft || C && C.scrollLeft || 0) - (B && B.clientLeft || C && C.clientLeft || 0), A.pageY = A.clientY + (B && B.scrollTop || C && C.scrollTop || 0) - (B && B.clientTop || C && C.clientTop || 0) } A.which == null && (A.charCode != null || A.keyCode != null) && (A.which = A.charCode != null ? A.charCode : A.keyCode), !A.metaKey && A.ctrlKey && (A.metaKey = A.ctrlKey), !A.which && A.button !== As && (A.which = A.button & 1 ? 1 : A.button & 2 ? 3 : A.button & 4 ? 2 : 0); return A }, guid: 100000000, proxy: Au.proxy, special: { ready: { setup: Au.bindReady, teardown: Au.noop }, live: { add: function (A) { Au.event.add(this, BU(A.origType, A.selector), Au.extend({}, A, { handler: BT, guid: A.handler.guid })) }, remove: function (A) { Au.event.remove(this, BU(A.origType, A.selector), A) } }, beforeunload: { setup: function (A, B, C) { Au.isWindow(this) && (this.onbeforeunload = C) }, teardown: function (A, B) { this.onbeforeunload === B && (this.onbeforeunload = null) } }} }, Au.removeEvent = At.removeEventListener ? function (A, B, C) { A.removeEventListener && A.removeEventListener(B, C, !1) } : function (A, B, C) { A.detachEvent && A.detachEvent("on" + B, C) }, Au.Event = function (A) { if (!this.preventDefault) { return new Au.Event(A) } A && A.type ? (this.originalEvent = A, this.type = A.type, this.isDefaultPrevented = A.defaultPrevented || A.returnValue === !1 || A.getPreventDefault && A.getPreventDefault() ? Ah : Ag) : this.type = A, this.timeStamp = Au.now(), this[Au.expando] = !0 }, Au.Event.prototype = { preventDefault: function () { this.isDefaultPrevented = Ah; var A = this.originalEvent; A && (A.preventDefault ? A.preventDefault() : A.returnValue = !1) }, stopPropagation: function () { this.isPropagationStopped = Ah; var A = this.originalEvent; A && (A.stopPropagation && A.stopPropagation(), A.cancelBubble = !0) }, stopImmediatePropagation: function () { this.isImmediatePropagationStopped = Ah, this.stopPropagation() }, isDefaultPrevented: Ag, isPropagationStopped: Ag, isImmediatePropagationStopped: Ag }; var BW = function (A) { var B = A.relatedTarget; try { if (B !== At && !B.parentNode) { return } while (B && B !== this) { B = B.parentNode } B !== this && (A.type = A.data, Au.event.handle.apply(this, arguments)) } catch (C) { } }, BX = function (A) { A.type = A.data, Au.event.handle.apply(this, arguments) }; Au.each({ mouseenter: "mouseover", mouseleave: "mouseout" }, function (A, B) { Au.event.special[A] = { setup: function (C) { Au.event.add(this, B, C && C.selector ? BX : BW, A) }, teardown: function (C) { Au.event.remove(this, B, C && C.selector ? BX : BW) } } }), Au.support.submitBubbles || (Au.event.special.submit = { setup: function (A, B) { if (this.nodeName && this.nodeName.toLowerCase() !== "form") { Au.event.add(this, "click.specialSubmit", function (C) { var D = C.target, E = D.type; (E === "submit" || E === "image") && Au(D).closest("form").length && BR("submit", this, arguments) }), Au.event.add(this, "keypress.specialSubmit", function (C) { var D = C.target, E = D.type; (E === "text" || E === "password") && Au(D).closest("form").length && C.keyCode === 13 && BR("submit", this, arguments) }) } else { return !1 } }, teardown: function (A) { Au.event.remove(this, ".specialSubmit") } }); if (!Au.support.changeBubbles) { var BY, BP = function (A) { var B = A.type, C = A.value; B === "radio" || B === "checkbox" ? C = A.checked : B === "select-multiple" ? C = A.selectedIndex > -1 ? Au.map(A.options, function (D) { return D.selected }).join("-") : "" : A.nodeName.toLowerCase() === "select" && (C = A.selectedIndex); return C }, BQ = function BQ(A) { var B = A.target, C, D; if (Ab.test(B.nodeName) && !B.readOnly) { C = Au._data(B, "_change_data"), D = BP(B), (A.type !== "focusout" || B.type !== "radio") && Au._data(B, "_change_data", D); if (C === As || D === C) { return } if (C != null || D) { A.type = "change", A.liveFired = As, Au.event.trigger(A, arguments[1], B) } } }; Au.event.special.change = { filters: { focusout: BQ, beforedeactivate: BQ, click: function (A) { var B = A.target, C = B.type; (C === "radio" || C === "checkbox" || B.nodeName.toLowerCase() === "select") && BQ.call(this, A) }, keydown: function (A) { var B = A.target, C = B.type; (A.keyCode === 13 && B.nodeName.toLowerCase() !== "textarea" || A.keyCode === 32 && (C === "checkbox" || C === "radio") || C === "select-multiple") && BQ.call(this, A) }, beforeactivate: function (A) { var B = A.target; Au._data(B, "_change_data", BP(B)) } }, setup: function (A, B) { if (this.type === "file") { return !1 } for (var C in BY) { Au.event.add(this, C + ".specialChange", BY[C]) } return Ab.test(this.nodeName) }, teardown: function (A) { Au.event.remove(this, ".specialChange"); return Ab.test(this.nodeName) } }, BY = Au.event.special.change.filters, BY.focus = BY.beforeactivate } At.addEventListener && Au.each({ focus: "focusin", blur: "focusout" }, function (A, B) { function C(D) { D = Au.event.fix(D), D.type = B; return Au.event.handle.call(this, D) } Au.event.special[B] = { setup: function () { this.addEventListener(A, C, !0) }, teardown: function () { this.removeEventListener(A, C, !0) } } }), Au.each(["bind", "one"], function (A, B) { Au.fn[B] = function (D, G, H) { if (typeof D === "object") { for (var I in D) { this[B](I, G, D[I], H) } return this } if (Au.isFunction(G) || G === !1) { H = G, G = As } var E = B === "one" ? Au.proxy(H, function (J) { Au(this).unbind(J, E); return H.apply(this, arguments) }) : H; if (D === "unload" && B !== "one") { this.one(D, G, H) } else { for (var F = 0, C = this.length; F < C; F++) { Au.event.add(this[F], D, E, G) } } return this } }), Au.fn.extend({ unbind: function (A, B) { if (typeof A !== "object" || A.preventDefault) { for (var D = 0, E = this.length; D < E; D++) { Au.event.remove(this[D], A, B) } } else { for (var C in A) { this.unbind(C, A[C]) } } return this }, delegate: function (A, B, C, D) { return this.live(B, C, D, A) }, undelegate: function (A, B, C) { return arguments.length === 0 ? this.unbind("live") : this.die(B, null, C, A) }, trigger: function (A, B) { return this.each(function () { Au.event.trigger(A, B, this) }) }, triggerHandler: function (A, B) { if (this[0]) { var C = Au.Event(A); C.preventDefault(), C.stopPropagation(), Au.event.trigger(C, B, this[0]); return C.result } }, toggle: function (A) { var B = arguments, C = 1; while (C < B.length) { Au.proxy(A, B[C++]) } return this.click(Au.proxy(A, function (D) { var E = (Au._data(this, "lastToggle" + A.guid) || 0) % C; Au._data(this, "lastToggle" + A.guid, E + 1), D.preventDefault(); return B[E].apply(this, arguments) || !1 })) }, hover: function (A, B) { return this.mouseenter(A).mouseleave(B || A) } }); var BS = { focus: "focusin", blur: "focusout", mouseenter: "mouseover", mouseleave: "mouseout" }; Au.each(["live", "die"], function (A, B) { Au.fn[B] = function (J, K, L, M) { var P, C = 0, D, E, F, G = M || this.selector, H = M ? this : Au(this.context); if (typeof J === "object" && !J.preventDefault) { for (var I in J) { H[B](I, K, J[I], G) } return this } Au.isFunction(K) && (L = K, K = As), J = (J || "").split(" "); while ((P = J[C++]) != null) { D = Aa.exec(P), E = "", D && (E = D[0], P = P.replace(Aa, "")); if (P === "hover") { J.push("mouseenter" + E, "mouseleave" + E); continue } F = P, P === "focus" || P === "blur" ? (J.push(BS[P] + E), P = P + E) : P = (BS[P] || P) + E; if (B === "live") { for (var N = 0, O = H.length; N < O; N++) { Au.event.add(H[N], "live." + BU(P, G), { data: K, selector: G, handler: L, origType: P, origHandler: L, preType: F }) } } else { H.unbind("live." + BU(P, G), L) } } return this } }), Au.each("blur focus focusin focusout load resize scroll unload click dblclick mousedown mouseup mousemove mouseover mouseout mouseenter mouseleave change select submit keydown keypress keyup error".split(" "), function (A, B) { Au.fn[B] = function (C, D) { D == null && (D = C, C = null); return arguments.length > 0 ? this.bind(B, C, D) : this.trigger(B) }, Au.attrFn && (Au.attrFn[B] = !0) }), function () { function F(V, W, X, Y, Z, k) { for (var l = 0, m = Y.length; l < m; l++) { var T = Y[l]; if (T) { var U = !1; T = T[V]; while (T) { if (T.sizcache === X) { U = Y[T.sizset]; break } if (T.nodeType === 1) { k || (T.sizcache = X, T.sizset = l); if (typeof W !== "string") { if (T === W) { U = !0; break } } else { if (K.filter(W, [T]).length > 0) { U = T; break } } } T = T[V] } Y[l] = U } } } function E(V, W, X, Y, Z, k) { for (var l = 0, m = Y.length; l < m; l++) { var T = Y[l]; if (T) { var U = !1; T = T[V]; while (T) { if (T.sizcache === X) { U = Y[T.sizset]; break } T.nodeType === 1 && !k && (T.sizcache = X, T.sizset = l); if (T.nodeName.toLowerCase() === W) { U = T; break } T = T[V] } Y[l] = U } } } var P = /((?:\((?:\([^()]+\)|[^()]+)+\)|\[(?:\[[^\[\]]*\]|['"][^'"]*['"]|[^\[\]'"]+)+\]|\\.|[^ >+~,(\[\\]+)+|[>+~])(\s*,\s*)?((?:.|\r|\n)*)/g, Q = 0, R = Object.prototype.toString, S = !1, H = !0, I = /\\/g, J = /\W/; [0, 0].sort(function () { H = !1; return 0 }); var K = function (m, p, v, z) { v = v || [], p = p || At; var a = p; if (p.nodeType !== 1 && p.nodeType !== 9) { return [] } if (!m || typeof m !== "string") { return v } var c, f, k, l, U, V, W, X, Y = !0, Z = K.isXML(p), B1 = [], T = m; do { P.exec(""), c = P.exec(T); if (c) { T = c[3], B1.push(c[1]); if (c[2]) { l = c[3]; break } } } while (c); if (B1.length > 1 && M.exec(m)) { if (B1.length === 2 && L.relative[B1[0]]) { f = G(B1[0] + B1[1], p) } else { f = L.relative[B1[0]] ? [p] : K(B1.shift(), p); while (B1.length) { m = B1.shift(), L.relative[m] && (m += B1.shift()), f = G(m, f) } } } else { !z && B1.length > 1 && p.nodeType === 9 && !Z && L.match.ID.test(B1[0]) && !L.match.ID.test(B1[B1.length - 1]) && (U = K.find(B1.shift(), p, Z), p = U.expr ? K.filter(U.expr, U.set)[0] : U.set[0]); if (p) { U = z ? { expr: B1.pop(), set: A(z)} : K.find(B1.pop(), B1.length === 1 && (B1[0] === "~" || B1[0] === "+") && p.parentNode ? p.parentNode : p, Z), f = U.expr ? K.filter(U.expr, U.set) : U.set, B1.length > 0 ? k = A(f) : Y = !1; while (B1.length) { V = B1.pop(), W = V, L.relative[V] ? W = B1.pop() : V = "", W == null && (W = p), L.relative[V](k, W, Z) } } else { k = B1 = [] } } k || (k = f), k || K.error(V || m); if (R.call(k) === "[object Array]") { if (Y) { if (p && p.nodeType === 1) { for (X = 0; k[X] != null; X++) { k[X] && (k[X] === !0 || k[X].nodeType === 1 && K.contains(p, k[X])) && v.push(f[X]) } } else { for (X = 0; k[X] != null; X++) { k[X] && k[X].nodeType === 1 && v.push(f[X]) } } } else { v.push.apply(v, k) } } else { A(k, v) } l && (K(l, a, v, z), K.uniqueSort(v)); return v }; K.uniqueSort = function (T) { if (C) { S = H, T.sort(C); if (S) { for (var U = 1; U < T.length; U++) { T[U] === T[U - 1] && T.splice(U--, 1) } } } return T }, K.matches = function (T, U) { return K(T, null, null, U) }, K.matchesSelector = function (T, U) { return K(U, null, null, [T]).length > 0 }, K.find = function (U, V, W) { var X; if (!U) { return [] } for (var Y = 0, Z = L.order.length; Y < Z; Y++) { var i, k = L.order[Y]; if (i = L.leftMatch[k].exec(U)) { var T = i[1]; i.splice(1, 1); if (T.substr(T.length - 1) !== "\\") { i[1] = (i[1] || "").replace(I, ""), X = L.find[k](i, V, W); if (X != null) { U = U.replace(L.match[k], ""); break } } } } X || (X = typeof V.getElementsByTagName !== "undefined" ? V.getElementsByTagName("*") : []); return { set: X, expr: U} }, K.filter = function (v, w, x, y) { var z, B1, Y = v, Z = [], b = w, k = w && w[0] && K.isXML(w[0]); while (v && w.length) { for (var l in L.filter) { if ((z = L.leftMatch[l].exec(v)) != null && z[2]) { var u, T, U = L.filter[l], V = z[1]; B1 = !1, z.splice(1, 1); if (V.substr(V.length - 1) === "\\") { continue } b === Z && (Z = []); if (L.preFilter[l]) { z = L.preFilter[l](z, b, x, Z, y, k); if (z) { if (z === !0) { continue } } else { B1 = u = !0 } } if (z) { for (var W = 0; (T = b[W]) != null; W++) { if (T) { u = U(T, z, W, b); var X = y ^ !!u; x && u != null ? X ? B1 = !0 : b[W] = !1 : X && (Z.push(T), B1 = !0) } } } if (u !== As) { x || (b = Z), v = v.replace(L.match[l], ""); if (!B1) { return [] } break } } } if (v === Y) { if (B1 == null) { K.error(v) } else { break } } Y = v } return b }, K.error = function (T) { throw "Syntax error, unrecognized expression: " + T }; var L = K.selectors = { order: ["ID", "NAME", "TAG"], match: { ID: /#((?:[\w\u00c0-\uFFFF\-]|\\.)+)/, CLASS: /\.((?:[\w\u00c0-\uFFFF\-]|\\.)+)/, NAME: /\[name=['"]*((?:[\w\u00c0-\uFFFF\-]|\\.)+)['"]*\]/, ATTR: /\[\s*((?:[\w\u00c0-\uFFFF\-]|\\.)+)\s*(?:(\S?=)\s*(?:(['"])(.*?)\3|(#?(?:[\w\u00c0-\uFFFF\-]|\\.)*)|)|)\s*\]/, TAG: /^((?:[\w\u00c0-\uFFFF\*\-]|\\.)+)/, CHILD: /:(only|nth|last|first)-child(?:\(\s*(even|odd|(?:[+\-]?\d+|(?:[+\-]?\d*)?n\s*(?:[+\-]\s*\d+)?))\s*\))?/, POS: /:(nth|eq|gt|lt|first|last|even|odd)(?:\((\d*)\))?(?=[^\-]|$)/, PSEUDO: /:((?:[\w\u00c0-\uFFFF\-]|\\.)+)(?:\((['"]?)((?:\([^\)]+\)|[^\(\)]*)+)\2\))?/ }, leftMatch: {}, attrMap: { "class": "className", "for": "htmlFor" }, attrHandle: { href: function (T) { return T.getAttribute("href") }, type: function (T) { return T.getAttribute("type") } }, relative: { "+": function (T, U) { var V = typeof U === "string", W = V && !J.test(U), X = V && !W; W && (U = U.toLowerCase()); for (var Y = 0, Z = T.length, i; Y < Z; Y++) { if (i = T[Y]) { while ((i = i.previousSibling) && i.nodeType !== 1) { } T[Y] = X || i && i.nodeName.toLowerCase() === U ? i || !1 : i === U } } X && K.filter(U, T, !0) }, ">": function (T, U) { var V, W = typeof U === "string", X = 0, Y = T.length; if (W && !J.test(U)) { U = U.toLowerCase(); for (; X < Y; X++) { V = T[X]; if (V) { var Z = V.parentNode; T[X] = Z.nodeName.toLowerCase() === U ? Z : !1 } } } else { for (; X < Y; X++) { V = T[X], V && (T[X] = W ? V.parentNode : V.parentNode === U) } W && K.filter(U, T, !0) } }, "": function (T, U, V) { var W, X = Q++, Y = F; typeof U === "string" && !J.test(U) && (U = U.toLowerCase(), W = U, Y = E), Y("parentNode", U, X, T, W, V) }, "~": function (T, U, V) { var W, X = Q++, Y = F; typeof U === "string" && !J.test(U) && (U = U.toLowerCase(), W = U, Y = E), Y("previousSibling", U, X, T, W, V) } }, find: { ID: function (T, U, V) { if (typeof U.getElementById !== "undefined" && !V) { var W = U.getElementById(T[1]); return W && W.parentNode ? [W] : [] } }, NAME: function (T, U) { if (typeof U.getElementsByName !== "undefined") { var V = [], W = U.getElementsByName(T[1]); for (var X = 0, Y = W.length; X < Y; X++) { W[X].getAttribute("name") === T[1] && V.push(W[X]) } return V.length === 0 ? null : V } }, TAG: function (T, U) { if (typeof U.getElementsByTagName !== "undefined") { return U.getElementsByTagName(T[1]) } } }, preFilter: { CLASS: function (T, U, V, W, X, Y) { T = " " + T[1].replace(I, "") + " "; if (Y) { return T } for (var Z = 0, i; (i = U[Z]) != null; Z++) { i && (X ^ (i.className && (" " + i.className + " ").replace(/[\t\n\r]/g, " ").indexOf(T) >= 0) ? V || W.push(i) : V && (U[Z] = !1)) } return !1 }, ID: function (T) { return T[1].replace(I, "") }, TAG: function (T, U) { return T[1].replace(I, "").toLowerCase() }, CHILD: function (T) { if (T[1] === "nth") { T[2] || K.error(T[0]), T[2] = T[2].replace(/^\+|\s*/g, ""); var U = /(-?)(\d*)(?:n([+\-]?\d*))?/.exec(T[2] === "even" && "2n" || T[2] === "odd" && "2n+1" || !/\D/.test(T[2]) && "0n+" + T[2] || T[2]); T[2] = U[1] + (U[2] || 1) - 0, T[3] = U[3] - 0 } else { T[2] && K.error(T[0]) } T[0] = Q++; return T }, ATTR: function (T, U, V, W, X, Y) { var Z = T[1] = T[1].replace(I, ""); !Y && L.attrMap[Z] && (T[1] = L.attrMap[Z]), T[4] = (T[4] || T[5] || "").replace(I, ""), T[2] === "~=" && (T[4] = " " + T[4] + " "); return T }, PSEUDO: function (T, U, V, W, X) { if (T[1] === "not") { if ((P.exec(T[3]) || "").length > 1 || /^\w/.test(T[3])) { T[3] = K(T[3], null, null, U) } else { var Y = K.filter(T[3], U, V, !0 ^ X); V || W.push.apply(W, Y); return !1 } } else { if (L.match.POS.test(T[0]) || L.match.CHILD.test(T[0])) { return !0 } } return T }, POS: function (T) { T.unshift(!0); return T } }, filters: { enabled: function (T) { return T.disabled === !1 && T.type !== "hidden" }, disabled: function (T) { return T.disabled === !0 }, checked: function (T) { return T.checked === !0 }, selected: function (T) { T.parentNode && T.parentNode.selectedIndex; return T.selected === !0 }, parent: function (T) { return !!T.firstChild }, empty: function (T) { return !T.firstChild }, has: function (T, U, V) { return !!K(V[3], T).length }, header: function (T) { return /h\d/i.test(T.nodeName) }, text: function (T) { return "text" === T.getAttribute("type") }, radio: function (T) { return "radio" === T.type }, checkbox: function (T) { return "checkbox" === T.type }, file: function (T) { return "file" === T.type }, password: function (T) { return "password" === T.type }, submit: function (T) { return "submit" === T.type }, image: function (T) { return "image" === T.type }, reset: function (T) { return "reset" === T.type }, button: function (T) { return "button" === T.type || T.nodeName.toLowerCase() === "button" }, input: function (T) { return /input|select|textarea|button/i.test(T.nodeName) } }, setFilters: { first: function (T, U) { return U === 0 }, last: function (T, U, V, W) { return U === W.length - 1 }, even: function (T, U) { return U % 2 === 0 }, odd: function (T, U) { return U % 2 === 1 }, lt: function (T, U, V) { return U < V[3] - 0 }, gt: function (T, U, V) { return U > V[3] - 0 }, nth: function (T, U, V) { return V[3] - 0 === U }, eq: function (T, U, V) { return V[3] - 0 === U } }, filter: { PSEUDO: function (U, V, W, X) { var Y = V[1], Z = L.filters[Y]; if (Z) { return Z(U, W, V, X) } if (Y === "contains") { return (U.textContent || U.innerText || K.getText([U]) || "").indexOf(V[3]) >= 0 } if (Y === "not") { var j = V[3]; for (var k = 0, T = j.length; k < T; k++) { if (j[k] === U) { return !1 } } return !0 } K.error(Y) }, CHILD: function (V, W) { var X = W[1], Y = V; switch (X) { case "only": case "first": while (Y = Y.previousSibling) { if (Y.nodeType === 1) { return !1 } } if (X === "first") { return !0 } Y = V; case "last": while (Y = Y.nextSibling) { if (Y.nodeType === 1) { return !1 } } return !0; case "nth": var Z = W[2], k = W[3]; if (Z === 1 && k === 0) { return !0 } var l = W[0], m = V.parentNode; if (m && (m.sizcache !== l || !V.nodeIndex)) { var T = 0; for (Y = m.firstChild; Y; Y = Y.nextSibling) { Y.nodeType === 1 && (Y.nodeIndex = ++T) } m.sizcache = l } var U = V.nodeIndex - k; return Z === 0 ? U === 0 : U % Z === 0 && U / Z >= 0 } }, ID: function (T, U) { return T.nodeType === 1 && T.getAttribute("id") === U }, TAG: function (T, U) { return U === "*" && T.nodeType === 1 || T.nodeName.toLowerCase() === U }, CLASS: function (T, U) { return (" " + (T.className || T.getAttribute("class")) + " ").indexOf(U) > -1 }, ATTR: function (T, U) { var V = U[1], W = L.attrHandle[V] ? L.attrHandle[V](T) : T[V] != null ? T[V] : T.getAttribute(V), X = W + "", Y = U[2], Z = U[4]; return W == null ? Y === "!=" : Y === "=" ? X === Z : Y === "*=" ? X.indexOf(Z) >= 0 : Y === "~=" ? (" " + X + " ").indexOf(Z) >= 0 : Z ? Y === "!=" ? X !== Z : Y === "^=" ? X.indexOf(Z) === 0 : Y === "$=" ? X.substr(X.length - Z.length) === Z : Y === "|=" ? X === Z || X.substr(0, Z.length + 1) === Z + "-" : !1 : X && W !== !1 }, POS: function (T, U, V, W) { var X = U[2], Y = L.setFilters[X]; if (Y) { return Y(T, V, U, W) } } } }, M = L.match.POS, N = function (T, U) { return "\\" + (U - 0 + 1) }; for (var O in L.match) { L.match[O] = new RegExp(L.match[O].source + /(?![^\[]*\])(?![^\(]*\))/.source), L.leftMatch[O] = new RegExp(/(^(?:.|\r|\n)*?)/.source + L.match[O].source.replace(/\\(\d+)/g, N)) } var A = function (T, U) { T = Array.prototype.slice.call(T, 0); if (U) { U.push.apply(U, T); return U } return T }; try { Array.prototype.slice.call(At.documentElement.childNodes, 0)[0].nodeType } catch (B) { A = function (T, U) { var V = 0, W = U || []; if (R.call(T) === "[object Array]") { Array.prototype.push.apply(W, T) } else { if (typeof T.length === "number") { for (var X = T.length; V < X; V++) { W.push(T[V]) } } else { for (; T[V]; V++) { W.push(T[V]) } } } return W } } var C, D; At.documentElement.compareDocumentPosition ? C = function (T, U) { if (T === U) { S = !0; return 0 } if (!T.compareDocumentPosition || !U.compareDocumentPosition) { return T.compareDocumentPosition ? -1 : 1 } return T.compareDocumentPosition(U) & 4 ? -1 : 1 } : (C = function (W, X) { var Y, Z, g = [], l = [], m = W.parentNode, T = X.parentNode, U = m; if (W === X) { S = !0; return 0 } if (m === T) { return D(W, X) } if (!m) { return -1 } if (!T) { return 1 } while (U) { g.unshift(U), U = U.parentNode } U = T; while (U) { l.unshift(U), U = U.parentNode } Y = g.length, Z = l.length; for (var V = 0; V < Y && V < Z; V++) { if (g[V] !== l[V]) { return D(g[V], l[V]) } } return V === Y ? D(W, l[V], -1) : D(g[V], X, 1) }, D = function (T, U, V) { if (T === U) { return V } var W = T.nextSibling; while (W) { if (W === U) { return -1 } W = W.nextSibling } return 1 }), K.getText = function (T) { var U = "", V; for (var W = 0; T[W]; W++) { V = T[W], V.nodeType === 3 || V.nodeType === 4 ? U += V.nodeValue : V.nodeType !== 8 && (U += K.getText(V.childNodes)) } return U }, function () { var T = At.createElement("div"), U = "script" + (new Date).getTime(), V = At.documentElement; T.innerHTML = "<a name='" + U + "'/>", V.insertBefore(T, V.firstChild), At.getElementById(U) && (L.find.ID = function (W, X, Y) { if (typeof X.getElementById !== "undefined" && !Y) { var Z = X.getElementById(W[1]); return Z ? Z.id === W[1] || typeof Z.getAttributeNode !== "undefined" && Z.getAttributeNode("id").nodeValue === W[1] ? [Z] : As : [] } }, L.filter.ID = function (W, X) { var Y = typeof W.getAttributeNode !== "undefined" && W.getAttributeNode("id"); return W.nodeType === 1 && Y && Y.nodeValue === X }), V.removeChild(T), V = T = null } (), function () { var T = At.createElement("div"); T.appendChild(At.createComment("")), T.getElementsByTagName("*").length > 0 && (L.find.TAG = function (U, V) { var W = V.getElementsByTagName(U[1]); if (U[1] === "*") { var X = []; for (var Y = 0; W[Y]; Y++) { W[Y].nodeType === 1 && X.push(W[Y]) } W = X } return W }), T.innerHTML = "<a href='#'></a>", T.firstChild && typeof T.firstChild.getAttribute !== "undefined" && T.firstChild.getAttribute("href") !== "#" && (L.attrHandle.href = function (U) { return U.getAttribute("href", 2) }), T = null } (), At.querySelectorAll && function () { var T = K, U = At.createElement("div"), V = "__sizzle__"; U.innerHTML = "<p class='TEST'></p>"; if (!U.querySelectorAll || U.querySelectorAll(".TEST").length !== 0) { K = function (d, k, l, p) { k = k || At; if (!p && !K.isXML(k)) { var w = /^(\w+$)|^\.([\w\-]+$)|^#([\w\-]+$)/.exec(d); if (w && (k.nodeType === 1 || k.nodeType === 9)) { if (w[1]) { return A(k.getElementsByTagName(d), l) } if (w[2] && L.find.CLASS && k.getElementsByClassName) { return A(k.getElementsByClassName(w[2]), l) } } if (k.nodeType === 9) { if (d === "body" && k.body) { return A([k.body], l) } if (w && w[3]) { var X = k.getElementById(w[3]); if (!X || !X.parentNode) { return A([], l) } if (X.id === w[3]) { return A([X], l) } } try { return A(k.querySelectorAll(d), l) } catch (Y) { } } else { if (k.nodeType === 1 && k.nodeName.toLowerCase() !== "object") { var Z = k, a = k.getAttribute("id"), c = a || V, t = k.parentNode, u = /^\s*[+~]/.test(d); a ? c = c.replace(/'/g, "\\$&") : k.setAttribute("id", c), u && t && (k = k.parentNode); try { if (!u || t) { return A(k.querySelectorAll("[id='" + c + "'] " + d), l) } } catch (v) { } finally { a || Z.removeAttribute("id") } } } } return T(d, k, l, p) }; for (var W in T) { K[W] = T[W] } U = null } } (), function () { var T = At.documentElement, U = T.matchesSelector || T.mozMatchesSelector || T.webkitMatchesSelector || T.msMatchesSelector, V = !1; try { U.call(At.documentElement, "[test!='']:sizzle") } catch (W) { V = !0 } U && (K.matchesSelector = function (X, Y) { Y = Y.replace(/\=\s*([^'"\]]*)\s*\]/g, "='$1']"); if (!K.isXML(X)) { try { if (V || !L.match.PSEUDO.test(Y) && !/!=/.test(Y)) { return U.call(X, Y) } } catch (Z) { } } return K(Y, null, null, [X]).length > 0 }) } (), function () { var T = At.createElement("div"); T.innerHTML = "<div class='test e'></div><div class='test'></div>"; if (T.getElementsByClassName && T.getElementsByClassName("e").length !== 0) { T.lastChild.className = "e"; if (T.getElementsByClassName("e").length === 1) { return } L.order.splice(1, 0, "CLASS"), L.find.CLASS = function (U, V, W) { if (typeof V.getElementsByClassName !== "undefined" && !W) { return V.getElementsByClassName(U[1]) } }, T = null } } (), At.documentElement.contains ? K.contains = function (T, U) { return T !== U && (T.contains ? T.contains(U) : !0) } : At.documentElement.compareDocumentPosition ? K.contains = function (T, U) { return !!(T.compareDocumentPosition(U) & 16) } : K.contains = function () { return !1 }, K.isXML = function (T) { var U = (T ? T.ownerDocument || T : 0).documentElement; return U ? U.nodeName !== "HTML" : !1 }; var G = function (T, U) { var V, W = [], X = "", Y = U.nodeType ? [U] : U; while (V = L.match.PSEUDO.exec(T)) { X += V[0], T = T.replace(L.match.PSEUDO, "") } T = L.relative[T] ? T + "*" : T; for (var Z = 0, i = Y.length; Z < i; Z++) { K(T, Y[Z], W) } return K.filter(X, W) }; Au.find = K, Au.expr = K.selectors, Au.expr[":"] = Au.expr.filters, Au.unique = K.uniqueSort, Au.text = K.getText, Au.isXMLDoc = K.isXML, Au.contains = K.contains } (); var BV = /Until$/, BG = /^(?:parents|prevUntil|prevAll)/, BH = /,/, BI = /^.[^:#\[\.,]*$/, BJ = Array.prototype.slice, BO = Au.expr.match.POS, BL = { children: !0, contents: !0, next: !0, prev: !0 }; Au.fn.extend({ find: function (B) { var C = this.pushStack("", "find", B), D = 0; for (var E = 0, F = this.length; E < F; E++) { D = C.length, Au.find(B, this[E], C); if (E > 0) { for (var G = D; G < C.length; G++) { for (var A = 0; A < D; A++) { if (C[A] === C[G]) { C.splice(G--, 1); break } } } } } return C }, has: function (A) { var B = Au(A); return this.filter(function () { for (var C = 0, D = B.length; C < D; C++) { if (Au.contains(this, B[C])) { return !0 } } }) }, not: function (A) { return this.pushStack(BN(this, A, !1), "not", A) }, filter: function (A) { return this.pushStack(BN(this, A, !0), "filter", A) }, is: function (A) { return !!A && Au.filter(A, this).length > 0 }, closest: function (E, F) { var G = [], H, I, J = this[0]; if (Au.isArray(E)) { var K, A, B = {}, C = 1; if (J && E.length) { for (H = 0, I = E.length; H < I; H++) { A = E[H], B[A] || (B[A] = Au.expr.match.POS.test(A) ? Au(A, F || this.context) : A) } while (J && J.ownerDocument && J !== F) { for (A in B) { K = B[A], (K.jquery ? K.index(J) > -1 : Au(J).is(K)) && G.push({ selector: A, elem: J, level: C }) } J = J.parentNode, C++ } } return G } var D = BO.test(E) ? Au(E, F || this.context) : null; for (H = 0, I = this.length; H < I; H++) { J = this[H]; while (J) { if (D ? D.index(J) > -1 : Au.find.matchesSelector(J, E)) { G.push(J); break } J = J.parentNode; if (!J || !J.ownerDocument || J === F) { break } } } G = G.length > 1 ? Au.unique(G) : G; return this.pushStack(G, "closest", E) }, index: function (A) { if (!A || typeof A === "string") { return Au.inArray(this[0], A ? Au(A) : this.parent().children()) } return Au.inArray(A.jquery ? A[0] : A, this) }, add: function (A, B) { var C = typeof A === "string" ? Au(A, B) : Au.makeArray(A), D = Au.merge(this.get(), C); return this.pushStack(BM(C[0]) || BM(D[0]) ? D : Au.unique(D)) }, andSelf: function () { return this.add(this.prevObject) } }), Au.each({ parent: function (A) { var B = A.parentNode; return B && B.nodeType !== 11 ? B : null }, parents: function (A) { return Au.dir(A, "parentNode") }, parentsUntil: function (A, B, C) { return Au.dir(A, "parentNode", C) }, next: function (A) { return Au.nth(A, 2, "nextSibling") }, prev: function (A) { return Au.nth(A, 2, "previousSibling") }, nextAll: function (A) { return Au.dir(A, "nextSibling") }, prevAll: function (A) { return Au.dir(A, "previousSibling") }, nextUntil: function (A, B, C) { return Au.dir(A, "nextSibling", C) }, prevUntil: function (A, B, C) { return Au.dir(A, "previousSibling", C) }, siblings: function (A) { return Au.sibling(A.parentNode.firstChild, A) }, children: function (A) { return Au.sibling(A.firstChild) }, contents: function (A) { return Au.nodeName(A, "iframe") ? A.contentDocument || A.contentWindow.document : Au.makeArray(A.childNodes) } }, function (A, B) { Au.fn[A] = function (C, D) { var E = Au.map(this, B, C), F = BJ.call(arguments); BV.test(A) || (D = C), D && typeof D === "string" && (E = Au.filter(D, E)), E = this.length > 1 && !BL[A] ? Au.unique(E) : E, (this.length > 1 || BH.test(D)) && BG.test(A) && (E = E.reverse()); return this.pushStack(E, A, F.join(",")) } }), Au.extend({ filter: function (A, B, C) { C && (A = ":not(" + A + ")"); return B.length === 1 ? Au.find.matchesSelector(B[0], A) ? [B[0]] : [] : Au.find.matches(A, B) }, dir: function (A, B, C) { var D = [], E = A[B]; while (E && E.nodeType !== 9 && (C === As || E.nodeType !== 1 || !Au(E).is(C))) { E.nodeType === 1 && D.push(E), E = E[B] } return D }, nth: function (A, B, C, D) { B = B || 1; var E = 0; for (; A; A = A[C]) { if (A.nodeType === 1 && ++E === B) { break } } return A }, sibling: function (A, B) { var C = []; for (; A; A = A.nextSibling) { A.nodeType === 1 && A !== B && C.push(A) } return C } }); var By = / jQuery\d+="(?:\d+|null)"/g, Bz = /^\s+/, B0 = /<(?!area|br|col|embed|hr|img|input|link|meta|param)(([\w:]+)[^>]*)\/>/ig, BB = /<([\w:]+)/, BC = /<tbody/i, BD = /<|&#?\w+;/, BE = /<(?:script|object|embed|option|style)/i, BF = /checked\s*(?:[^=]|=\s*.checked.)/i, Bq = { option: [1, "<select multiple='multiple'>", "</select>"], legend: [1, "<fieldset>", "</fieldset>"], thead: [1, "<table>", "</table>"], tr: [2, "<table><tbody>", "</tbody></table>"], td: [3, "<table><tbody><tr>", "</tr></tbody></table>"], col: [2, "<table><tbody></tbody><colgroup>", "</colgroup></table>"], area: [1, "<map>", "</map>"], _default: [0, "", ""] }; Bq.optgroup = Bq.option, Bq.tbody = Bq.tfoot = Bq.colgroup = Bq.caption = Bq.thead, Bq.th = Bq.td, Au.support.htmlSerialize || (Bq._default = [1, "div<div>", "</div>"]), Au.fn.extend({ text: function (A) { if (Au.isFunction(A)) { return this.each(function (B) { var C = Au(this); C.text(A.call(this, B, C.text())) }) } if (typeof A !== "object" && A !== As) { return this.empty().append((this[0] && this[0].ownerDocument || At).createTextNode(A)) } return Au.text(this) }, wrapAll: function (A) { if (Au.isFunction(A)) { return this.each(function (C) { Au(this).wrapAll(A.call(this, C)) }) } if (this[0]) { var B = Au(A, this[0].ownerDocument).eq(0).clone(!0); this[0].parentNode && B.insertBefore(this[0]), B.map(function () { var C = this; while (C.firstChild && C.firstChild.nodeType === 1) { C = C.firstChild } return C }).append(this) } return this }, wrapInner: function (A) { if (Au.isFunction(A)) { return this.each(function (B) { Au(this).wrapInner(A.call(this, B)) }) } return this.each(function () { var B = Au(this), C = B.contents(); C.length ? C.wrapAll(A) : B.append(A) }) }, wrap: function (A) { return this.each(function () { Au(this).wrapAll(A) }) }, unwrap: function () { return this.parent().each(function () { Au.nodeName(this, "body") || Au(this).replaceWith(this.childNodes) }).end() }, append: function () { return this.domManip(arguments, !0, function (A) { this.nodeType === 1 && this.appendChild(A) }) }, prepend: function () { return this.domManip(arguments, !0, function (A) { this.nodeType === 1 && this.insertBefore(A, this.firstChild) }) }, before: function () { if (this[0] && this[0].parentNode) { return this.domManip(arguments, !1, function (B) { this.parentNode.insertBefore(B, this) }) } if (arguments.length) { var A = Au(arguments[0]); A.push.apply(A, this.toArray()); return this.pushStack(A, "before", arguments) } }, after: function () { if (this[0] && this[0].parentNode) { return this.domManip(arguments, !1, function (B) { this.parentNode.insertBefore(B, this.nextSibling) }) } if (arguments.length) { var A = this.pushStack(this, "after", arguments); A.push.apply(A, Au(arguments[0]).toArray()); return A } }, remove: function (A, B) { for (var C = 0, D; (D = this[C]) != null; C++) { if (!A || Au.filter(A, [D]).length) { !B && D.nodeType === 1 && (Au.cleanData(D.getElementsByTagName("*")), Au.cleanData([D])), D.parentNode && D.parentNode.removeChild(D) } } return this }, empty: function () { for (var A = 0, B; (B = this[A]) != null; A++) { B.nodeType === 1 && Au.cleanData(B.getElementsByTagName("*")); while (B.firstChild) { B.removeChild(B.firstChild) } } return this }, clone: function (A, B) { A = A == null ? !1 : A, B = B == null ? A : B; return this.map(function () { return Au.clone(this, A, B) }) }, html: function (A) { if (A === As) { return this[0] && this[0].nodeType === 1 ? this[0].innerHTML.replace(By, "") : null } if (typeof A !== "string" || BE.test(A) || !Au.support.leadingWhitespace && Bz.test(A) || Bq[(BB.exec(A) || ["", ""])[1].toLowerCase()]) { Au.isFunction(A) ? this.each(function (E) { var F = Au(this); F.html(A.call(this, E, F.html())) }) : this.empty().append(A) } else { A = A.replace(B0, "<$1></$2>"); try { for (var B = 0, C = this.length; B < C; B++) { this[B].nodeType === 1 && (Au.cleanData(this[B].getElementsByTagName("*")), this[B].innerHTML = A) } } catch (D) { this.empty().append(A) } } return this }, replaceWith: function (A) { if (this[0] && this[0].parentNode) { if (Au.isFunction(A)) { return this.each(function (B) { var C = Au(this), D = C.html(); C.replaceWith(A.call(this, B, D)) }) } typeof A !== "string" && (A = Au(A).detach()); return this.each(function () { var B = this.nextSibling, C = this.parentNode; Au(this).remove(), B ? Au(B).before(A) : Au(C).append(A) }) } return this.pushStack(Au(Au.isFunction(A) ? A() : A), "replaceWith", A) }, detach: function (A) { return this.remove(A, !0) }, domManip: function (G, H, I) { var J, K, L, A, B = G[0], C = []; if (!Au.support.checkClone && arguments.length === 3 && typeof B === "string" && BF.test(B)) { return this.each(function () { Au(this).domManip(G, H, I, !0) }) } if (Au.isFunction(B)) { return this.each(function (M) { var N = Au(this); G[0] = B.call(this, M, H ? N.html() : As), N.domManip(G, H, I) }) } if (this[0]) { A = B && B.parentNode, Au.support.parentNode && A && A.nodeType === 11 && A.childNodes.length === this.length ? J = { fragment: A} : J = Au.buildFragment(G, this, C), L = J.fragment, L.childNodes.length === 1 ? K = L = L.firstChild : K = L.firstChild; if (K) { H = H && Au.nodeName(K, "tr"); for (var D = 0, E = this.length, F = E - 1; D < E; D++) { I.call(H ? Br(this[D], K) : this[D], J.cacheable || E > 1 && D < F ? Au.clone(L, !0, !0) : L) } } C.length && Au.each(C, A8) } return this } }), Au.buildFragment = function (A, B, E) { var F, G, C, D = B && B[0] ? B[0].ownerDocument || B[0] : At; A.length === 1 && typeof A[0] === "string" && A[0].length < 512 && D === At && A[0].charAt(0) === "<" && !BE.test(A[0]) && (Au.support.checkClone || !BF.test(A[0])) && (G = !0, C = Au.fragments[A[0]], C && (C !== 1 && (F = C))), F || (F = D.createDocumentFragment(), Au.clean(A, D, F, E)), G && (Au.fragments[A[0]] = C ? F : 1); return { fragment: F, cacheable: G} }, Au.fragments = {}, Au.each({ appendTo: "append", prependTo: "prepend", insertBefore: "before", insertAfter: "after", replaceAll: "replaceWith" }, function (A, B) { Au.fn[A] = function (E) { var G = [], H = Au(E), I = this.length === 1 && this[0].parentNode; if (I && I.nodeType === 11 && I.childNodes.length === 1 && H.length === 1) { H[B](this[0]); return this } for (var C = 0, F = H.length; C < F; C++) { var D = (C > 0 ? this.clone(!0) : this).get(); Au(H[C])[B](D), G = G.concat(D) } return this.pushStack(G, A, H.selector) } }), Au.extend({ clone: function (B, C, D) { var E = B.cloneNode(!0), F, G, A; if ((!Au.support.noCloneEvent || !Au.support.noCloneChecked) && (B.nodeType === 1 || B.nodeType === 11) && !Au.isXMLDoc(B)) { AR(B, E), F = Bx(B), G = Bx(E); for (A = 0; F[A]; ++A) { AR(F[A], G[A]) } } if (C) { Bs(B, E); if (D) { F = Bx(B), G = Bx(E); for (A = 0; F[A]; ++A) { Bs(F[A], G[A]) } } } return E }, clean: function (H, I, J, K) { I = I || At, typeof I.createElement === "undefined" && (I = I.ownerDocument || I[0] && I[0].ownerDocument || At); var L = []; for (var N = 0, A; (A = H[N]) != null; N++) { typeof A === "number" && (A += ""); if (!A) { continue } if (typeof A !== "string" || BD.test(A)) { if (typeof A === "string") { A = A.replace(B0, "<$1></$2>"); var B = (BB.exec(A) || ["", ""])[1].toLowerCase(), C = Bq[B] || Bq._default, D = C[0], E = I.createElement("div"); E.innerHTML = C[1] + A + C[2]; while (D--) { E = E.lastChild } if (!Au.support.tbody) { var F = BC.test(A), G = B === "table" && !F ? E.firstChild && E.firstChild.childNodes : C[1] === "<table>" && !F ? E.childNodes : []; for (var M = G.length - 1; M >= 0; --M) { Au.nodeName(G[M], "tbody") && !G[M].childNodes.length && G[M].parentNode.removeChild(G[M]) } } !Au.support.leadingWhitespace && Bz.test(A) && E.insertBefore(I.createTextNode(Bz.exec(A)[0]), E.firstChild), A = E.childNodes } } else { A = I.createTextNode(A) } A.nodeType ? L.push(A) : L = Au.merge(L, A) } if (J) { for (N = 0; L[N]; N++) { !K || !Au.nodeName(L[N], "script") || L[N].type && L[N].type.toLowerCase() !== "text/javascript" ? (L[N].nodeType === 1 && L.splice.apply(L, [N + 1, 0].concat(Au.makeArray(L[N].getElementsByTagName("script")))), J.appendChild(L[N])) : K.push(L[N].parentNode ? L[N].parentNode.removeChild(L[N]) : L[N]) } } return L }, cleanData: function (D) { var E, F, G = Au.cache, H = Au.expando, I = Au.event.special, J = Au.support.deleteExpando; for (var A = 0, B; (B = D[A]) != null; A++) { if (B.nodeName && Au.noData[B.nodeName.toLowerCase()]) { continue } F = B[Au.expando]; if (F) { E = G[F] && G[F][H]; if (E && E.events) { for (var C in E.events) { I[C] ? Au.event.remove(B, C) : Au.removeEvent(B, C, E.handle) } E.handle && (E.handle.elem = null) } J ? delete B[Au.expando] : B.removeAttribute && B.removeAttribute(Au.expando), delete G[F] } } } }); var Bo = /alpha\([^)]*\)/i, Ac = /opacity=([^)]*)/, AH = /-([a-z])/ig, AW = /([A-Z])/g, Bb = /^-?\d+(?:px)?$/i, Bt = /^-?\d/, Bf = { position: "absolute", visibility: "hidden", display: "block" }, BK = ["Left", "Right"], AB = ["Top", "Bottom"], AP, A5, Bi, AV = function (A, B) { return B.toUpperCase() }; Au.fn.css = function (A, B) { if (arguments.length === 2 && B === As) { return this } return Au.access(this, A, B, !0, function (C, D, E) { return E !== As ? Au.style(C, D, E) : Au.css(C, D) }) }, Au.extend({ cssHooks: { opacity: { get: function (A, B) { if (B) { var C = AP(A, "opacity", "opacity"); return C === "" ? "1" : C } return A.style.opacity } } }, cssNumber: { zIndex: !0, fontWeight: !0, opacity: !0, zoom: !0, lineHeight: !0 }, cssProps: { "float": Au.support.cssFloat ? "cssFloat" : "styleFloat" }, style: function (D, E, F, G) { if (D && D.nodeType !== 3 && D.nodeType !== 8 && D.style) { var H, I = Au.camelCase(E), A = D.style, B = Au.cssHooks[I]; E = Au.cssProps[I] || I; if (F === As) { if (B && "get" in B && (H = B.get(D, !1, G)) !== As) { return H } return A[E] } if (typeof F === "number" && isNaN(F) || F == null) { return } typeof F === "number" && !Au.cssNumber[I] && (F += "px"); if (!B || !("set" in B) || (F = B.set(D, F)) !== As) { try { A[E] = F } catch (C) { } } } }, css: function (B, C, D) { var E, F = Au.camelCase(C), A = Au.cssHooks[F]; C = Au.cssProps[F] || F; if (A && "get" in A && (E = A.get(B, !0, D)) !== As) { return E } if (AP) { return AP(B, C, F) } }, swap: function (A, B, C) { var D = {}; for (var E in B) { D[E] = A.style[E], A.style[E] = B[E] } C.call(A); for (E in B) { A.style[E] = D[E] } }, camelCase: function (A) { return A.replace(AH, AV) } }), Au.curCSS = Au.css, Au.each(["height", "width"], function (A, B) { Au.cssHooks[B] = { get: function (C, D, E) { var F; if (D) { C.offsetWidth !== 0 ? F = AE(C, B, E) : Au.swap(C, Bf, function () { F = AE(C, B, E) }); if (F <= 0) { F = AP(C, B, B), F === "0px" && Bi && (F = Bi(C, B, B)); if (F != null) { return F === "" || F === "auto" ? "0px" : F } } if (F < 0 || F == null) { F = C.style[B]; return F === "" || F === "auto" ? "0px" : F } return typeof F === "string" ? F : F + "px" } }, set: function (C, D) { if (!Bb.test(D)) { return D } D = parseFloat(D); if (D >= 0) { return D + "px" } } } }), Au.support.opacity || (Au.cssHooks.opacity = { get: function (A, B) { return Ac.test((B && A.currentStyle ? A.currentStyle.filter : A.style.filter) || "") ? parseFloat(RegExp.$1) / 100 + "" : B ? "1" : "" }, set: function (A, B) { var C = A.style; C.zoom = 1; var D = Au.isNaN(B) ? "" : "alpha(opacity=" + B * 100 + ")", E = C.filter || ""; C.filter = Bo.test(E) ? E.replace(Bo, D) : C.filter + " " + D } }), At.defaultView && At.defaultView.getComputedStyle && (A5 = function (B, C, D) { var E, F, A; D = D.replace(AW, "-$1").toLowerCase(); if (!(F = B.ownerDocument.defaultView)) { return As } if (A = F.getComputedStyle(B, null)) { E = A.getPropertyValue(D), E === "" && !Au.contains(B.ownerDocument.documentElement, B) && (E = Au.style(B, D)) } return E }), At.documentElement.currentStyle && (Bi = function (A, B) { var C, D = A.currentStyle && A.currentStyle[B], E = A.runtimeStyle && A.runtimeStyle[B], F = A.style; !Bb.test(D) && Bt.test(D) && (C = F.left, E && (A.runtimeStyle.left = A.currentStyle.left), F.left = B === "fontSize" ? "1em" : D || 0, D = F.pixelLeft + "px", F.left = C, E && (A.runtimeStyle.left = E)); return D === "" ? "auto" : D }), AP = A5 || Bi, Au.expr && Au.expr.filters && (Au.expr.filters.hidden = function (A) { var B = A.offsetWidth, C = A.offsetHeight; return B === 0 && C === 0 || !Au.support.reliableHiddenOffsets && (A.style.display || Au.css(A, "display")) === "none" }, Au.expr.filters.visible = function (A) { return !Au.expr.filters.hidden(A) }); var Bw = /%20/g, AJ = /\[\]$/, AY = /\r?\n/g, Bd = /#.*$/, AK = /^(.*?):[ \t]*([^\r\n]*)\r?$/mg, Aq = /^(?:color|date|datetime|email|hidden|month|number|password|range|search|tel|text|time|url|week)$/i, AN = /(?:^file|^widget|\-extension):$/, A2 = /^(?:GET|HEAD)$/, AS = /^\/\//, A7 = /\?/, Bk = /<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi, Bn = /^(?:select|textarea)/i, Ay = /\s+/, AO = /([?&])_=[^&]*/, A3 = /(^|\-)([a-z])/g, Bh = function (A, B, C) { return B + C.toUpperCase() }, Bl = /^([\w\+\.\-]+:)\/\/([^\/?#:]*)(?::(\d+))?/, AD = Au.fn.load, AZ = {}, AM = {}, AX, Bc; try { AX = At.location.href } catch (Bg) { AX = At.createElement("a"), AX.href = "", AX = AX.href } Bc = Bl.exec(AX.toLowerCase()), Au.fn.extend({ load: function (B, C, E) { if (typeof B !== "string" && AD) { return AD.apply(this, arguments) } if (!this.length) { return this } var F = B.indexOf(" "); if (F >= 0) { var G = B.slice(F, B.length); B = B.slice(0, F) } var A = "GET"; C && (Au.isFunction(C) ? (E = C, C = As) : typeof C === "object" && (C = Au.param(C, Au.ajaxSettings.traditional), A = "POST")); var D = this; Au.ajax({ url: B, type: A, dataType: "html", data: C, complete: function (H, I, J) { J = H.responseText, H.isResolved() && (H.done(function (K) { J = K }), D.html(G ? Au("<div>").append(J.replace(Bk, "")).find(G) : J)), E && D.each(E, [J, I, H]) } }); return this }, serialize: function () { return Au.param(this.serializeArray()) }, serializeArray: function () { return this.map(function () { return this.elements ? Au.makeArray(this.elements) : this }).filter(function () { return this.name && !this.disabled && (this.checked || Bn.test(this.nodeName) || Aq.test(this.type)) }).map(function (A, B) { var C = Au(this).val(); return C == null ? null : Au.isArray(C) ? Au.map(C, function (D, E) { return { name: B.name, value: D.replace(AY, "\r\n")} }) : { name: B.name, value: C.replace(AY, "\r\n")} }).get() } }), Au.each("ajaxStart ajaxStop ajaxComplete ajaxError ajaxSuccess ajaxSend".split(" "), function (A, B) { Au.fn[B] = function (C) { return this.bind(B, C) } }), Au.each(["get", "post"], function (A, B) { Au[B] = function (C, D, E, F) { Au.isFunction(D) && (F = F || E, E = D, D = As); return Au.ajax({ type: B, url: C, data: D, success: E, dataType: F }) } }), Au.extend({ getScript: function (A, B) { return Au.get(A, As, B, "script") }, getJSON: function (A, B, C) { return Au.get(A, B, C, "json") }, ajaxSetup: function (A, B) { B ? Au.extend(!0, A, Au.ajaxSettings, B) : (B = A, A = Au.extend(!0, Au.ajaxSettings, B)); for (var C in { context: 1, url: 1 }) { C in B ? A[C] = B[C] : C in Au.ajaxSettings && (A[C] = Au.ajaxSettings[C]) } return A }, ajaxSettings: { url: AX, isLocal: AN.test(Bc[1]), global: !0, type: "GET", contentType: "application/x-www-form-urlencoded", processData: !0, async: !0, accepts: { xml: "application/xml, text/xml", html: "text/html", text: "text/plain", json: "application/json, text/javascript", "*": "*/*" }, contents: { xml: /xml/, html: /html/, json: /json/ }, responseFields: { xml: "responseXML", text: "responseText" }, converters: { "* text": Ar.String, "text html": !0, "text json": Au.parseJSON, "text xml": Au.parseXML} }, ajaxPrefilter: Ao(AZ), ajaxTransport: Ao(AM), ajax: function (R, S) { function H(e, f, Y, Z) { if (D !== 2) { D = 2, B && clearTimeout(B), Q = As, O = Z || "", G.readyState = e ? 4 : 0; var g, h, i, j = Y ? AQ(T, G, Y) : As, b, d; if (e >= 200 && e < 300 || e === 304) { if (T.ifModified) { if (b = G.getResponseHeader("Last-Modified")) { Au.lastModified[M] = b } if (d = G.getResponseHeader("Etag")) { Au.etag[M] = d } } if (e === 304) { f = "notmodified", g = !0 } else { try { h = A6(T, j), f = "success", g = !0 } catch (X) { f = "parsererror", i = X } } } else { i = f; if (!f || e) { f = "error", e < 0 && (e = 0) } } G.status = e, G.statusText = f, g ? J.resolveWith(U, [h, f, G]) : J.rejectWith(U, [G, f, i]), G.statusCode(L), L = As, E && V.trigger("ajax" + (g ? "Success" : "Error"), [G, T, g ? h : i]), K.resolveWith(U, [G, f]), E && (V.trigger("ajaxComplete", [G, T]), --Au.active || Au.event.trigger("ajaxStop")) } } typeof R === "object" && (S = R, R = As), S = S || {}; var T = Au.ajaxSetup({}, S), U = T.context || T, V = U !== T && (U.nodeType || U instanceof Au) ? Au(U) : Au.event, J = Au.Deferred(), K = Au._Deferred(), L = T.statusCode || {}, M, N = {}, O, P, Q, B, C, D = 0, E, F, G = { readyState: 0, setRequestHeader: function (X, Y) { D || (N[X.toLowerCase().replace(A3, Bh)] = Y); return this }, getAllResponseHeaders: function () { return D === 2 ? O : null }, getResponseHeader: function (X) { var Y; if (D === 2) { if (!P) { P = {}; while (Y = AK.exec(O)) { P[Y[1].toLowerCase()] = Y[2] } } Y = P[X.toLowerCase()] } return Y === As ? null : Y }, overrideMimeType: function (X) { D || (T.mimeType = X); return this }, abort: function (X) { X = X || "abort", Q && Q.abort(X), H(0, X); return this } }; J.promise(G), G.success = G.done, G.error = G.fail, G.complete = K.done, G.statusCode = function (X) { if (X) { var Y; if (D < 2) { for (Y in X) { L[Y] = [L[Y], X[Y]] } } else { Y = X[G.status], G.then(Y, Y) } } return this }, T.url = ((R || T.url) + "").replace(Bd, "").replace(AS, Bc[1] + "//"), T.dataTypes = Au.trim(T.dataType || "*").toLowerCase().split(Ay), T.crossDomain || (C = Bl.exec(T.url.toLowerCase()), T.crossDomain = C && (C[1] != Bc[1] || C[2] != Bc[2] || (C[3] || (C[1] === "http:" ? 80 : 443)) != (Bc[3] || (Bc[1] === "http:" ? 80 : 443)))), T.data && T.processData && typeof T.data !== "string" && (T.data = Au.param(T.data, T.traditional)), AL(AZ, T, S, G); if (D === 2) { return !1 } E = T.global, T.type = T.type.toUpperCase(), T.hasContent = !A2.test(T.type), E && Au.active++ === 0 && Au.event.trigger("ajaxStart"); if (!T.hasContent) { T.data && (T.url += (A7.test(T.url) ? "&" : "?") + T.data), M = T.url; if (T.cache === !1) { var I = Au.now(), W = T.url.replace(AO, "$1_=" + I); T.url = W + (W === T.url ? (A7.test(T.url) ? "&" : "?") + "_=" + I : "") } } if (T.data && T.hasContent && T.contentType !== !1 || S.contentType) { N["Content-Type"] = T.contentType } T.ifModified && (M = M || T.url, Au.lastModified[M] && (N["If-Modified-Since"] = Au.lastModified[M]), Au.etag[M] && (N["If-None-Match"] = Au.etag[M])), N.Accept = T.dataTypes[0] && T.accepts[T.dataTypes[0]] ? T.accepts[T.dataTypes[0]] + (T.dataTypes[0] !== "*" ? ", */*; q=0.01" : "") : T.accepts["*"]; for (F in T.headers) { G.setRequestHeader(F, T.headers[F]) } if (T.beforeSend && (T.beforeSend.call(U, G, T) === !1 || D === 2)) { G.abort(); return !1 } for (F in { success: 1, error: 1, complete: 1 }) { G[F](T[F]) } Q = AL(AM, T, S, G); if (Q) { G.readyState = 1, E && V.trigger("ajaxSend", [G, T]), T.async && T.timeout > 0 && (B = setTimeout(function () { G.abort("timeout") }, T.timeout)); try { D = 1, Q.send(N, H) } catch (A) { status < 2 ? H(-1, A) : Au.error(A) } } else { H(-1, "No Transport") } return G }, param: function (A, B) { var C = [], D = function (F, G) { G = Au.isFunction(G) ? G() : G, C[C.length] = encodeURIComponent(F) + "=" + encodeURIComponent(G) }; B === As && (B = Au.ajaxSettings.traditional); if (Au.isArray(A) || A.jquery && !Au.isPlainObject(A)) { Au.each(A, function () { D(this.name, this.value) }) } else { for (var E in A) { A0(E, A[E], B, D) } } return C.join("&").replace(Bw, "+") } }), Au.extend({ active: 0, lastModified: {}, etag: {} }); var Bj = Au.now(), AU = /(\=)\?(&|$)|()\?\?()/i; Au.ajaxSetup({ jsonp: "callback", jsonpCallback: function () { return Au.expando + "_" + Bj++ } }), Au.ajaxPrefilter("json jsonp", function (F, G, H) { var I = typeof F.data === "string"; if (F.dataTypes[0] === "jsonp" || G.jsonpCallback || G.jsonp != null || F.jsonp !== !1 && (AU.test(F.url) || I && AU.test(F.data))) { var J, K = F.jsonpCallback = Au.isFunction(F.jsonpCallback) ? F.jsonpCallback() : F.jsonpCallback, A = Ar[K], B = F.url, C = F.data, D = "$1" + K + "$2", E = function () { Ar[K] = A, J && Au.isFunction(A) && Ar[K](J[0]) }; F.jsonp !== !1 && (B = B.replace(AU, D), F.url === B && (I && (C = C.replace(AU, D)), F.data === C && (B += (/\?/.test(B) ? "&" : "?") + F.jsonp + "=" + K))), F.url = B, F.data = C, Ar[K] = function (L) { J = [L] }, H.then(E, E), F.converters["script json"] = function () { J || Au.error(K + " was not called"); return J[0] }, F.dataTypes[0] = "json"; return "script" } }), Au.ajaxSetup({ accepts: { script: "text/javascript, application/javascript, application/ecmascript, application/x-ecmascript" }, contents: { script: /javascript|ecmascript/ }, converters: { "text script": function (A) { Au.globalEval(A); return A } } }), Au.ajaxPrefilter("script", function (A) { A.cache === As && (A.cache = !1), A.crossDomain && (A.type = "GET", A.global = !1) }), Au.ajaxTransport("script", function (A) { if (A.crossDomain) { var B, C = At.head || At.getElementsByTagName("head")[0] || At.documentElement; return { send: function (D, E) { B = At.createElement("script"), B.async = "async", A.scriptCharset && (B.charset = A.scriptCharset), B.src = A.url, B.onload = B.onreadystatechange = function (F, G) { if (!B.readyState || /loaded|complete/.test(B.readyState)) { B.onload = B.onreadystatechange = null, C && B.parentNode && C.removeChild(B), B = As, G || E(200, "success") } }, C.insertBefore(B, C.firstChild) }, abort: function () { B && B.onload(0, 1) } } } }); var AF = Au.now(), AT, A9; Au.ajaxSettings.xhr = Ar.ActiveXObject ? function () { return !this.isLocal && Be() || BA() } : Be, A9 = Au.ajaxSettings.xhr(), Au.support.ajax = !!A9, Au.support.cors = A9 && "withCredentials" in A9, A9 = As, Au.support.ajax && Au.ajaxTransport(function (A) { if (!A.crossDomain || Au.support.cors) { var B; return { send: function (F, G) { var H = A.xhr(), D, E; A.username ? H.open(A.type, A.url, A.async, A.username, A.password) : H.open(A.type, A.url, A.async); if (A.xhrFields) { for (E in A.xhrFields) { H[E] = A.xhrFields[E] } } A.mimeType && H.overrideMimeType && H.overrideMimeType(A.mimeType), (!A.crossDomain || A.hasContent) && !F["X-Requested-With"] && (F["X-Requested-With"] = "XMLHttpRequest"); try { for (E in F) { H.setRequestHeader(E, F[E]) } } catch (C) { } H.send(A.hasContent && A.data || null), B = function (P, I) { var J, K, L, M, N; try { if (B && (I || H.readyState === 4)) { B = As, D && (H.onreadystatechange = Au.noop, delete AT[D]); if (I) { H.readyState !== 4 && H.abort() } else { J = H.status, L = H.getAllResponseHeaders(), M = {}, N = H.responseXML, N && N.documentElement && (M.xml = N), M.text = H.responseText; try { K = H.statusText } catch (O) { K = "" } J || !A.isLocal || A.crossDomain ? J === 1223 && (J = 204) : J = M.text ? 200 : 404 } } } catch (Q) { I || G(-1, Q) } M && G(J, K, M, L) }, A.async && H.readyState !== 4 ? (AT || (AT = {}, Bp()), D = AF++, H.onreadystatechange = AT[D] = B) : B() }, abort: function () { B && B(0, 1) } } } }); var BZ = {}, Ba = /^(?:toggle|show|hide)$/, AC = /^([+\-]=)?([\d+.\-]+)([a-z%]*)$/i, Bv, A4 = [["height", "marginTop", "marginBottom", "paddingTop", "paddingBottom"], ["width", "marginLeft", "marginRight", "paddingLeft", "paddingRight"], ["opacity"]]; Au.fn.extend({ show: function (B, C, D) { var E, F; if (B || B === 0) { return this.animate(A1("show", 3), B, C, D) } for (var G = 0, A = this.length; G < A; G++) { E = this[G], F = E.style.display, !Au._data(E, "olddisplay") && F === "none" && (F = E.style.display = ""), F === "" && Au.css(E, "display") === "none" && Au._data(E, "olddisplay", AI(E.nodeName)) } for (G = 0; G < A; G++) { E = this[G], F = E.style.display; if (F === "" || F === "none") { E.style.display = Au._data(E, "olddisplay") || "" } } return this }, hide: function (A, B, C) { if (A || A === 0) { return this.animate(A1("hide", 3), A, B, C) } for (var D = 0, E = this.length; D < E; D++) { var F = Au.css(this[D], "display"); F !== "none" && !Au._data(this[D], "olddisplay") && Au._data(this[D], "olddisplay", F) } for (D = 0; D < E; D++) { this[D].style.display = "none" } return this }, _toggle: Au.fn.toggle, toggle: function (A, B, C) { var D = typeof A === "boolean"; Au.isFunction(A) && Au.isFunction(B) ? this._toggle.apply(this, arguments) : A == null || D ? this.each(function () { var E = D ? A : Au(this).is(":hidden"); Au(this)[E ? "show" : "hide"]() }) : this.animate(A1("toggle", 3), A, B, C); return this }, fadeTo: function (A, B, C, D) { return this.filter(":hidden").css("opacity", 0).show().end().animate({ opacity: B }, A, C, D) }, animate: function (A, B, C, D) { var E = Au.speed(B, C, D); if (Au.isEmptyObject(A)) { return this.each(E.complete) } return this[E.queue === !1 ? "each" : "queue"](function () { var H = Au.extend({}, E), I, K = this.nodeType === 1, L = K && Au(this).is(":hidden"), F = this; for (I in A) { var J = Au.camelCase(I); I !== J && (A[J] = A[I], delete A[I], I = J); if (A[I] === "hide" && L || A[I] === "show" && !L) { return H.complete.call(this) } if (K && (I === "height" || I === "width")) { H.overflow = [this.style.overflow, this.style.overflowX, this.style.overflowY]; if (Au.css(this, "display") === "inline" && Au.css(this, "float") === "none") { if (Au.support.inlineBlockNeedsLayout) { var G = AI(this.nodeName); G === "inline" ? this.style.display = "inline-block" : (this.style.display = "inline", this.style.zoom = 1) } else { this.style.display = "inline-block" } } } Au.isArray(A[I]) && ((H.specialEasing = H.specialEasing || {})[I] = A[I][1], A[I] = A[I][0]) } H.overflow != null && (this.style.overflow = "hidden"), H.curAnim = Au.extend({}, A), Au.each(A, function (O, Q) { var R = new Au.fx(F, H, O); if (Ba.test(Q)) { R[Q === "toggle" ? L ? "show" : "hide" : Q](A) } else { var P = AC.exec(Q), M = R.cur(); if (P) { var N = parseFloat(P[2]), S = P[3] || (Au.cssNumber[O] ? "" : "px"); S !== "px" && (Au.style(F, O, (N || 1) + S), M = (N || 1) / R.cur() * M, Au.style(F, O, M + S)), P[1] && (N = (P[1] === "-=" ? -1 : 1) * N + M), R.custom(M, N, S) } else { R.custom(M, Q, "") } } }); return !0 }) }, stop: function (A, B) { var C = Au.timers; A && this.queue([]), this.each(function () { for (var D = C.length - 1; D >= 0; D--) { C[D].elem === this && (B && C[D](!0), C.splice(D, 1)) } }), B || this.dequeue(); return this } }), Au.each({ slideDown: A1("show", 1), slideUp: A1("hide", 1), slideToggle: A1("toggle", 1), fadeIn: { opacity: "show" }, fadeOut: { opacity: "hide" }, fadeToggle: { opacity: "toggle"} }, function (A, B) { Au.fn[A] = function (C, D, E) { return this.animate(B, C, D, E) } }), Au.extend({ speed: function (A, B, C) { var D = A && typeof A === "object" ? Au.extend({}, A) : { complete: C || !C && B || Au.isFunction(A) && A, duration: A, easing: C && B || B && !Au.isFunction(B) && B }; D.duration = Au.fx.off ? 0 : typeof D.duration === "number" ? D.duration : D.duration in Au.fx.speeds ? Au.fx.speeds[D.duration] : Au.fx.speeds._default, D.old = D.complete, D.complete = function () { D.queue !== !1 && Au(this).dequeue(), Au.isFunction(D.old) && D.old.call(this) }; return D }, easing: { linear: function (A, B, C, D) { return C + D * A }, swing: function (A, B, C, D) { return (-Math.cos(A * Math.PI) / 2 + 0.5) * D + C } }, timers: [], fx: function (A, B, C) { this.options = B, this.elem = A, this.prop = C, B.orig || (B.orig = {}) } }), Au.fx.prototype = { update: function () { this.options.step && this.options.step.call(this.elem, this.now, this), (Au.fx.step[this.prop] || Au.fx.step._default)(this) }, cur: function () { if (this.elem[this.prop] != null && (!this.elem.style || this.elem.style[this.prop] == null)) { return this.elem[this.prop] } var A, B = Au.css(this.elem, this.prop); return isNaN(A = parseFloat(B)) ? !B || B === "auto" ? 0 : B : A }, custom: function (A, B, C) { function F(G) { return D.step(G) } var D = this, E = Au.fx; this.startTime = Au.now(), this.start = A, this.end = B, this.unit = C || this.unit || (Au.cssNumber[this.prop] ? "" : "px"), this.now = this.start, this.pos = this.state = 0, F.elem = this.elem, F() && Au.timers.push(F) && !Bv && (Bv = setInterval(E.tick, E.interval)) }, show: function () { this.options.orig[this.prop] = Au.style(this.elem, this.prop), this.options.show = !0, this.custom(this.prop === "width" || this.prop === "height" ? 1 : 0, this.cur()), Au(this.elem).show() }, hide: function () { this.options.orig[this.prop] = Au.style(this.elem, this.prop), this.options.hide = !0, this.custom(this.cur(), 0) }, step: function (D) { var E = Au.now(), F = !0; if (D || E >= this.options.duration + this.startTime) { this.now = this.end, this.pos = this.state = 1, this.update(), this.options.curAnim[this.prop] = !0; for (var G in this.options.curAnim) { this.options.curAnim[G] !== !0 && (F = !1) } if (F) { if (this.options.overflow != null && !Au.support.shrinkWrapBlocks) { var H = this.elem, I = this.options; Au.each(["", "X", "Y"], function (K, L) { H.style["overflow" + L] = I.overflow[K] }) } this.options.hide && Au(this.elem).hide(); if (this.options.hide || this.options.show) { for (var J in this.options.curAnim) { Au.style(this.elem, J, this.options.orig[J]) } } this.options.complete.call(this.elem) } return !1 } var A = E - this.startTime; this.state = A / this.options.duration; var B = this.options.specialEasing && this.options.specialEasing[this.prop], C = this.options.easing || (Au.easing.swing ? "swing" : "linear"); this.pos = Au.easing[B || C](this.state, A, 0, 1, this.options.duration), this.now = this.start + (this.end - this.start) * this.pos, this.update(); return !0 } }, Au.extend(Au.fx, { tick: function () { var A = Au.timers; for (var B = 0; B < A.length; B++) { A[B]() || A.splice(B--, 1) } A.length || Au.fx.stop() }, interval: 13, stop: function () { clearInterval(Bv), Bv = null }, speeds: { slow: 600, fast: 200, _default: 400 }, step: { opacity: function (A) { Au.style(A.elem, "opacity", A.now) }, _default: function (A) { A.elem.style && A.elem.style[A.prop] != null ? A.elem.style[A.prop] = (A.prop === "width" || A.prop === "height" ? Math.max(0, A.now) : A.now) + A.unit : A.elem[A.prop] = A.now } } }), Au.expr && Au.expr.filters && (Au.expr.filters.animated = function (A) { return Au.grep(Au.timers, function (B) { return A === B.elem }).length }); var Bm = /^t(?:able|d|h)$/i, AG = /^(?:body|html)$/i; "getBoundingClientRect" in At.documentElement ? Au.fn.offset = function (H) { var I = this[0], J; if (H) { return this.each(function (O) { Au.offset.setOffset(this, H, O) }) } if (!I || !I.ownerDocument) { return null } if (I === I.ownerDocument.body) { return Au.offset.bodyOffset(I) } try { J = I.getBoundingClientRect() } catch (K) { } var L = I.ownerDocument, M = L.documentElement; if (!J || !Au.contains(M, I)) { return J ? { top: J.top, left: J.left} : { top: 0, left: 0} } var N = L.body, A = Bu(L), B = M.clientTop || N.clientTop || 0, C = M.clientLeft || N.clientLeft || 0, D = A.pageYOffset || Au.support.boxModel && M.scrollTop || N.scrollTop, E = A.pageXOffset || Au.support.boxModel && M.scrollLeft || N.scrollLeft, F = J.top + D - B, G = J.left + E - C; return { top: F, left: G} } : Au.fn.offset = function (F) { var G = this[0]; if (F) { return this.each(function (M) { Au.offset.setOffset(this, F, M) }) } if (!G || !G.ownerDocument) { return null } if (G === G.ownerDocument.body) { return Au.offset.bodyOffset(G) } Au.offset.initialize(); var H, I = G.offsetParent, J = G, K = G.ownerDocument, L = K.documentElement, A = K.body, B = K.defaultView, C = B ? B.getComputedStyle(G, null) : G.currentStyle, D = G.offsetTop, E = G.offsetLeft; while ((G = G.parentNode) && G !== A && G !== L) { if (Au.offset.supportsFixedPosition && C.position === "fixed") { break } H = B ? B.getComputedStyle(G, null) : G.currentStyle, D -= G.scrollTop, E -= G.scrollLeft, G === I && (D += G.offsetTop, E += G.offsetLeft, Au.offset.doesNotAddBorder && (!Au.offset.doesAddBorderForTableAndCells || !Bm.test(G.nodeName)) && (D += parseFloat(H.borderTopWidth) || 0, E += parseFloat(H.borderLeftWidth) || 0), J = I, I = G.offsetParent), Au.offset.subtractsBorderForOverflowNotVisible && H.overflow !== "visible" && (D += parseFloat(H.borderTopWidth) || 0, E += parseFloat(H.borderLeftWidth) || 0), C = H } if (C.position === "relative" || C.position === "static") { D += A.offsetTop, E += A.offsetLeft } Au.offset.supportsFixedPosition && C.position === "fixed" && (D += Math.max(L.scrollTop, A.scrollTop), E += Math.max(L.scrollLeft, A.scrollLeft)); return { top: D, left: E} }, Au.offset = { initialize: function () { var C = At.body, D = At.createElement("div"), E, F, G, H, A = parseFloat(Au.css(C, "marginTop")) || 0, B = "<div style='position:absolute;top:0;left:0;margin:0;border:5px solid #000;padding:0;width:1px;height:1px;'><div></div></div><table style='position:absolute;top:0;left:0;margin:0;border:5px solid #000;padding:0;width:1px;height:1px;' cellpadding='0' cellspacing='0'><tr><td></td></tr></table>"; Au.extend(D.style, { position: "absolute", top: 0, left: 0, margin: 0, border: 0, width: "1px", height: "1px", visibility: "hidden" }), D.innerHTML = B, C.insertBefore(D, C.firstChild), E = D.firstChild, F = E.firstChild, H = E.nextSibling.firstChild.firstChild, this.doesNotAddBorder = F.offsetTop !== 5, this.doesAddBorderForTableAndCells = H.offsetTop === 5, F.style.position = "fixed", F.style.top = "20px", this.supportsFixedPosition = F.offsetTop === 20 || F.offsetTop === 15, F.style.position = F.style.top = "", E.style.overflow = "hidden", E.style.position = "relative", this.subtractsBorderForOverflowNotVisible = F.offsetTop === -5, this.doesNotIncludeMarginInBodyOffset = C.offsetTop !== A, C.removeChild(D), C = D = E = F = G = H = null, Au.offset.initialize = Au.noop }, bodyOffset: function (A) { var B = A.offsetTop, C = A.offsetLeft; Au.offset.initialize(), Au.offset.doesNotIncludeMarginInBodyOffset && (B += parseFloat(Au.css(A, "marginTop")) || 0, C += parseFloat(Au.css(A, "marginLeft")) || 0); return { top: B, left: C} }, setOffset: function (G, H, I) { var J = Au.css(G, "position"); J === "static" && (G.style.position = "relative"); var K = Au(G), L = K.offset(), M = Au.css(G, "top"), A = Au.css(G, "left"), B = J === "absolute" && Au.inArray("auto", [M, A]) > -1, C = {}, D = {}, E, F; B && (D = K.position()), E = B ? D.top : parseInt(M, 10) || 0, F = B ? D.left : parseInt(A, 10) || 0, Au.isFunction(H) && (H = H.call(G, I, L)), H.top != null && (C.top = H.top - L.top + E), H.left != null && (C.left = H.left - L.left + F), "using" in H ? H.using.call(G, C) : K.css(C) } }, Au.fn.extend({ position: function () { if (!this[0]) { return null } var A = this[0], B = this.offsetParent(), C = this.offset(), D = AG.test(B[0].nodeName) ? { top: 0, left: 0} : B.offset(); C.top -= parseFloat(Au.css(A, "marginTop")) || 0, C.left -= parseFloat(Au.css(A, "marginLeft")) || 0, D.top += parseFloat(Au.css(B[0], "borderTopWidth")) || 0, D.left += parseFloat(Au.css(B[0], "borderLeftWidth")) || 0; return { top: C.top - D.top, left: C.left - D.left} }, offsetParent: function () { return this.map(function () { var A = this.offsetParent || At.body; while (A && (!AG.test(A.nodeName) && Au.css(A, "position") === "static")) { A = A.offsetParent } return A }) } }), Au.each(["Left", "Top"], function (A, B) { var C = "scroll" + B; Au.fn[C] = function (D) { var E = this[0], F; if (!E) { return null } if (D !== As) { return this.each(function () { F = Bu(this), F ? F.scrollTo(A ? Au(F).scrollLeft() : D, A ? D : Au(F).scrollTop()) : this[C] = D }) } F = Bu(E); return F ? "pageXOffset" in F ? F[A ? "pageYOffset" : "pageXOffset"] : Au.support.boxModel && F.document.documentElement[C] || F.document.body[C] : E[C] } }), Au.each(["Height", "Width"], function (A, B) { var C = B.toLowerCase(); Au.fn["inner" + B] = function () { return this[0] ? parseFloat(Au.css(this[0], C, "padding")) : null }, Au.fn["outer" + B] = function (D) { return this[0] ? parseFloat(Au.css(this[0], C, D ? "margin" : "border")) : null }, Au.fn[C] = function (D) { var G = this[0]; if (!G) { return D == null ? null : this } if (Au.isFunction(D)) { return this.each(function (I) { var J = Au(this); J[C](D.call(this, I, J[C]())) }) } if (Au.isWindow(G)) { var H = G.document.documentElement["client" + B]; return G.document.compatMode === "CSS1Compat" && H || G.document.body["client" + B] || H } if (G.nodeType === 9) { return Math.max(G.documentElement["client" + B], G.body["scroll" + B], G.documentElement["scroll" + B], G.body["offset" + B], G.documentElement["offset" + B]) } if (D === As) { var E = Au.css(G, C), F = parseFloat(E); return Au.isNaN(F) ? E : F } return this.css(C, typeof D === "string" ? D : D + "px") } }), Ar.jQuery = Ar.$ = Au })(window);


/*!
 * jQuery UI 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI
 */
(function(c,j){function k(a,b){var d=a.nodeName.toLowerCase();if("area"===d){b=a.parentNode;d=b.name;if(!a.href||!d||b.nodeName.toLowerCase()!=="map")return false;a=c("img[usemap=#"+d+"]")[0];return!!a&&l(a)}return(/input|select|textarea|button|object/.test(d)?!a.disabled:"a"==d?a.href||b:b)&&l(a)}function l(a){return!c(a).parents().andSelf().filter(function(){return c.curCSS(this,"visibility")==="hidden"||c.expr.filters.hidden(this)}).length}c.ui=c.ui||{};if(!c.ui.version){c.extend(c.ui,{version:"1.8.13",
keyCode:{ALT:18,BACKSPACE:8,CAPS_LOCK:20,COMMA:188,COMMAND:91,COMMAND_LEFT:91,COMMAND_RIGHT:93,CONTROL:17,DELETE:46,DOWN:40,END:35,ENTER:13,ESCAPE:27,HOME:36,INSERT:45,LEFT:37,MENU:93,NUMPAD_ADD:107,NUMPAD_DECIMAL:110,NUMPAD_DIVIDE:111,NUMPAD_ENTER:108,NUMPAD_MULTIPLY:106,NUMPAD_SUBTRACT:109,PAGE_DOWN:34,PAGE_UP:33,PERIOD:190,RIGHT:39,SHIFT:16,SPACE:32,TAB:9,UP:38,WINDOWS:91}});c.fn.extend({_focus:c.fn.focus,focus:function(a,b){return typeof a==="number"?this.each(function(){var d=this;setTimeout(function(){c(d).focus();
b&&b.call(d)},a)}):this._focus.apply(this,arguments)},scrollParent:function(){var a;a=c.browser.msie&&/(static|relative)/.test(this.css("position"))||/absolute/.test(this.css("position"))?this.parents().filter(function(){return/(relative|absolute|fixed)/.test(c.curCSS(this,"position",1))&&/(auto|scroll)/.test(c.curCSS(this,"overflow",1)+c.curCSS(this,"overflow-y",1)+c.curCSS(this,"overflow-x",1))}).eq(0):this.parents().filter(function(){return/(auto|scroll)/.test(c.curCSS(this,"overflow",1)+c.curCSS(this,
"overflow-y",1)+c.curCSS(this,"overflow-x",1))}).eq(0);return/fixed/.test(this.css("position"))||!a.length?c(document):a},zIndex:function(a){if(a!==j)return this.css("zIndex",a);if(this.length){a=c(this[0]);for(var b;a.length&&a[0]!==document;){b=a.css("position");if(b==="absolute"||b==="relative"||b==="fixed"){b=parseInt(a.css("zIndex"),10);if(!isNaN(b)&&b!==0)return b}a=a.parent()}}return 0},disableSelection:function(){return this.bind((c.support.selectstart?"selectstart":"mousedown")+".ui-disableSelection",
function(a){a.preventDefault()})},enableSelection:function(){return this.unbind(".ui-disableSelection")}});c.each(["Width","Height"],function(a,b){function d(f,g,m,n){c.each(e,function(){g-=parseFloat(c.curCSS(f,"padding"+this,true))||0;if(m)g-=parseFloat(c.curCSS(f,"border"+this+"Width",true))||0;if(n)g-=parseFloat(c.curCSS(f,"margin"+this,true))||0});return g}var e=b==="Width"?["Left","Right"]:["Top","Bottom"],h=b.toLowerCase(),i={innerWidth:c.fn.innerWidth,innerHeight:c.fn.innerHeight,outerWidth:c.fn.outerWidth,
outerHeight:c.fn.outerHeight};c.fn["inner"+b]=function(f){if(f===j)return i["inner"+b].call(this);return this.each(function(){c(this).css(h,d(this,f)+"px")})};c.fn["outer"+b]=function(f,g){if(typeof f!=="number")return i["outer"+b].call(this,f);return this.each(function(){c(this).css(h,d(this,f,true,g)+"px")})}});c.extend(c.expr[":"],{data:function(a,b,d){return!!c.data(a,d[3])},focusable:function(a){return k(a,!isNaN(c.attr(a,"tabindex")))},tabbable:function(a){var b=c.attr(a,"tabindex"),d=isNaN(b);
return(d||b>=0)&&k(a,!d)}});c(function(){var a=document.body,b=a.appendChild(b=document.createElement("div"));c.extend(b.style,{minHeight:"100px",height:"auto",padding:0,borderWidth:0});c.support.minHeight=b.offsetHeight===100;c.support.selectstart="onselectstart"in b;a.removeChild(b).style.display="none"});c.extend(c.ui,{plugin:{add:function(a,b,d){a=c.ui[a].prototype;for(var e in d){a.plugins[e]=a.plugins[e]||[];a.plugins[e].push([b,d[e]])}},call:function(a,b,d){if((b=a.plugins[b])&&a.element[0].parentNode)for(var e=
0;e<b.length;e++)a.options[b[e][0]]&&b[e][1].apply(a.element,d)}},contains:function(a,b){return document.compareDocumentPosition?a.compareDocumentPosition(b)&16:a!==b&&a.contains(b)},hasScroll:function(a,b){if(c(a).css("overflow")==="hidden")return false;b=b&&b==="left"?"scrollLeft":"scrollTop";var d=false;if(a[b]>0)return true;a[b]=1;d=a[b]>0;a[b]=0;return d},isOverAxis:function(a,b,d){return a>b&&a<b+d},isOver:function(a,b,d,e,h,i){return c.ui.isOverAxis(a,d,h)&&c.ui.isOverAxis(b,e,i)}})}})(jQuery);
;/*!
 * jQuery UI Widget 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Widget
 */
(function(b,j){if(b.cleanData){var k=b.cleanData;b.cleanData=function(a){for(var c=0,d;(d=a[c])!=null;c++)b(d).triggerHandler("remove");k(a)}}else{var l=b.fn.remove;b.fn.remove=function(a,c){return this.each(function(){if(!c)if(!a||b.filter(a,[this]).length)b("*",this).add([this]).each(function(){b(this).triggerHandler("remove")});return l.call(b(this),a,c)})}}b.widget=function(a,c,d){var e=a.split(".")[0],f;a=a.split(".")[1];f=e+"-"+a;if(!d){d=c;c=b.Widget}b.expr[":"][f]=function(h){return!!b.data(h,
a)};b[e]=b[e]||{};b[e][a]=function(h,g){arguments.length&&this._createWidget(h,g)};c=new c;c.options=b.extend(true,{},c.options);b[e][a].prototype=b.extend(true,c,{namespace:e,widgetName:a,widgetEventPrefix:b[e][a].prototype.widgetEventPrefix||a,widgetBaseClass:f},d);b.widget.bridge(a,b[e][a])};b.widget.bridge=function(a,c){b.fn[a]=function(d){var e=typeof d==="string",f=Array.prototype.slice.call(arguments,1),h=this;d=!e&&f.length?b.extend.apply(null,[true,d].concat(f)):d;if(e&&d.charAt(0)==="_")return h;
e?this.each(function(){var g=b.data(this,a),i=g&&b.isFunction(g[d])?g[d].apply(g,f):g;if(i!==g&&i!==j){h=i;return false}}):this.each(function(){var g=b.data(this,a);g?g.option(d||{})._init():b.data(this,a,new c(d,this))});return h}};b.Widget=function(a,c){arguments.length&&this._createWidget(a,c)};b.Widget.prototype={widgetName:"widget",widgetEventPrefix:"",options:{disabled:false},_createWidget:function(a,c){b.data(c,this.widgetName,this);this.element=b(c);this.options=b.extend(true,{},this.options,
this._getCreateOptions(),a);var d=this;this.element.bind("remove."+this.widgetName,function(){d.destroy()});this._create();this._trigger("create");this._init()},_getCreateOptions:function(){return b.metadata&&b.metadata.get(this.element[0])[this.widgetName]},_create:function(){},_init:function(){},destroy:function(){this.element.unbind("."+this.widgetName).removeData(this.widgetName);this.widget().unbind("."+this.widgetName).removeAttr("aria-disabled").removeClass(this.widgetBaseClass+"-disabled ui-state-disabled")},
widget:function(){return this.element},option:function(a,c){var d=a;if(arguments.length===0)return b.extend({},this.options);if(typeof a==="string"){if(c===j)return this.options[a];d={};d[a]=c}this._setOptions(d);return this},_setOptions:function(a){var c=this;b.each(a,function(d,e){c._setOption(d,e)});return this},_setOption:function(a,c){this.options[a]=c;if(a==="disabled")this.widget()[c?"addClass":"removeClass"](this.widgetBaseClass+"-disabled ui-state-disabled").attr("aria-disabled",c);return this},
enable:function(){return this._setOption("disabled",false)},disable:function(){return this._setOption("disabled",true)},_trigger:function(a,c,d){var e=this.options[a];c=b.Event(c);c.type=(a===this.widgetEventPrefix?a:this.widgetEventPrefix+a).toLowerCase();d=d||{};if(c.originalEvent){a=b.event.props.length;for(var f;a;){f=b.event.props[--a];c[f]=c.originalEvent[f]}}this.element.trigger(c,d);return!(b.isFunction(e)&&e.call(this.element[0],c,d)===false||c.isDefaultPrevented())}}})(jQuery);
;/*!
 * jQuery UI Mouse 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Mouse
 *
 * Depends:
 *	jquery.ui.widget.js
 */
(function(b){var d=false;b(document).mousedown(function(){d=false});b.widget("ui.mouse",{options:{cancel:":input,option",distance:1,delay:0},_mouseInit:function(){var a=this;this.element.bind("mousedown."+this.widgetName,function(c){return a._mouseDown(c)}).bind("click."+this.widgetName,function(c){if(true===b.data(c.target,a.widgetName+".preventClickEvent")){b.removeData(c.target,a.widgetName+".preventClickEvent");c.stopImmediatePropagation();return false}});this.started=false},_mouseDestroy:function(){this.element.unbind("."+
this.widgetName)},_mouseDown:function(a){if(!d){this._mouseStarted&&this._mouseUp(a);this._mouseDownEvent=a;var c=this,f=a.which==1,g=typeof this.options.cancel=="string"?b(a.target).parents().add(a.target).filter(this.options.cancel).length:false;if(!f||g||!this._mouseCapture(a))return true;this.mouseDelayMet=!this.options.delay;if(!this.mouseDelayMet)this._mouseDelayTimer=setTimeout(function(){c.mouseDelayMet=true},this.options.delay);if(this._mouseDistanceMet(a)&&this._mouseDelayMet(a)){this._mouseStarted=
this._mouseStart(a)!==false;if(!this._mouseStarted){a.preventDefault();return true}}true===b.data(a.target,this.widgetName+".preventClickEvent")&&b.removeData(a.target,this.widgetName+".preventClickEvent");this._mouseMoveDelegate=function(e){return c._mouseMove(e)};this._mouseUpDelegate=function(e){return c._mouseUp(e)};b(document).bind("mousemove."+this.widgetName,this._mouseMoveDelegate).bind("mouseup."+this.widgetName,this._mouseUpDelegate);a.preventDefault();return d=true}},_mouseMove:function(a){if(b.browser.msie&&
!(document.documentMode>=9)&&!a.button)return this._mouseUp(a);if(this._mouseStarted){this._mouseDrag(a);return a.preventDefault()}if(this._mouseDistanceMet(a)&&this._mouseDelayMet(a))(this._mouseStarted=this._mouseStart(this._mouseDownEvent,a)!==false)?this._mouseDrag(a):this._mouseUp(a);return!this._mouseStarted},_mouseUp:function(a){b(document).unbind("mousemove."+this.widgetName,this._mouseMoveDelegate).unbind("mouseup."+this.widgetName,this._mouseUpDelegate);if(this._mouseStarted){this._mouseStarted=
false;a.target==this._mouseDownEvent.target&&b.data(a.target,this.widgetName+".preventClickEvent",true);this._mouseStop(a)}return false},_mouseDistanceMet:function(a){return Math.max(Math.abs(this._mouseDownEvent.pageX-a.pageX),Math.abs(this._mouseDownEvent.pageY-a.pageY))>=this.options.distance},_mouseDelayMet:function(){return this.mouseDelayMet},_mouseStart:function(){},_mouseDrag:function(){},_mouseStop:function(){},_mouseCapture:function(){return true}})})(jQuery);
;/*
 * jQuery UI Position 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Position
 */
(function(c){c.ui=c.ui||{};var n=/left|center|right/,o=/top|center|bottom/,t=c.fn.position,u=c.fn.offset;c.fn.position=function(b){if(!b||!b.of)return t.apply(this,arguments);b=c.extend({},b);var a=c(b.of),d=a[0],g=(b.collision||"flip").split(" "),e=b.offset?b.offset.split(" "):[0,0],h,k,j;if(d.nodeType===9){h=a.width();k=a.height();j={top:0,left:0}}else if(d.setTimeout){h=a.width();k=a.height();j={top:a.scrollTop(),left:a.scrollLeft()}}else if(d.preventDefault){b.at="left top";h=k=0;j={top:b.of.pageY,
left:b.of.pageX}}else{h=a.outerWidth();k=a.outerHeight();j=a.offset()}c.each(["my","at"],function(){var f=(b[this]||"").split(" ");if(f.length===1)f=n.test(f[0])?f.concat(["center"]):o.test(f[0])?["center"].concat(f):["center","center"];f[0]=n.test(f[0])?f[0]:"center";f[1]=o.test(f[1])?f[1]:"center";b[this]=f});if(g.length===1)g[1]=g[0];e[0]=parseInt(e[0],10)||0;if(e.length===1)e[1]=e[0];e[1]=parseInt(e[1],10)||0;if(b.at[0]==="right")j.left+=h;else if(b.at[0]==="center")j.left+=h/2;if(b.at[1]==="bottom")j.top+=
k;else if(b.at[1]==="center")j.top+=k/2;j.left+=e[0];j.top+=e[1];return this.each(function(){var f=c(this),l=f.outerWidth(),m=f.outerHeight(),p=parseInt(c.curCSS(this,"marginLeft",true))||0,q=parseInt(c.curCSS(this,"marginTop",true))||0,v=l+p+(parseInt(c.curCSS(this,"marginRight",true))||0),w=m+q+(parseInt(c.curCSS(this,"marginBottom",true))||0),i=c.extend({},j),r;if(b.my[0]==="right")i.left-=l;else if(b.my[0]==="center")i.left-=l/2;if(b.my[1]==="bottom")i.top-=m;else if(b.my[1]==="center")i.top-=
m/2;i.left=Math.round(i.left);i.top=Math.round(i.top);r={left:i.left-p,top:i.top-q};c.each(["left","top"],function(s,x){c.ui.position[g[s]]&&c.ui.position[g[s]][x](i,{targetWidth:h,targetHeight:k,elemWidth:l,elemHeight:m,collisionPosition:r,collisionWidth:v,collisionHeight:w,offset:e,my:b.my,at:b.at})});c.fn.bgiframe&&f.bgiframe();f.offset(c.extend(i,{using:b.using}))})};c.ui.position={fit:{left:function(b,a){var d=c(window);d=a.collisionPosition.left+a.collisionWidth-d.width()-d.scrollLeft();b.left=
d>0?b.left-d:Math.max(b.left-a.collisionPosition.left,b.left)},top:function(b,a){var d=c(window);d=a.collisionPosition.top+a.collisionHeight-d.height()-d.scrollTop();b.top=d>0?b.top-d:Math.max(b.top-a.collisionPosition.top,b.top)}},flip:{left:function(b,a){if(a.at[0]!=="center"){var d=c(window);d=a.collisionPosition.left+a.collisionWidth-d.width()-d.scrollLeft();var g=a.my[0]==="left"?-a.elemWidth:a.my[0]==="right"?a.elemWidth:0,e=a.at[0]==="left"?a.targetWidth:-a.targetWidth,h=-2*a.offset[0];b.left+=
a.collisionPosition.left<0?g+e+h:d>0?g+e+h:0}},top:function(b,a){if(a.at[1]!=="center"){var d=c(window);d=a.collisionPosition.top+a.collisionHeight-d.height()-d.scrollTop();var g=a.my[1]==="top"?-a.elemHeight:a.my[1]==="bottom"?a.elemHeight:0,e=a.at[1]==="top"?a.targetHeight:-a.targetHeight,h=-2*a.offset[1];b.top+=a.collisionPosition.top<0?g+e+h:d>0?g+e+h:0}}}};if(!c.offset.setOffset){c.offset.setOffset=function(b,a){if(/static/.test(c.curCSS(b,"position")))b.style.position="relative";var d=c(b),
g=d.offset(),e=parseInt(c.curCSS(b,"top",true),10)||0,h=parseInt(c.curCSS(b,"left",true),10)||0;g={top:a.top-g.top+e,left:a.left-g.left+h};"using"in a?a.using.call(b,g):d.css(g)};c.fn.offset=function(b){var a=this[0];if(!a||!a.ownerDocument)return null;if(b)return this.each(function(){c.offset.setOffset(this,b)});return u.call(this)}}})(jQuery);
;/*
 * jQuery UI Draggable 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Draggables
 *
 * Depends:
 *	jquery.ui.core.js
 *	jquery.ui.mouse.js
 *	jquery.ui.widget.js
 */
(function(d){d.widget("ui.draggable",d.ui.mouse,{widgetEventPrefix:"drag",options:{addClasses:true,appendTo:"parent",axis:false,connectToSortable:false,containment:false,cursor:"auto",cursorAt:false,grid:false,handle:false,helper:"original",iframeFix:false,opacity:false,refreshPositions:false,revert:false,revertDuration:500,scope:"default",scroll:true,scrollSensitivity:20,scrollSpeed:20,snap:false,snapMode:"both",snapTolerance:20,stack:false,zIndex:false},_create:function(){if(this.options.helper==
"original"&&!/^(?:r|a|f)/.test(this.element.css("position")))this.element[0].style.position="relative";this.options.addClasses&&this.element.addClass("ui-draggable");this.options.disabled&&this.element.addClass("ui-draggable-disabled");this._mouseInit()},destroy:function(){if(this.element.data("draggable")){this.element.removeData("draggable").unbind(".draggable").removeClass("ui-draggable ui-draggable-dragging ui-draggable-disabled");this._mouseDestroy();return this}},_mouseCapture:function(a){var b=
this.options;if(this.helper||b.disabled||d(a.target).is(".ui-resizable-handle"))return false;this.handle=this._getHandle(a);if(!this.handle)return false;d(b.iframeFix===true?"iframe":b.iframeFix).each(function(){d('<div class="ui-draggable-iframeFix" style="background: #fff;"></div>').css({width:this.offsetWidth+"px",height:this.offsetHeight+"px",position:"absolute",opacity:"0.001",zIndex:1E3}).css(d(this).offset()).appendTo("body")});return true},_mouseStart:function(a){var b=this.options;this.helper=
this._createHelper(a);this._cacheHelperProportions();if(d.ui.ddmanager)d.ui.ddmanager.current=this;this._cacheMargins();this.cssPosition=this.helper.css("position");this.scrollParent=this.helper.scrollParent();this.offset=this.positionAbs=this.element.offset();this.offset={top:this.offset.top-this.margins.top,left:this.offset.left-this.margins.left};d.extend(this.offset,{click:{left:a.pageX-this.offset.left,top:a.pageY-this.offset.top},parent:this._getParentOffset(),relative:this._getRelativeOffset()});
this.originalPosition=this.position=this._generatePosition(a);this.originalPageX=a.pageX;this.originalPageY=a.pageY;b.cursorAt&&this._adjustOffsetFromHelper(b.cursorAt);b.containment&&this._setContainment();if(this._trigger("start",a)===false){this._clear();return false}this._cacheHelperProportions();d.ui.ddmanager&&!b.dropBehaviour&&d.ui.ddmanager.prepareOffsets(this,a);this.helper.addClass("ui-draggable-dragging");this._mouseDrag(a,true);return true},_mouseDrag:function(a,b){this.position=this._generatePosition(a);
this.positionAbs=this._convertPositionTo("absolute");if(!b){b=this._uiHash();if(this._trigger("drag",a,b)===false){this._mouseUp({});return false}this.position=b.position}if(!this.options.axis||this.options.axis!="y")this.helper[0].style.left=this.position.left+"px";if(!this.options.axis||this.options.axis!="x")this.helper[0].style.top=this.position.top+"px";d.ui.ddmanager&&d.ui.ddmanager.drag(this,a);return false},_mouseStop:function(a){var b=false;if(d.ui.ddmanager&&!this.options.dropBehaviour)b=
d.ui.ddmanager.drop(this,a);if(this.dropped){b=this.dropped;this.dropped=false}if((!this.element[0]||!this.element[0].parentNode)&&this.options.helper=="original")return false;if(this.options.revert=="invalid"&&!b||this.options.revert=="valid"&&b||this.options.revert===true||d.isFunction(this.options.revert)&&this.options.revert.call(this.element,b)){var c=this;d(this.helper).animate(this.originalPosition,parseInt(this.options.revertDuration,10),function(){c._trigger("stop",a)!==false&&c._clear()})}else this._trigger("stop",
a)!==false&&this._clear();return false},_mouseUp:function(a){this.options.iframeFix===true&&d("div.ui-draggable-iframeFix").each(function(){this.parentNode.removeChild(this)});return d.ui.mouse.prototype._mouseUp.call(this,a)},cancel:function(){this.helper.is(".ui-draggable-dragging")?this._mouseUp({}):this._clear();return this},_getHandle:function(a){var b=!this.options.handle||!d(this.options.handle,this.element).length?true:false;d(this.options.handle,this.element).find("*").andSelf().each(function(){if(this==
a.target)b=true});return b},_createHelper:function(a){var b=this.options;a=d.isFunction(b.helper)?d(b.helper.apply(this.element[0],[a])):b.helper=="clone"?this.element.clone().removeAttr("id"):this.element;a.parents("body").length||a.appendTo(b.appendTo=="parent"?this.element[0].parentNode:b.appendTo);a[0]!=this.element[0]&&!/(fixed|absolute)/.test(a.css("position"))&&a.css("position","absolute");return a},_adjustOffsetFromHelper:function(a){if(typeof a=="string")a=a.split(" ");if(d.isArray(a))a=
{left:+a[0],top:+a[1]||0};if("left"in a)this.offset.click.left=a.left+this.margins.left;if("right"in a)this.offset.click.left=this.helperProportions.width-a.right+this.margins.left;if("top"in a)this.offset.click.top=a.top+this.margins.top;if("bottom"in a)this.offset.click.top=this.helperProportions.height-a.bottom+this.margins.top},_getParentOffset:function(){this.offsetParent=this.helper.offsetParent();var a=this.offsetParent.offset();if(this.cssPosition=="absolute"&&this.scrollParent[0]!=document&&
d.ui.contains(this.scrollParent[0],this.offsetParent[0])){a.left+=this.scrollParent.scrollLeft();a.top+=this.scrollParent.scrollTop()}if(this.offsetParent[0]==document.body||this.offsetParent[0].tagName&&this.offsetParent[0].tagName.toLowerCase()=="html"&&d.browser.msie)a={top:0,left:0};return{top:a.top+(parseInt(this.offsetParent.css("borderTopWidth"),10)||0),left:a.left+(parseInt(this.offsetParent.css("borderLeftWidth"),10)||0)}},_getRelativeOffset:function(){if(this.cssPosition=="relative"){var a=
this.element.position();return{top:a.top-(parseInt(this.helper.css("top"),10)||0)+this.scrollParent.scrollTop(),left:a.left-(parseInt(this.helper.css("left"),10)||0)+this.scrollParent.scrollLeft()}}else return{top:0,left:0}},_cacheMargins:function(){this.margins={left:parseInt(this.element.css("marginLeft"),10)||0,top:parseInt(this.element.css("marginTop"),10)||0,right:parseInt(this.element.css("marginRight"),10)||0,bottom:parseInt(this.element.css("marginBottom"),10)||0}},_cacheHelperProportions:function(){this.helperProportions=
{width:this.helper.outerWidth(),height:this.helper.outerHeight()}},_setContainment:function(){var a=this.options;if(a.containment=="parent")a.containment=this.helper[0].parentNode;if(a.containment=="document"||a.containment=="window")this.containment=[(a.containment=="document"?0:d(window).scrollLeft())-this.offset.relative.left-this.offset.parent.left,(a.containment=="document"?0:d(window).scrollTop())-this.offset.relative.top-this.offset.parent.top,(a.containment=="document"?0:d(window).scrollLeft())+
d(a.containment=="document"?document:window).width()-this.helperProportions.width-this.margins.left,(a.containment=="document"?0:d(window).scrollTop())+(d(a.containment=="document"?document:window).height()||document.body.parentNode.scrollHeight)-this.helperProportions.height-this.margins.top];if(!/^(document|window|parent)$/.test(a.containment)&&a.containment.constructor!=Array){a=d(a.containment);var b=a[0];if(b){a.offset();var c=d(b).css("overflow")!="hidden";this.containment=[(parseInt(d(b).css("borderLeftWidth"),
10)||0)+(parseInt(d(b).css("paddingLeft"),10)||0),(parseInt(d(b).css("borderTopWidth"),10)||0)+(parseInt(d(b).css("paddingTop"),10)||0),(c?Math.max(b.scrollWidth,b.offsetWidth):b.offsetWidth)-(parseInt(d(b).css("borderLeftWidth"),10)||0)-(parseInt(d(b).css("paddingRight"),10)||0)-this.helperProportions.width-this.margins.left-this.margins.right,(c?Math.max(b.scrollHeight,b.offsetHeight):b.offsetHeight)-(parseInt(d(b).css("borderTopWidth"),10)||0)-(parseInt(d(b).css("paddingBottom"),10)||0)-this.helperProportions.height-
this.margins.top-this.margins.bottom];this.relative_container=a}}else if(a.containment.constructor==Array)this.containment=a.containment},_convertPositionTo:function(a,b){if(!b)b=this.position;a=a=="absolute"?1:-1;var c=this.cssPosition=="absolute"&&!(this.scrollParent[0]!=document&&d.ui.contains(this.scrollParent[0],this.offsetParent[0]))?this.offsetParent:this.scrollParent,f=/(html|body)/i.test(c[0].tagName);return{top:b.top+this.offset.relative.top*a+this.offset.parent.top*a-(d.browser.safari&&
d.browser.version<526&&this.cssPosition=="fixed"?0:(this.cssPosition=="fixed"?-this.scrollParent.scrollTop():f?0:c.scrollTop())*a),left:b.left+this.offset.relative.left*a+this.offset.parent.left*a-(d.browser.safari&&d.browser.version<526&&this.cssPosition=="fixed"?0:(this.cssPosition=="fixed"?-this.scrollParent.scrollLeft():f?0:c.scrollLeft())*a)}},_generatePosition:function(a){var b=this.options,c=this.cssPosition=="absolute"&&!(this.scrollParent[0]!=document&&d.ui.contains(this.scrollParent[0],
this.offsetParent[0]))?this.offsetParent:this.scrollParent,f=/(html|body)/i.test(c[0].tagName),e=a.pageX,h=a.pageY;if(this.originalPosition){var g;if(this.containment){if(this.relative_container){g=this.relative_container.offset();g=[this.containment[0]+g.left,this.containment[1]+g.top,this.containment[2]+g.left,this.containment[3]+g.top]}else g=this.containment;if(a.pageX-this.offset.click.left<g[0])e=g[0]+this.offset.click.left;if(a.pageY-this.offset.click.top<g[1])h=g[1]+this.offset.click.top;
if(a.pageX-this.offset.click.left>g[2])e=g[2]+this.offset.click.left;if(a.pageY-this.offset.click.top>g[3])h=g[3]+this.offset.click.top}if(b.grid){h=this.originalPageY+Math.round((h-this.originalPageY)/b.grid[1])*b.grid[1];h=g?!(h-this.offset.click.top<g[1]||h-this.offset.click.top>g[3])?h:!(h-this.offset.click.top<g[1])?h-b.grid[1]:h+b.grid[1]:h;e=this.originalPageX+Math.round((e-this.originalPageX)/b.grid[0])*b.grid[0];e=g?!(e-this.offset.click.left<g[0]||e-this.offset.click.left>g[2])?e:!(e-this.offset.click.left<
g[0])?e-b.grid[0]:e+b.grid[0]:e}}return{top:h-this.offset.click.top-this.offset.relative.top-this.offset.parent.top+(d.browser.safari&&d.browser.version<526&&this.cssPosition=="fixed"?0:this.cssPosition=="fixed"?-this.scrollParent.scrollTop():f?0:c.scrollTop()),left:e-this.offset.click.left-this.offset.relative.left-this.offset.parent.left+(d.browser.safari&&d.browser.version<526&&this.cssPosition=="fixed"?0:this.cssPosition=="fixed"?-this.scrollParent.scrollLeft():f?0:c.scrollLeft())}},_clear:function(){this.helper.removeClass("ui-draggable-dragging");
this.helper[0]!=this.element[0]&&!this.cancelHelperRemoval&&this.helper.remove();this.helper=null;this.cancelHelperRemoval=false},_trigger:function(a,b,c){c=c||this._uiHash();d.ui.plugin.call(this,a,[b,c]);if(a=="drag")this.positionAbs=this._convertPositionTo("absolute");return d.Widget.prototype._trigger.call(this,a,b,c)},plugins:{},_uiHash:function(){return{helper:this.helper,position:this.position,originalPosition:this.originalPosition,offset:this.positionAbs}}});d.extend(d.ui.draggable,{version:"1.8.13"});
d.ui.plugin.add("draggable","connectToSortable",{start:function(a,b){var c=d(this).data("draggable"),f=c.options,e=d.extend({},b,{item:c.element});c.sortables=[];d(f.connectToSortable).each(function(){var h=d.data(this,"sortable");if(h&&!h.options.disabled){c.sortables.push({instance:h,shouldRevert:h.options.revert});h.refreshPositions();h._trigger("activate",a,e)}})},stop:function(a,b){var c=d(this).data("draggable"),f=d.extend({},b,{item:c.element});d.each(c.sortables,function(){if(this.instance.isOver){this.instance.isOver=
0;c.cancelHelperRemoval=true;this.instance.cancelHelperRemoval=false;if(this.shouldRevert)this.instance.options.revert=true;this.instance._mouseStop(a);this.instance.options.helper=this.instance.options._helper;c.options.helper=="original"&&this.instance.currentItem.css({top:"auto",left:"auto"})}else{this.instance.cancelHelperRemoval=false;this.instance._trigger("deactivate",a,f)}})},drag:function(a,b){var c=d(this).data("draggable"),f=this;d.each(c.sortables,function(){this.instance.positionAbs=
c.positionAbs;this.instance.helperProportions=c.helperProportions;this.instance.offset.click=c.offset.click;if(this.instance._intersectsWith(this.instance.containerCache)){if(!this.instance.isOver){this.instance.isOver=1;this.instance.currentItem=d(f).clone().removeAttr("id").appendTo(this.instance.element).data("sortable-item",true);this.instance.options._helper=this.instance.options.helper;this.instance.options.helper=function(){return b.helper[0]};a.target=this.instance.currentItem[0];this.instance._mouseCapture(a,
true);this.instance._mouseStart(a,true,true);this.instance.offset.click.top=c.offset.click.top;this.instance.offset.click.left=c.offset.click.left;this.instance.offset.parent.left-=c.offset.parent.left-this.instance.offset.parent.left;this.instance.offset.parent.top-=c.offset.parent.top-this.instance.offset.parent.top;c._trigger("toSortable",a);c.dropped=this.instance.element;c.currentItem=c.element;this.instance.fromOutside=c}this.instance.currentItem&&this.instance._mouseDrag(a)}else if(this.instance.isOver){this.instance.isOver=
0;this.instance.cancelHelperRemoval=true;this.instance.options.revert=false;this.instance._trigger("out",a,this.instance._uiHash(this.instance));this.instance._mouseStop(a,true);this.instance.options.helper=this.instance.options._helper;this.instance.currentItem.remove();this.instance.placeholder&&this.instance.placeholder.remove();c._trigger("fromSortable",a);c.dropped=false}})}});d.ui.plugin.add("draggable","cursor",{start:function(){var a=d("body"),b=d(this).data("draggable").options;if(a.css("cursor"))b._cursor=
a.css("cursor");a.css("cursor",b.cursor)},stop:function(){var a=d(this).data("draggable").options;a._cursor&&d("body").css("cursor",a._cursor)}});d.ui.plugin.add("draggable","opacity",{start:function(a,b){a=d(b.helper);b=d(this).data("draggable").options;if(a.css("opacity"))b._opacity=a.css("opacity");a.css("opacity",b.opacity)},stop:function(a,b){a=d(this).data("draggable").options;a._opacity&&d(b.helper).css("opacity",a._opacity)}});d.ui.plugin.add("draggable","scroll",{start:function(){var a=d(this).data("draggable");
if(a.scrollParent[0]!=document&&a.scrollParent[0].tagName!="HTML")a.overflowOffset=a.scrollParent.offset()},drag:function(a){var b=d(this).data("draggable"),c=b.options,f=false;if(b.scrollParent[0]!=document&&b.scrollParent[0].tagName!="HTML"){if(!c.axis||c.axis!="x")if(b.overflowOffset.top+b.scrollParent[0].offsetHeight-a.pageY<c.scrollSensitivity)b.scrollParent[0].scrollTop=f=b.scrollParent[0].scrollTop+c.scrollSpeed;else if(a.pageY-b.overflowOffset.top<c.scrollSensitivity)b.scrollParent[0].scrollTop=
f=b.scrollParent[0].scrollTop-c.scrollSpeed;if(!c.axis||c.axis!="y")if(b.overflowOffset.left+b.scrollParent[0].offsetWidth-a.pageX<c.scrollSensitivity)b.scrollParent[0].scrollLeft=f=b.scrollParent[0].scrollLeft+c.scrollSpeed;else if(a.pageX-b.overflowOffset.left<c.scrollSensitivity)b.scrollParent[0].scrollLeft=f=b.scrollParent[0].scrollLeft-c.scrollSpeed}else{if(!c.axis||c.axis!="x")if(a.pageY-d(document).scrollTop()<c.scrollSensitivity)f=d(document).scrollTop(d(document).scrollTop()-c.scrollSpeed);
else if(d(window).height()-(a.pageY-d(document).scrollTop())<c.scrollSensitivity)f=d(document).scrollTop(d(document).scrollTop()+c.scrollSpeed);if(!c.axis||c.axis!="y")if(a.pageX-d(document).scrollLeft()<c.scrollSensitivity)f=d(document).scrollLeft(d(document).scrollLeft()-c.scrollSpeed);else if(d(window).width()-(a.pageX-d(document).scrollLeft())<c.scrollSensitivity)f=d(document).scrollLeft(d(document).scrollLeft()+c.scrollSpeed)}f!==false&&d.ui.ddmanager&&!c.dropBehaviour&&d.ui.ddmanager.prepareOffsets(b,
a)}});d.ui.plugin.add("draggable","snap",{start:function(){var a=d(this).data("draggable"),b=a.options;a.snapElements=[];d(b.snap.constructor!=String?b.snap.items||":data(draggable)":b.snap).each(function(){var c=d(this),f=c.offset();this!=a.element[0]&&a.snapElements.push({item:this,width:c.outerWidth(),height:c.outerHeight(),top:f.top,left:f.left})})},drag:function(a,b){for(var c=d(this).data("draggable"),f=c.options,e=f.snapTolerance,h=b.offset.left,g=h+c.helperProportions.width,n=b.offset.top,
o=n+c.helperProportions.height,i=c.snapElements.length-1;i>=0;i--){var j=c.snapElements[i].left,l=j+c.snapElements[i].width,k=c.snapElements[i].top,m=k+c.snapElements[i].height;if(j-e<h&&h<l+e&&k-e<n&&n<m+e||j-e<h&&h<l+e&&k-e<o&&o<m+e||j-e<g&&g<l+e&&k-e<n&&n<m+e||j-e<g&&g<l+e&&k-e<o&&o<m+e){if(f.snapMode!="inner"){var p=Math.abs(k-o)<=e,q=Math.abs(m-n)<=e,r=Math.abs(j-g)<=e,s=Math.abs(l-h)<=e;if(p)b.position.top=c._convertPositionTo("relative",{top:k-c.helperProportions.height,left:0}).top-c.margins.top;
if(q)b.position.top=c._convertPositionTo("relative",{top:m,left:0}).top-c.margins.top;if(r)b.position.left=c._convertPositionTo("relative",{top:0,left:j-c.helperProportions.width}).left-c.margins.left;if(s)b.position.left=c._convertPositionTo("relative",{top:0,left:l}).left-c.margins.left}var t=p||q||r||s;if(f.snapMode!="outer"){p=Math.abs(k-n)<=e;q=Math.abs(m-o)<=e;r=Math.abs(j-h)<=e;s=Math.abs(l-g)<=e;if(p)b.position.top=c._convertPositionTo("relative",{top:k,left:0}).top-c.margins.top;if(q)b.position.top=
c._convertPositionTo("relative",{top:m-c.helperProportions.height,left:0}).top-c.margins.top;if(r)b.position.left=c._convertPositionTo("relative",{top:0,left:j}).left-c.margins.left;if(s)b.position.left=c._convertPositionTo("relative",{top:0,left:l-c.helperProportions.width}).left-c.margins.left}if(!c.snapElements[i].snapping&&(p||q||r||s||t))c.options.snap.snap&&c.options.snap.snap.call(c.element,a,d.extend(c._uiHash(),{snapItem:c.snapElements[i].item}));c.snapElements[i].snapping=p||q||r||s||t}else{c.snapElements[i].snapping&&
c.options.snap.release&&c.options.snap.release.call(c.element,a,d.extend(c._uiHash(),{snapItem:c.snapElements[i].item}));c.snapElements[i].snapping=false}}}});d.ui.plugin.add("draggable","stack",{start:function(){var a=d(this).data("draggable").options;a=d.makeArray(d(a.stack)).sort(function(c,f){return(parseInt(d(c).css("zIndex"),10)||0)-(parseInt(d(f).css("zIndex"),10)||0)});if(a.length){var b=parseInt(a[0].style.zIndex)||0;d(a).each(function(c){this.style.zIndex=b+c});this[0].style.zIndex=b+a.length}}});
d.ui.plugin.add("draggable","zIndex",{start:function(a,b){a=d(b.helper);b=d(this).data("draggable").options;if(a.css("zIndex"))b._zIndex=a.css("zIndex");a.css("zIndex",b.zIndex)},stop:function(a,b){a=d(this).data("draggable").options;a._zIndex&&d(b.helper).css("zIndex",a._zIndex)}})})(jQuery);
;/*
 * jQuery UI Droppable 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Droppables
 *
 * Depends:
 *	jquery.ui.core.js
 *	jquery.ui.widget.js
 *	jquery.ui.mouse.js
 *	jquery.ui.draggable.js
 */
(function(d){d.widget("ui.droppable",{widgetEventPrefix:"drop",options:{accept:"*",activeClass:false,addClasses:true,greedy:false,hoverClass:false,scope:"default",tolerance:"intersect"},_create:function(){var a=this.options,b=a.accept;this.isover=0;this.isout=1;this.accept=d.isFunction(b)?b:function(c){return c.is(b)};this.proportions={width:this.element[0].offsetWidth,height:this.element[0].offsetHeight};d.ui.ddmanager.droppables[a.scope]=d.ui.ddmanager.droppables[a.scope]||[];d.ui.ddmanager.droppables[a.scope].push(this);
a.addClasses&&this.element.addClass("ui-droppable")},destroy:function(){for(var a=d.ui.ddmanager.droppables[this.options.scope],b=0;b<a.length;b++)a[b]==this&&a.splice(b,1);this.element.removeClass("ui-droppable ui-droppable-disabled").removeData("droppable").unbind(".droppable");return this},_setOption:function(a,b){if(a=="accept")this.accept=d.isFunction(b)?b:function(c){return c.is(b)};d.Widget.prototype._setOption.apply(this,arguments)},_activate:function(a){var b=d.ui.ddmanager.current;this.options.activeClass&&
this.element.addClass(this.options.activeClass);b&&this._trigger("activate",a,this.ui(b))},_deactivate:function(a){var b=d.ui.ddmanager.current;this.options.activeClass&&this.element.removeClass(this.options.activeClass);b&&this._trigger("deactivate",a,this.ui(b))},_over:function(a){var b=d.ui.ddmanager.current;if(!(!b||(b.currentItem||b.element)[0]==this.element[0]))if(this.accept.call(this.element[0],b.currentItem||b.element)){this.options.hoverClass&&this.element.addClass(this.options.hoverClass);
this._trigger("over",a,this.ui(b))}},_out:function(a){var b=d.ui.ddmanager.current;if(!(!b||(b.currentItem||b.element)[0]==this.element[0]))if(this.accept.call(this.element[0],b.currentItem||b.element)){this.options.hoverClass&&this.element.removeClass(this.options.hoverClass);this._trigger("out",a,this.ui(b))}},_drop:function(a,b){var c=b||d.ui.ddmanager.current;if(!c||(c.currentItem||c.element)[0]==this.element[0])return false;var e=false;this.element.find(":data(droppable)").not(".ui-draggable-dragging").each(function(){var g=
d.data(this,"droppable");if(g.options.greedy&&!g.options.disabled&&g.options.scope==c.options.scope&&g.accept.call(g.element[0],c.currentItem||c.element)&&d.ui.intersect(c,d.extend(g,{offset:g.element.offset()}),g.options.tolerance)){e=true;return false}});if(e)return false;if(this.accept.call(this.element[0],c.currentItem||c.element)){this.options.activeClass&&this.element.removeClass(this.options.activeClass);this.options.hoverClass&&this.element.removeClass(this.options.hoverClass);this._trigger("drop",
a,this.ui(c));return this.element}return false},ui:function(a){return{draggable:a.currentItem||a.element,helper:a.helper,position:a.position,offset:a.positionAbs}}});d.extend(d.ui.droppable,{version:"1.8.13"});d.ui.intersect=function(a,b,c){if(!b.offset)return false;var e=(a.positionAbs||a.position.absolute).left,g=e+a.helperProportions.width,f=(a.positionAbs||a.position.absolute).top,h=f+a.helperProportions.height,i=b.offset.left,k=i+b.proportions.width,j=b.offset.top,l=j+b.proportions.height;
switch(c){case "fit":return i<=e&&g<=k&&j<=f&&h<=l;case "intersect":return i<e+a.helperProportions.width/2&&g-a.helperProportions.width/2<k&&j<f+a.helperProportions.height/2&&h-a.helperProportions.height/2<l;case "pointer":return d.ui.isOver((a.positionAbs||a.position.absolute).top+(a.clickOffset||a.offset.click).top,(a.positionAbs||a.position.absolute).left+(a.clickOffset||a.offset.click).left,j,i,b.proportions.height,b.proportions.width);case "touch":return(f>=j&&f<=l||h>=j&&h<=l||f<j&&h>l)&&(e>=
i&&e<=k||g>=i&&g<=k||e<i&&g>k);default:return false}};d.ui.ddmanager={current:null,droppables:{"default":[]},prepareOffsets:function(a,b){var c=d.ui.ddmanager.droppables[a.options.scope]||[],e=b?b.type:null,g=(a.currentItem||a.element).find(":data(droppable)").andSelf(),f=0;a:for(;f<c.length;f++)if(!(c[f].options.disabled||a&&!c[f].accept.call(c[f].element[0],a.currentItem||a.element))){for(var h=0;h<g.length;h++)if(g[h]==c[f].element[0]){c[f].proportions.height=0;continue a}c[f].visible=c[f].element.css("display")!=
"none";if(c[f].visible){e=="mousedown"&&c[f]._activate.call(c[f],b);c[f].offset=c[f].element.offset();c[f].proportions={width:c[f].element[0].offsetWidth,height:c[f].element[0].offsetHeight}}}},drop:function(a,b){var c=false;d.each(d.ui.ddmanager.droppables[a.options.scope]||[],function(){if(this.options){if(!this.options.disabled&&this.visible&&d.ui.intersect(a,this,this.options.tolerance))c=c||this._drop.call(this,b);if(!this.options.disabled&&this.visible&&this.accept.call(this.element[0],a.currentItem||
a.element)){this.isout=1;this.isover=0;this._deactivate.call(this,b)}}});return c},drag:function(a,b){a.options.refreshPositions&&d.ui.ddmanager.prepareOffsets(a,b);d.each(d.ui.ddmanager.droppables[a.options.scope]||[],function(){if(!(this.options.disabled||this.greedyChild||!this.visible)){var c=d.ui.intersect(a,this,this.options.tolerance);if(c=!c&&this.isover==1?"isout":c&&this.isover==0?"isover":null){var e;if(this.options.greedy){var g=this.element.parents(":data(droppable):eq(0)");if(g.length){e=
d.data(g[0],"droppable");e.greedyChild=c=="isover"?1:0}}if(e&&c=="isover"){e.isover=0;e.isout=1;e._out.call(e,b)}this[c]=1;this[c=="isout"?"isover":"isout"]=0;this[c=="isover"?"_over":"_out"].call(this,b);if(e&&c=="isout"){e.isout=0;e.isover=1;e._over.call(e,b)}}}})}}})(jQuery);
;/*
 * jQuery UI Resizable 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Resizables
 *
 * Depends:
 *	jquery.ui.core.js
 *	jquery.ui.mouse.js
 *	jquery.ui.widget.js
 */
(function(e){e.widget("ui.resizable",e.ui.mouse,{widgetEventPrefix:"resize",options:{alsoResize:false,animate:false,animateDuration:"slow",animateEasing:"swing",aspectRatio:false,autoHide:false,containment:false,ghost:false,grid:false,handles:"e,s,se",helper:false,maxHeight:null,maxWidth:null,minHeight:10,minWidth:10,zIndex:1E3},_create:function(){var b=this,a=this.options;this.element.addClass("ui-resizable");e.extend(this,{_aspectRatio:!!a.aspectRatio,aspectRatio:a.aspectRatio,originalElement:this.element,
_proportionallyResizeElements:[],_helper:a.helper||a.ghost||a.animate?a.helper||"ui-resizable-helper":null});if(this.element[0].nodeName.match(/canvas|textarea|input|select|button|img/i)){/relative/.test(this.element.css("position"))&&e.browser.opera&&this.element.css({position:"relative",top:"auto",left:"auto"});this.element.wrap(e('<div class="ui-wrapper" style="overflow: hidden;"></div>').css({position:this.element.css("position"),width:this.element.outerWidth(),height:this.element.outerHeight(),
top:this.element.css("top"),left:this.element.css("left")}));this.element=this.element.parent().data("resizable",this.element.data("resizable"));this.elementIsWrapper=true;this.element.css({marginLeft:this.originalElement.css("marginLeft"),marginTop:this.originalElement.css("marginTop"),marginRight:this.originalElement.css("marginRight"),marginBottom:this.originalElement.css("marginBottom")});this.originalElement.css({marginLeft:0,marginTop:0,marginRight:0,marginBottom:0});this.originalResizeStyle=
this.originalElement.css("resize");this.originalElement.css("resize","none");this._proportionallyResizeElements.push(this.originalElement.css({position:"static",zoom:1,display:"block"}));this.originalElement.css({margin:this.originalElement.css("margin")});this._proportionallyResize()}this.handles=a.handles||(!e(".ui-resizable-handle",this.element).length?"e,s,se":{n:".ui-resizable-n",e:".ui-resizable-e",s:".ui-resizable-s",w:".ui-resizable-w",se:".ui-resizable-se",sw:".ui-resizable-sw",ne:".ui-resizable-ne",
nw:".ui-resizable-nw"});if(this.handles.constructor==String){if(this.handles=="all")this.handles="n,e,s,w,se,sw,ne,nw";var c=this.handles.split(",");this.handles={};for(var d=0;d<c.length;d++){var f=e.trim(c[d]),g=e('<div class="ui-resizable-handle '+("ui-resizable-"+f)+'"></div>');/sw|se|ne|nw/.test(f)&&g.css({zIndex:++a.zIndex});"se"==f&&g.addClass("ui-icon ui-icon-gripsmall-diagonal-se");this.handles[f]=".ui-resizable-"+f;this.element.append(g)}}this._renderAxis=function(h){h=h||this.element;for(var i in this.handles){if(this.handles[i].constructor==
String)this.handles[i]=e(this.handles[i],this.element).show();if(this.elementIsWrapper&&this.originalElement[0].nodeName.match(/textarea|input|select|button/i)){var j=e(this.handles[i],this.element),k=0;k=/sw|ne|nw|se|n|s/.test(i)?j.outerHeight():j.outerWidth();j=["padding",/ne|nw|n/.test(i)?"Top":/se|sw|s/.test(i)?"Bottom":/^e$/.test(i)?"Right":"Left"].join("");h.css(j,k);this._proportionallyResize()}e(this.handles[i])}};this._renderAxis(this.element);this._handles=e(".ui-resizable-handle",this.element).disableSelection();
this._handles.mouseover(function(){if(!b.resizing){if(this.className)var h=this.className.match(/ui-resizable-(se|sw|ne|nw|n|e|s|w)/i);b.axis=h&&h[1]?h[1]:"se"}});if(a.autoHide){this._handles.hide();e(this.element).addClass("ui-resizable-autohide").hover(function(){if(!a.disabled){e(this).removeClass("ui-resizable-autohide");b._handles.show()}},function(){if(!a.disabled)if(!b.resizing){e(this).addClass("ui-resizable-autohide");b._handles.hide()}})}this._mouseInit()},destroy:function(){this._mouseDestroy();
var b=function(c){e(c).removeClass("ui-resizable ui-resizable-disabled ui-resizable-resizing").removeData("resizable").unbind(".resizable").find(".ui-resizable-handle").remove()};if(this.elementIsWrapper){b(this.element);var a=this.element;a.after(this.originalElement.css({position:a.css("position"),width:a.outerWidth(),height:a.outerHeight(),top:a.css("top"),left:a.css("left")})).remove()}this.originalElement.css("resize",this.originalResizeStyle);b(this.originalElement);return this},_mouseCapture:function(b){var a=
false;for(var c in this.handles)if(e(this.handles[c])[0]==b.target)a=true;return!this.options.disabled&&a},_mouseStart:function(b){var a=this.options,c=this.element.position(),d=this.element;this.resizing=true;this.documentScroll={top:e(document).scrollTop(),left:e(document).scrollLeft()};if(d.is(".ui-draggable")||/absolute/.test(d.css("position")))d.css({position:"absolute",top:c.top,left:c.left});e.browser.opera&&/relative/.test(d.css("position"))&&d.css({position:"relative",top:"auto",left:"auto"});
this._renderProxy();c=m(this.helper.css("left"));var f=m(this.helper.css("top"));if(a.containment){c+=e(a.containment).scrollLeft()||0;f+=e(a.containment).scrollTop()||0}this.offset=this.helper.offset();this.position={left:c,top:f};this.size=this._helper?{width:d.outerWidth(),height:d.outerHeight()}:{width:d.width(),height:d.height()};this.originalSize=this._helper?{width:d.outerWidth(),height:d.outerHeight()}:{width:d.width(),height:d.height()};this.originalPosition={left:c,top:f};this.sizeDiff=
{width:d.outerWidth()-d.width(),height:d.outerHeight()-d.height()};this.originalMousePosition={left:b.pageX,top:b.pageY};this.aspectRatio=typeof a.aspectRatio=="number"?a.aspectRatio:this.originalSize.width/this.originalSize.height||1;a=e(".ui-resizable-"+this.axis).css("cursor");e("body").css("cursor",a=="auto"?this.axis+"-resize":a);d.addClass("ui-resizable-resizing");this._propagate("start",b);return true},_mouseDrag:function(b){var a=this.helper,c=this.originalMousePosition,d=this._change[this.axis];
if(!d)return false;c=d.apply(this,[b,b.pageX-c.left||0,b.pageY-c.top||0]);if(this._aspectRatio||b.shiftKey)c=this._updateRatio(c,b);c=this._respectSize(c,b);this._propagate("resize",b);a.css({top:this.position.top+"px",left:this.position.left+"px",width:this.size.width+"px",height:this.size.height+"px"});!this._helper&&this._proportionallyResizeElements.length&&this._proportionallyResize();this._updateCache(c);this._trigger("resize",b,this.ui());return false},_mouseStop:function(b){this.resizing=
false;var a=this.options,c=this;if(this._helper){var d=this._proportionallyResizeElements,f=d.length&&/textarea/i.test(d[0].nodeName);d=f&&e.ui.hasScroll(d[0],"left")?0:c.sizeDiff.height;f=f?0:c.sizeDiff.width;f={width:c.helper.width()-f,height:c.helper.height()-d};d=parseInt(c.element.css("left"),10)+(c.position.left-c.originalPosition.left)||null;var g=parseInt(c.element.css("top"),10)+(c.position.top-c.originalPosition.top)||null;a.animate||this.element.css(e.extend(f,{top:g,left:d}));c.helper.height(c.size.height);
c.helper.width(c.size.width);this._helper&&!a.animate&&this._proportionallyResize()}e("body").css("cursor","auto");this.element.removeClass("ui-resizable-resizing");this._propagate("stop",b);this._helper&&this.helper.remove();return false},_updateCache:function(b){this.offset=this.helper.offset();if(l(b.left))this.position.left=b.left;if(l(b.top))this.position.top=b.top;if(l(b.height))this.size.height=b.height;if(l(b.width))this.size.width=b.width},_updateRatio:function(b){var a=this.position,c=this.size,
d=this.axis;if(b.height)b.width=c.height*this.aspectRatio;else if(b.width)b.height=c.width/this.aspectRatio;if(d=="sw"){b.left=a.left+(c.width-b.width);b.top=null}if(d=="nw"){b.top=a.top+(c.height-b.height);b.left=a.left+(c.width-b.width)}return b},_respectSize:function(b){var a=this.options,c=this.axis,d=l(b.width)&&a.maxWidth&&a.maxWidth<b.width,f=l(b.height)&&a.maxHeight&&a.maxHeight<b.height,g=l(b.width)&&a.minWidth&&a.minWidth>b.width,h=l(b.height)&&a.minHeight&&a.minHeight>b.height;if(g)b.width=
a.minWidth;if(h)b.height=a.minHeight;if(d)b.width=a.maxWidth;if(f)b.height=a.maxHeight;var i=this.originalPosition.left+this.originalSize.width,j=this.position.top+this.size.height,k=/sw|nw|w/.test(c);c=/nw|ne|n/.test(c);if(g&&k)b.left=i-a.minWidth;if(d&&k)b.left=i-a.maxWidth;if(h&&c)b.top=j-a.minHeight;if(f&&c)b.top=j-a.maxHeight;if((a=!b.width&&!b.height)&&!b.left&&b.top)b.top=null;else if(a&&!b.top&&b.left)b.left=null;return b},_proportionallyResize:function(){if(this._proportionallyResizeElements.length)for(var b=
this.helper||this.element,a=0;a<this._proportionallyResizeElements.length;a++){var c=this._proportionallyResizeElements[a];if(!this.borderDif){var d=[c.css("borderTopWidth"),c.css("borderRightWidth"),c.css("borderBottomWidth"),c.css("borderLeftWidth")],f=[c.css("paddingTop"),c.css("paddingRight"),c.css("paddingBottom"),c.css("paddingLeft")];this.borderDif=e.map(d,function(g,h){g=parseInt(g,10)||0;h=parseInt(f[h],10)||0;return g+h})}e.browser.msie&&(e(b).is(":hidden")||e(b).parents(":hidden").length)||
c.css({height:b.height()-this.borderDif[0]-this.borderDif[2]||0,width:b.width()-this.borderDif[1]-this.borderDif[3]||0})}},_renderProxy:function(){var b=this.options;this.elementOffset=this.element.offset();if(this._helper){this.helper=this.helper||e('<div style="overflow:hidden;"></div>');var a=e.browser.msie&&e.browser.version<7,c=a?1:0;a=a?2:-1;this.helper.addClass(this._helper).css({width:this.element.outerWidth()+a,height:this.element.outerHeight()+a,position:"absolute",left:this.elementOffset.left-
c+"px",top:this.elementOffset.top-c+"px",zIndex:++b.zIndex});this.helper.appendTo("body").disableSelection()}else this.helper=this.element},_change:{e:function(b,a){return{width:this.originalSize.width+a}},w:function(b,a){return{left:this.originalPosition.left+a,width:this.originalSize.width-a}},n:function(b,a,c){return{top:this.originalPosition.top+c,height:this.originalSize.height-c}},s:function(b,a,c){return{height:this.originalSize.height+c}},se:function(b,a,c){return e.extend(this._change.s.apply(this,
arguments),this._change.e.apply(this,[b,a,c]))},sw:function(b,a,c){return e.extend(this._change.s.apply(this,arguments),this._change.w.apply(this,[b,a,c]))},ne:function(b,a,c){return e.extend(this._change.n.apply(this,arguments),this._change.e.apply(this,[b,a,c]))},nw:function(b,a,c){return e.extend(this._change.n.apply(this,arguments),this._change.w.apply(this,[b,a,c]))}},_propagate:function(b,a){e.ui.plugin.call(this,b,[a,this.ui()]);b!="resize"&&this._trigger(b,a,this.ui())},plugins:{},ui:function(){return{originalElement:this.originalElement,
element:this.element,helper:this.helper,position:this.position,size:this.size,originalSize:this.originalSize,originalPosition:this.originalPosition}}});e.extend(e.ui.resizable,{version:"1.8.13"});e.ui.plugin.add("resizable","alsoResize",{start:function(){var b=e(this).data("resizable").options,a=function(c){e(c).each(function(){var d=e(this);d.data("resizable-alsoresize",{width:parseInt(d.width(),10),height:parseInt(d.height(),10),left:parseInt(d.css("left"),10),top:parseInt(d.css("top"),10),position:d.css("position")})})};
if(typeof b.alsoResize=="object"&&!b.alsoResize.parentNode)if(b.alsoResize.length){b.alsoResize=b.alsoResize[0];a(b.alsoResize)}else e.each(b.alsoResize,function(c){a(c)});else a(b.alsoResize)},resize:function(b,a){var c=e(this).data("resizable");b=c.options;var d=c.originalSize,f=c.originalPosition,g={height:c.size.height-d.height||0,width:c.size.width-d.width||0,top:c.position.top-f.top||0,left:c.position.left-f.left||0},h=function(i,j){e(i).each(function(){var k=e(this),q=e(this).data("resizable-alsoresize"),
p={},r=j&&j.length?j:k.parents(a.originalElement[0]).length?["width","height"]:["width","height","top","left"];e.each(r,function(n,o){if((n=(q[o]||0)+(g[o]||0))&&n>=0)p[o]=n||null});if(e.browser.opera&&/relative/.test(k.css("position"))){c._revertToRelativePosition=true;k.css({position:"absolute",top:"auto",left:"auto"})}k.css(p)})};typeof b.alsoResize=="object"&&!b.alsoResize.nodeType?e.each(b.alsoResize,function(i,j){h(i,j)}):h(b.alsoResize)},stop:function(){var b=e(this).data("resizable"),a=b.options,
c=function(d){e(d).each(function(){var f=e(this);f.css({position:f.data("resizable-alsoresize").position})})};if(b._revertToRelativePosition){b._revertToRelativePosition=false;typeof a.alsoResize=="object"&&!a.alsoResize.nodeType?e.each(a.alsoResize,function(d){c(d)}):c(a.alsoResize)}e(this).removeData("resizable-alsoresize")}});e.ui.plugin.add("resizable","animate",{stop:function(b){var a=e(this).data("resizable"),c=a.options,d=a._proportionallyResizeElements,f=d.length&&/textarea/i.test(d[0].nodeName),
g=f&&e.ui.hasScroll(d[0],"left")?0:a.sizeDiff.height;f={width:a.size.width-(f?0:a.sizeDiff.width),height:a.size.height-g};g=parseInt(a.element.css("left"),10)+(a.position.left-a.originalPosition.left)||null;var h=parseInt(a.element.css("top"),10)+(a.position.top-a.originalPosition.top)||null;a.element.animate(e.extend(f,h&&g?{top:h,left:g}:{}),{duration:c.animateDuration,easing:c.animateEasing,step:function(){var i={width:parseInt(a.element.css("width"),10),height:parseInt(a.element.css("height"),
10),top:parseInt(a.element.css("top"),10),left:parseInt(a.element.css("left"),10)};d&&d.length&&e(d[0]).css({width:i.width,height:i.height});a._updateCache(i);a._propagate("resize",b)}})}});e.ui.plugin.add("resizable","containment",{start:function(){var b=e(this).data("resizable"),a=b.element,c=b.options.containment;if(a=c instanceof e?c.get(0):/parent/.test(c)?a.parent().get(0):c){b.containerElement=e(a);if(/document/.test(c)||c==document){b.containerOffset={left:0,top:0};b.containerPosition={left:0,
top:0};b.parentData={element:e(document),left:0,top:0,width:e(document).width(),height:e(document).height()||document.body.parentNode.scrollHeight}}else{var d=e(a),f=[];e(["Top","Right","Left","Bottom"]).each(function(i,j){f[i]=m(d.css("padding"+j))});b.containerOffset=d.offset();b.containerPosition=d.position();b.containerSize={height:d.innerHeight()-f[3],width:d.innerWidth()-f[1]};c=b.containerOffset;var g=b.containerSize.height,h=b.containerSize.width;h=e.ui.hasScroll(a,"left")?a.scrollWidth:h;
g=e.ui.hasScroll(a)?a.scrollHeight:g;b.parentData={element:a,left:c.left,top:c.top,width:h,height:g}}}},resize:function(b){var a=e(this).data("resizable"),c=a.options,d=a.containerOffset,f=a.position;b=a._aspectRatio||b.shiftKey;var g={top:0,left:0},h=a.containerElement;if(h[0]!=document&&/static/.test(h.css("position")))g=d;if(f.left<(a._helper?d.left:0)){a.size.width+=a._helper?a.position.left-d.left:a.position.left-g.left;if(b)a.size.height=a.size.width/c.aspectRatio;a.position.left=c.helper?d.left:
0}if(f.top<(a._helper?d.top:0)){a.size.height+=a._helper?a.position.top-d.top:a.position.top;if(b)a.size.width=a.size.height*c.aspectRatio;a.position.top=a._helper?d.top:0}a.offset.left=a.parentData.left+a.position.left;a.offset.top=a.parentData.top+a.position.top;c=Math.abs((a._helper?a.offset.left-g.left:a.offset.left-g.left)+a.sizeDiff.width);d=Math.abs((a._helper?a.offset.top-g.top:a.offset.top-d.top)+a.sizeDiff.height);f=a.containerElement.get(0)==a.element.parent().get(0);g=/relative|absolute/.test(a.containerElement.css("position"));
if(f&&g)c-=a.parentData.left;if(c+a.size.width>=a.parentData.width){a.size.width=a.parentData.width-c;if(b)a.size.height=a.size.width/a.aspectRatio}if(d+a.size.height>=a.parentData.height){a.size.height=a.parentData.height-d;if(b)a.size.width=a.size.height*a.aspectRatio}},stop:function(){var b=e(this).data("resizable"),a=b.options,c=b.containerOffset,d=b.containerPosition,f=b.containerElement,g=e(b.helper),h=g.offset(),i=g.outerWidth()-b.sizeDiff.width;g=g.outerHeight()-b.sizeDiff.height;b._helper&&
!a.animate&&/relative/.test(f.css("position"))&&e(this).css({left:h.left-d.left-c.left,width:i,height:g});b._helper&&!a.animate&&/static/.test(f.css("position"))&&e(this).css({left:h.left-d.left-c.left,width:i,height:g})}});e.ui.plugin.add("resizable","ghost",{start:function(){var b=e(this).data("resizable"),a=b.options,c=b.size;b.ghost=b.originalElement.clone();b.ghost.css({opacity:0.25,display:"block",position:"relative",height:c.height,width:c.width,margin:0,left:0,top:0}).addClass("ui-resizable-ghost").addClass(typeof a.ghost==
"string"?a.ghost:"");b.ghost.appendTo(b.helper)},resize:function(){var b=e(this).data("resizable");b.ghost&&b.ghost.css({position:"relative",height:b.size.height,width:b.size.width})},stop:function(){var b=e(this).data("resizable");b.ghost&&b.helper&&b.helper.get(0).removeChild(b.ghost.get(0))}});e.ui.plugin.add("resizable","grid",{resize:function(){var b=e(this).data("resizable"),a=b.options,c=b.size,d=b.originalSize,f=b.originalPosition,g=b.axis;a.grid=typeof a.grid=="number"?[a.grid,a.grid]:a.grid;
var h=Math.round((c.width-d.width)/(a.grid[0]||1))*(a.grid[0]||1);a=Math.round((c.height-d.height)/(a.grid[1]||1))*(a.grid[1]||1);if(/^(se|s|e)$/.test(g)){b.size.width=d.width+h;b.size.height=d.height+a}else if(/^(ne)$/.test(g)){b.size.width=d.width+h;b.size.height=d.height+a;b.position.top=f.top-a}else{if(/^(sw)$/.test(g)){b.size.width=d.width+h;b.size.height=d.height+a}else{b.size.width=d.width+h;b.size.height=d.height+a;b.position.top=f.top-a}b.position.left=f.left-h}}});var m=function(b){return parseInt(b,
10)||0},l=function(b){return!isNaN(parseInt(b,10))}})(jQuery);
;/*
 * jQuery UI Selectable 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Selectables
 *
 * Depends:
 *	jquery.ui.core.js
 *	jquery.ui.mouse.js
 *	jquery.ui.widget.js
 */
(function(e){e.widget("ui.selectable",e.ui.mouse,{options:{appendTo:"body",autoRefresh:true,distance:0,filter:"*",tolerance:"touch"},_create:function(){var c=this;this.element.addClass("ui-selectable");this.dragged=false;var f;this.refresh=function(){f=e(c.options.filter,c.element[0]);f.each(function(){var d=e(this),b=d.offset();e.data(this,"selectable-item",{element:this,$element:d,left:b.left,top:b.top,right:b.left+d.outerWidth(),bottom:b.top+d.outerHeight(),startselected:false,selected:d.hasClass("ui-selected"),
selecting:d.hasClass("ui-selecting"),unselecting:d.hasClass("ui-unselecting")})})};this.refresh();this.selectees=f.addClass("ui-selectee");this._mouseInit();this.helper=e("<div class='ui-selectable-helper'></div>")},destroy:function(){this.selectees.removeClass("ui-selectee").removeData("selectable-item");this.element.removeClass("ui-selectable ui-selectable-disabled").removeData("selectable").unbind(".selectable");this._mouseDestroy();return this},_mouseStart:function(c){var f=this;this.opos=[c.pageX,
c.pageY];if(!this.options.disabled){var d=this.options;this.selectees=e(d.filter,this.element[0]);this._trigger("start",c);e(d.appendTo).append(this.helper);this.helper.css({left:c.clientX,top:c.clientY,width:0,height:0});d.autoRefresh&&this.refresh();this.selectees.filter(".ui-selected").each(function(){var b=e.data(this,"selectable-item");b.startselected=true;if(!c.metaKey){b.$element.removeClass("ui-selected");b.selected=false;b.$element.addClass("ui-unselecting");b.unselecting=true;f._trigger("unselecting",
c,{unselecting:b.element})}});e(c.target).parents().andSelf().each(function(){var b=e.data(this,"selectable-item");if(b){var g=!c.metaKey||!b.$element.hasClass("ui-selected");b.$element.removeClass(g?"ui-unselecting":"ui-selected").addClass(g?"ui-selecting":"ui-unselecting");b.unselecting=!g;b.selecting=g;(b.selected=g)?f._trigger("selecting",c,{selecting:b.element}):f._trigger("unselecting",c,{unselecting:b.element});return false}})}},_mouseDrag:function(c){var f=this;this.dragged=true;if(!this.options.disabled){var d=
this.options,b=this.opos[0],g=this.opos[1],h=c.pageX,i=c.pageY;if(b>h){var j=h;h=b;b=j}if(g>i){j=i;i=g;g=j}this.helper.css({left:b,top:g,width:h-b,height:i-g});this.selectees.each(function(){var a=e.data(this,"selectable-item");if(!(!a||a.element==f.element[0])){var k=false;if(d.tolerance=="touch")k=!(a.left>h||a.right<b||a.top>i||a.bottom<g);else if(d.tolerance=="fit")k=a.left>b&&a.right<h&&a.top>g&&a.bottom<i;if(k){if(a.selected){a.$element.removeClass("ui-selected");a.selected=false}if(a.unselecting){a.$element.removeClass("ui-unselecting");
a.unselecting=false}if(!a.selecting){a.$element.addClass("ui-selecting");a.selecting=true;f._trigger("selecting",c,{selecting:a.element})}}else{if(a.selecting)if(c.metaKey&&a.startselected){a.$element.removeClass("ui-selecting");a.selecting=false;a.$element.addClass("ui-selected");a.selected=true}else{a.$element.removeClass("ui-selecting");a.selecting=false;if(a.startselected){a.$element.addClass("ui-unselecting");a.unselecting=true}f._trigger("unselecting",c,{unselecting:a.element})}if(a.selected)if(!c.metaKey&&
!a.startselected){a.$element.removeClass("ui-selected");a.selected=false;a.$element.addClass("ui-unselecting");a.unselecting=true;f._trigger("unselecting",c,{unselecting:a.element})}}}});return false}},_mouseStop:function(c){var f=this;this.dragged=false;e(".ui-unselecting",this.element[0]).each(function(){var d=e.data(this,"selectable-item");d.$element.removeClass("ui-unselecting");d.unselecting=false;d.startselected=false;f._trigger("unselected",c,{unselected:d.element})});e(".ui-selecting",this.element[0]).each(function(){var d=
e.data(this,"selectable-item");d.$element.removeClass("ui-selecting").addClass("ui-selected");d.selecting=false;d.selected=true;d.startselected=true;f._trigger("selected",c,{selected:d.element})});this._trigger("stop",c);this.helper.remove();return false}});e.extend(e.ui.selectable,{version:"1.8.13"})})(jQuery);
;/*
 * jQuery UI Sortable 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Sortables
 *
 * Depends:
 *	jquery.ui.core.js
 *	jquery.ui.mouse.js
 *	jquery.ui.widget.js
 */
(function(d){d.widget("ui.sortable",d.ui.mouse,{widgetEventPrefix:"sort",options:{appendTo:"parent",axis:false,connectWith:false,containment:false,cursor:"auto",cursorAt:false,dropOnEmpty:true,forcePlaceholderSize:false,forceHelperSize:false,grid:false,handle:false,helper:"original",items:"> *",opacity:false,placeholder:false,revert:false,scroll:true,scrollSensitivity:20,scrollSpeed:20,scope:"default",tolerance:"intersect",zIndex:1E3},_create:function(){var a=this.options;this.containerCache={};this.element.addClass("ui-sortable");
this.refresh();this.floating=this.items.length?a.axis==="x"||/left|right/.test(this.items[0].item.css("float"))||/inline|table-cell/.test(this.items[0].item.css("display")):false;this.offset=this.element.offset();this._mouseInit()},destroy:function(){this.element.removeClass("ui-sortable ui-sortable-disabled").removeData("sortable").unbind(".sortable");this._mouseDestroy();for(var a=this.items.length-1;a>=0;a--)this.items[a].item.removeData("sortable-item");return this},_setOption:function(a,b){if(a===
"disabled"){this.options[a]=b;this.widget()[b?"addClass":"removeClass"]("ui-sortable-disabled")}else d.Widget.prototype._setOption.apply(this,arguments)},_mouseCapture:function(a,b){if(this.reverting)return false;if(this.options.disabled||this.options.type=="static")return false;this._refreshItems(a);var c=null,e=this;d(a.target).parents().each(function(){if(d.data(this,"sortable-item")==e){c=d(this);return false}});if(d.data(a.target,"sortable-item")==e)c=d(a.target);if(!c)return false;if(this.options.handle&&
!b){var f=false;d(this.options.handle,c).find("*").andSelf().each(function(){if(this==a.target)f=true});if(!f)return false}this.currentItem=c;this._removeCurrentsFromItems();return true},_mouseStart:function(a,b,c){b=this.options;var e=this;this.currentContainer=this;this.refreshPositions();this.helper=this._createHelper(a);this._cacheHelperProportions();this._cacheMargins();this.scrollParent=this.helper.scrollParent();this.offset=this.currentItem.offset();this.offset={top:this.offset.top-this.margins.top,
left:this.offset.left-this.margins.left};this.helper.css("position","absolute");this.cssPosition=this.helper.css("position");d.extend(this.offset,{click:{left:a.pageX-this.offset.left,top:a.pageY-this.offset.top},parent:this._getParentOffset(),relative:this._getRelativeOffset()});this.originalPosition=this._generatePosition(a);this.originalPageX=a.pageX;this.originalPageY=a.pageY;b.cursorAt&&this._adjustOffsetFromHelper(b.cursorAt);this.domPosition={prev:this.currentItem.prev()[0],parent:this.currentItem.parent()[0]};
this.helper[0]!=this.currentItem[0]&&this.currentItem.hide();this._createPlaceholder();b.containment&&this._setContainment();if(b.cursor){if(d("body").css("cursor"))this._storedCursor=d("body").css("cursor");d("body").css("cursor",b.cursor)}if(b.opacity){if(this.helper.css("opacity"))this._storedOpacity=this.helper.css("opacity");this.helper.css("opacity",b.opacity)}if(b.zIndex){if(this.helper.css("zIndex"))this._storedZIndex=this.helper.css("zIndex");this.helper.css("zIndex",b.zIndex)}if(this.scrollParent[0]!=
document&&this.scrollParent[0].tagName!="HTML")this.overflowOffset=this.scrollParent.offset();this._trigger("start",a,this._uiHash());this._preserveHelperProportions||this._cacheHelperProportions();if(!c)for(c=this.containers.length-1;c>=0;c--)this.containers[c]._trigger("activate",a,e._uiHash(this));if(d.ui.ddmanager)d.ui.ddmanager.current=this;d.ui.ddmanager&&!b.dropBehaviour&&d.ui.ddmanager.prepareOffsets(this,a);this.dragging=true;this.helper.addClass("ui-sortable-helper");this._mouseDrag(a);
return true},_mouseDrag:function(a){this.position=this._generatePosition(a);this.positionAbs=this._convertPositionTo("absolute");if(!this.lastPositionAbs)this.lastPositionAbs=this.positionAbs;if(this.options.scroll){var b=this.options,c=false;if(this.scrollParent[0]!=document&&this.scrollParent[0].tagName!="HTML"){if(this.overflowOffset.top+this.scrollParent[0].offsetHeight-a.pageY<b.scrollSensitivity)this.scrollParent[0].scrollTop=c=this.scrollParent[0].scrollTop+b.scrollSpeed;else if(a.pageY-this.overflowOffset.top<
b.scrollSensitivity)this.scrollParent[0].scrollTop=c=this.scrollParent[0].scrollTop-b.scrollSpeed;if(this.overflowOffset.left+this.scrollParent[0].offsetWidth-a.pageX<b.scrollSensitivity)this.scrollParent[0].scrollLeft=c=this.scrollParent[0].scrollLeft+b.scrollSpeed;else if(a.pageX-this.overflowOffset.left<b.scrollSensitivity)this.scrollParent[0].scrollLeft=c=this.scrollParent[0].scrollLeft-b.scrollSpeed}else{if(a.pageY-d(document).scrollTop()<b.scrollSensitivity)c=d(document).scrollTop(d(document).scrollTop()-
b.scrollSpeed);else if(d(window).height()-(a.pageY-d(document).scrollTop())<b.scrollSensitivity)c=d(document).scrollTop(d(document).scrollTop()+b.scrollSpeed);if(a.pageX-d(document).scrollLeft()<b.scrollSensitivity)c=d(document).scrollLeft(d(document).scrollLeft()-b.scrollSpeed);else if(d(window).width()-(a.pageX-d(document).scrollLeft())<b.scrollSensitivity)c=d(document).scrollLeft(d(document).scrollLeft()+b.scrollSpeed)}c!==false&&d.ui.ddmanager&&!b.dropBehaviour&&d.ui.ddmanager.prepareOffsets(this,
a)}this.positionAbs=this._convertPositionTo("absolute");if(!this.options.axis||this.options.axis!="y")this.helper[0].style.left=this.position.left+"px";if(!this.options.axis||this.options.axis!="x")this.helper[0].style.top=this.position.top+"px";for(b=this.items.length-1;b>=0;b--){c=this.items[b];var e=c.item[0],f=this._intersectsWithPointer(c);if(f)if(e!=this.currentItem[0]&&this.placeholder[f==1?"next":"prev"]()[0]!=e&&!d.ui.contains(this.placeholder[0],e)&&(this.options.type=="semi-dynamic"?!d.ui.contains(this.element[0],
e):true)){this.direction=f==1?"down":"up";if(this.options.tolerance=="pointer"||this._intersectsWithSides(c))this._rearrange(a,c);else break;this._trigger("change",a,this._uiHash());break}}this._contactContainers(a);d.ui.ddmanager&&d.ui.ddmanager.drag(this,a);this._trigger("sort",a,this._uiHash());this.lastPositionAbs=this.positionAbs;return false},_mouseStop:function(a,b){if(a){d.ui.ddmanager&&!this.options.dropBehaviour&&d.ui.ddmanager.drop(this,a);if(this.options.revert){var c=this;b=c.placeholder.offset();
c.reverting=true;d(this.helper).animate({left:b.left-this.offset.parent.left-c.margins.left+(this.offsetParent[0]==document.body?0:this.offsetParent[0].scrollLeft),top:b.top-this.offset.parent.top-c.margins.top+(this.offsetParent[0]==document.body?0:this.offsetParent[0].scrollTop)},parseInt(this.options.revert,10)||500,function(){c._clear(a)})}else this._clear(a,b);return false}},cancel:function(){var a=this;if(this.dragging){this._mouseUp({target:null});this.options.helper=="original"?this.currentItem.css(this._storedCSS).removeClass("ui-sortable-helper"):
this.currentItem.show();for(var b=this.containers.length-1;b>=0;b--){this.containers[b]._trigger("deactivate",null,a._uiHash(this));if(this.containers[b].containerCache.over){this.containers[b]._trigger("out",null,a._uiHash(this));this.containers[b].containerCache.over=0}}}if(this.placeholder){this.placeholder[0].parentNode&&this.placeholder[0].parentNode.removeChild(this.placeholder[0]);this.options.helper!="original"&&this.helper&&this.helper[0].parentNode&&this.helper.remove();d.extend(this,{helper:null,
dragging:false,reverting:false,_noFinalSort:null});this.domPosition.prev?d(this.domPosition.prev).after(this.currentItem):d(this.domPosition.parent).prepend(this.currentItem)}return this},serialize:function(a){var b=this._getItemsAsjQuery(a&&a.connected),c=[];a=a||{};d(b).each(function(){var e=(d(a.item||this).attr(a.attribute||"id")||"").match(a.expression||/(.+)[-=_](.+)/);if(e)c.push((a.key||e[1]+"[]")+"="+(a.key&&a.expression?e[1]:e[2]))});!c.length&&a.key&&c.push(a.key+"=");return c.join("&")},
toArray:function(a){var b=this._getItemsAsjQuery(a&&a.connected),c=[];a=a||{};b.each(function(){c.push(d(a.item||this).attr(a.attribute||"id")||"")});return c},_intersectsWith:function(a){var b=this.positionAbs.left,c=b+this.helperProportions.width,e=this.positionAbs.top,f=e+this.helperProportions.height,g=a.left,h=g+a.width,i=a.top,k=i+a.height,j=this.offset.click.top,l=this.offset.click.left;j=e+j>i&&e+j<k&&b+l>g&&b+l<h;return this.options.tolerance=="pointer"||this.options.forcePointerForContainers||
this.options.tolerance!="pointer"&&this.helperProportions[this.floating?"width":"height"]>a[this.floating?"width":"height"]?j:g<b+this.helperProportions.width/2&&c-this.helperProportions.width/2<h&&i<e+this.helperProportions.height/2&&f-this.helperProportions.height/2<k},_intersectsWithPointer:function(a){var b=d.ui.isOverAxis(this.positionAbs.top+this.offset.click.top,a.top,a.height);a=d.ui.isOverAxis(this.positionAbs.left+this.offset.click.left,a.left,a.width);b=b&&a;a=this._getDragVerticalDirection();
var c=this._getDragHorizontalDirection();if(!b)return false;return this.floating?c&&c=="right"||a=="down"?2:1:a&&(a=="down"?2:1)},_intersectsWithSides:function(a){var b=d.ui.isOverAxis(this.positionAbs.top+this.offset.click.top,a.top+a.height/2,a.height);a=d.ui.isOverAxis(this.positionAbs.left+this.offset.click.left,a.left+a.width/2,a.width);var c=this._getDragVerticalDirection(),e=this._getDragHorizontalDirection();return this.floating&&e?e=="right"&&a||e=="left"&&!a:c&&(c=="down"&&b||c=="up"&&!b)},
_getDragVerticalDirection:function(){var a=this.positionAbs.top-this.lastPositionAbs.top;return a!=0&&(a>0?"down":"up")},_getDragHorizontalDirection:function(){var a=this.positionAbs.left-this.lastPositionAbs.left;return a!=0&&(a>0?"right":"left")},refresh:function(a){this._refreshItems(a);this.refreshPositions();return this},_connectWith:function(){var a=this.options;return a.connectWith.constructor==String?[a.connectWith]:a.connectWith},_getItemsAsjQuery:function(a){var b=[],c=[],e=this._connectWith();
if(e&&a)for(a=e.length-1;a>=0;a--)for(var f=d(e[a]),g=f.length-1;g>=0;g--){var h=d.data(f[g],"sortable");if(h&&h!=this&&!h.options.disabled)c.push([d.isFunction(h.options.items)?h.options.items.call(h.element):d(h.options.items,h.element).not(".ui-sortable-helper").not(".ui-sortable-placeholder"),h])}c.push([d.isFunction(this.options.items)?this.options.items.call(this.element,null,{options:this.options,item:this.currentItem}):d(this.options.items,this.element).not(".ui-sortable-helper").not(".ui-sortable-placeholder"),
this]);for(a=c.length-1;a>=0;a--)c[a][0].each(function(){b.push(this)});return d(b)},_removeCurrentsFromItems:function(){for(var a=this.currentItem.find(":data(sortable-item)"),b=0;b<this.items.length;b++)for(var c=0;c<a.length;c++)a[c]==this.items[b].item[0]&&this.items.splice(b,1)},_refreshItems:function(a){this.items=[];this.containers=[this];var b=this.items,c=[[d.isFunction(this.options.items)?this.options.items.call(this.element[0],a,{item:this.currentItem}):d(this.options.items,this.element),
this]],e=this._connectWith();if(e)for(var f=e.length-1;f>=0;f--)for(var g=d(e[f]),h=g.length-1;h>=0;h--){var i=d.data(g[h],"sortable");if(i&&i!=this&&!i.options.disabled){c.push([d.isFunction(i.options.items)?i.options.items.call(i.element[0],a,{item:this.currentItem}):d(i.options.items,i.element),i]);this.containers.push(i)}}for(f=c.length-1;f>=0;f--){a=c[f][1];e=c[f][0];h=0;for(g=e.length;h<g;h++){i=d(e[h]);i.data("sortable-item",a);b.push({item:i,instance:a,width:0,height:0,left:0,top:0})}}},refreshPositions:function(a){if(this.offsetParent&&
this.helper)this.offset.parent=this._getParentOffset();for(var b=this.items.length-1;b>=0;b--){var c=this.items[b];if(!(c.instance!=this.currentContainer&&this.currentContainer&&c.item[0]!=this.currentItem[0])){var e=this.options.toleranceElement?d(this.options.toleranceElement,c.item):c.item;if(!a){c.width=e.outerWidth();c.height=e.outerHeight()}e=e.offset();c.left=e.left;c.top=e.top}}if(this.options.custom&&this.options.custom.refreshContainers)this.options.custom.refreshContainers.call(this);else for(b=
this.containers.length-1;b>=0;b--){e=this.containers[b].element.offset();this.containers[b].containerCache.left=e.left;this.containers[b].containerCache.top=e.top;this.containers[b].containerCache.width=this.containers[b].element.outerWidth();this.containers[b].containerCache.height=this.containers[b].element.outerHeight()}return this},_createPlaceholder:function(a){var b=a||this,c=b.options;if(!c.placeholder||c.placeholder.constructor==String){var e=c.placeholder;c.placeholder={element:function(){var f=
d(document.createElement(b.currentItem[0].nodeName)).addClass(e||b.currentItem[0].className+" ui-sortable-placeholder").removeClass("ui-sortable-helper")[0];if(!e)f.style.visibility="hidden";return f},update:function(f,g){if(!(e&&!c.forcePlaceholderSize)){g.height()||g.height(b.currentItem.innerHeight()-parseInt(b.currentItem.css("paddingTop")||0,10)-parseInt(b.currentItem.css("paddingBottom")||0,10));g.width()||g.width(b.currentItem.innerWidth()-parseInt(b.currentItem.css("paddingLeft")||0,10)-parseInt(b.currentItem.css("paddingRight")||
0,10))}}}}b.placeholder=d(c.placeholder.element.call(b.element,b.currentItem));b.currentItem.after(b.placeholder);c.placeholder.update(b,b.placeholder)},_contactContainers:function(a){for(var b=null,c=null,e=this.containers.length-1;e>=0;e--)if(!d.ui.contains(this.currentItem[0],this.containers[e].element[0]))if(this._intersectsWith(this.containers[e].containerCache)){if(!(b&&d.ui.contains(this.containers[e].element[0],b.element[0]))){b=this.containers[e];c=e}}else if(this.containers[e].containerCache.over){this.containers[e]._trigger("out",
a,this._uiHash(this));this.containers[e].containerCache.over=0}if(b)if(this.containers.length===1){this.containers[c]._trigger("over",a,this._uiHash(this));this.containers[c].containerCache.over=1}else if(this.currentContainer!=this.containers[c]){b=1E4;e=null;for(var f=this.positionAbs[this.containers[c].floating?"left":"top"],g=this.items.length-1;g>=0;g--)if(d.ui.contains(this.containers[c].element[0],this.items[g].item[0])){var h=this.items[g][this.containers[c].floating?"left":"top"];if(Math.abs(h-
f)<b){b=Math.abs(h-f);e=this.items[g]}}if(e||this.options.dropOnEmpty){this.currentContainer=this.containers[c];e?this._rearrange(a,e,null,true):this._rearrange(a,null,this.containers[c].element,true);this._trigger("change",a,this._uiHash());this.containers[c]._trigger("change",a,this._uiHash(this));this.options.placeholder.update(this.currentContainer,this.placeholder);this.containers[c]._trigger("over",a,this._uiHash(this));this.containers[c].containerCache.over=1}}},_createHelper:function(a){var b=
this.options;a=d.isFunction(b.helper)?d(b.helper.apply(this.element[0],[a,this.currentItem])):b.helper=="clone"?this.currentItem.clone():this.currentItem;a.parents("body").length||d(b.appendTo!="parent"?b.appendTo:this.currentItem[0].parentNode)[0].appendChild(a[0]);if(a[0]==this.currentItem[0])this._storedCSS={width:this.currentItem[0].style.width,height:this.currentItem[0].style.height,position:this.currentItem.css("position"),top:this.currentItem.css("top"),left:this.currentItem.css("left")};if(a[0].style.width==
""||b.forceHelperSize)a.width(this.currentItem.width());if(a[0].style.height==""||b.forceHelperSize)a.height(this.currentItem.height());return a},_adjustOffsetFromHelper:function(a){if(typeof a=="string")a=a.split(" ");if(d.isArray(a))a={left:+a[0],top:+a[1]||0};if("left"in a)this.offset.click.left=a.left+this.margins.left;if("right"in a)this.offset.click.left=this.helperProportions.width-a.right+this.margins.left;if("top"in a)this.offset.click.top=a.top+this.margins.top;if("bottom"in a)this.offset.click.top=
this.helperProportions.height-a.bottom+this.margins.top},_getParentOffset:function(){this.offsetParent=this.helper.offsetParent();var a=this.offsetParent.offset();if(this.cssPosition=="absolute"&&this.scrollParent[0]!=document&&d.ui.contains(this.scrollParent[0],this.offsetParent[0])){a.left+=this.scrollParent.scrollLeft();a.top+=this.scrollParent.scrollTop()}if(this.offsetParent[0]==document.body||this.offsetParent[0].tagName&&this.offsetParent[0].tagName.toLowerCase()=="html"&&d.browser.msie)a=
{top:0,left:0};return{top:a.top+(parseInt(this.offsetParent.css("borderTopWidth"),10)||0),left:a.left+(parseInt(this.offsetParent.css("borderLeftWidth"),10)||0)}},_getRelativeOffset:function(){if(this.cssPosition=="relative"){var a=this.currentItem.position();return{top:a.top-(parseInt(this.helper.css("top"),10)||0)+this.scrollParent.scrollTop(),left:a.left-(parseInt(this.helper.css("left"),10)||0)+this.scrollParent.scrollLeft()}}else return{top:0,left:0}},_cacheMargins:function(){this.margins={left:parseInt(this.currentItem.css("marginLeft"),
10)||0,top:parseInt(this.currentItem.css("marginTop"),10)||0}},_cacheHelperProportions:function(){this.helperProportions={width:this.helper.outerWidth(),height:this.helper.outerHeight()}},_setContainment:function(){var a=this.options;if(a.containment=="parent")a.containment=this.helper[0].parentNode;if(a.containment=="document"||a.containment=="window")this.containment=[0-this.offset.relative.left-this.offset.parent.left,0-this.offset.relative.top-this.offset.parent.top,d(a.containment=="document"?
document:window).width()-this.helperProportions.width-this.margins.left,(d(a.containment=="document"?document:window).height()||document.body.parentNode.scrollHeight)-this.helperProportions.height-this.margins.top];if(!/^(document|window|parent)$/.test(a.containment)){var b=d(a.containment)[0];a=d(a.containment).offset();var c=d(b).css("overflow")!="hidden";this.containment=[a.left+(parseInt(d(b).css("borderLeftWidth"),10)||0)+(parseInt(d(b).css("paddingLeft"),10)||0)-this.margins.left,a.top+(parseInt(d(b).css("borderTopWidth"),
10)||0)+(parseInt(d(b).css("paddingTop"),10)||0)-this.margins.top,a.left+(c?Math.max(b.scrollWidth,b.offsetWidth):b.offsetWidth)-(parseInt(d(b).css("borderLeftWidth"),10)||0)-(parseInt(d(b).css("paddingRight"),10)||0)-this.helperProportions.width-this.margins.left,a.top+(c?Math.max(b.scrollHeight,b.offsetHeight):b.offsetHeight)-(parseInt(d(b).css("borderTopWidth"),10)||0)-(parseInt(d(b).css("paddingBottom"),10)||0)-this.helperProportions.height-this.margins.top]}},_convertPositionTo:function(a,b){if(!b)b=
this.position;a=a=="absolute"?1:-1;var c=this.cssPosition=="absolute"&&!(this.scrollParent[0]!=document&&d.ui.contains(this.scrollParent[0],this.offsetParent[0]))?this.offsetParent:this.scrollParent,e=/(html|body)/i.test(c[0].tagName);return{top:b.top+this.offset.relative.top*a+this.offset.parent.top*a-(d.browser.safari&&this.cssPosition=="fixed"?0:(this.cssPosition=="fixed"?-this.scrollParent.scrollTop():e?0:c.scrollTop())*a),left:b.left+this.offset.relative.left*a+this.offset.parent.left*a-(d.browser.safari&&
this.cssPosition=="fixed"?0:(this.cssPosition=="fixed"?-this.scrollParent.scrollLeft():e?0:c.scrollLeft())*a)}},_generatePosition:function(a){var b=this.options,c=this.cssPosition=="absolute"&&!(this.scrollParent[0]!=document&&d.ui.contains(this.scrollParent[0],this.offsetParent[0]))?this.offsetParent:this.scrollParent,e=/(html|body)/i.test(c[0].tagName);if(this.cssPosition=="relative"&&!(this.scrollParent[0]!=document&&this.scrollParent[0]!=this.offsetParent[0]))this.offset.relative=this._getRelativeOffset();
var f=a.pageX,g=a.pageY;if(this.originalPosition){if(this.containment){if(a.pageX-this.offset.click.left<this.containment[0])f=this.containment[0]+this.offset.click.left;if(a.pageY-this.offset.click.top<this.containment[1])g=this.containment[1]+this.offset.click.top;if(a.pageX-this.offset.click.left>this.containment[2])f=this.containment[2]+this.offset.click.left;if(a.pageY-this.offset.click.top>this.containment[3])g=this.containment[3]+this.offset.click.top}if(b.grid){g=this.originalPageY+Math.round((g-
this.originalPageY)/b.grid[1])*b.grid[1];g=this.containment?!(g-this.offset.click.top<this.containment[1]||g-this.offset.click.top>this.containment[3])?g:!(g-this.offset.click.top<this.containment[1])?g-b.grid[1]:g+b.grid[1]:g;f=this.originalPageX+Math.round((f-this.originalPageX)/b.grid[0])*b.grid[0];f=this.containment?!(f-this.offset.click.left<this.containment[0]||f-this.offset.click.left>this.containment[2])?f:!(f-this.offset.click.left<this.containment[0])?f-b.grid[0]:f+b.grid[0]:f}}return{top:g-
this.offset.click.top-this.offset.relative.top-this.offset.parent.top+(d.browser.safari&&this.cssPosition=="fixed"?0:this.cssPosition=="fixed"?-this.scrollParent.scrollTop():e?0:c.scrollTop()),left:f-this.offset.click.left-this.offset.relative.left-this.offset.parent.left+(d.browser.safari&&this.cssPosition=="fixed"?0:this.cssPosition=="fixed"?-this.scrollParent.scrollLeft():e?0:c.scrollLeft())}},_rearrange:function(a,b,c,e){c?c[0].appendChild(this.placeholder[0]):b.item[0].parentNode.insertBefore(this.placeholder[0],
this.direction=="down"?b.item[0]:b.item[0].nextSibling);this.counter=this.counter?++this.counter:1;var f=this,g=this.counter;window.setTimeout(function(){g==f.counter&&f.refreshPositions(!e)},0)},_clear:function(a,b){this.reverting=false;var c=[];!this._noFinalSort&&this.currentItem[0].parentNode&&this.placeholder.before(this.currentItem);this._noFinalSort=null;if(this.helper[0]==this.currentItem[0]){for(var e in this._storedCSS)if(this._storedCSS[e]=="auto"||this._storedCSS[e]=="static")this._storedCSS[e]=
"";this.currentItem.css(this._storedCSS).removeClass("ui-sortable-helper")}else this.currentItem.show();this.fromOutside&&!b&&c.push(function(f){this._trigger("receive",f,this._uiHash(this.fromOutside))});if((this.fromOutside||this.domPosition.prev!=this.currentItem.prev().not(".ui-sortable-helper")[0]||this.domPosition.parent!=this.currentItem.parent()[0])&&!b)c.push(function(f){this._trigger("update",f,this._uiHash())});if(!d.ui.contains(this.element[0],this.currentItem[0])){b||c.push(function(f){this._trigger("remove",
f,this._uiHash())});for(e=this.containers.length-1;e>=0;e--)if(d.ui.contains(this.containers[e].element[0],this.currentItem[0])&&!b){c.push(function(f){return function(g){f._trigger("receive",g,this._uiHash(this))}}.call(this,this.containers[e]));c.push(function(f){return function(g){f._trigger("update",g,this._uiHash(this))}}.call(this,this.containers[e]))}}for(e=this.containers.length-1;e>=0;e--){b||c.push(function(f){return function(g){f._trigger("deactivate",g,this._uiHash(this))}}.call(this,
this.containers[e]));if(this.containers[e].containerCache.over){c.push(function(f){return function(g){f._trigger("out",g,this._uiHash(this))}}.call(this,this.containers[e]));this.containers[e].containerCache.over=0}}this._storedCursor&&d("body").css("cursor",this._storedCursor);this._storedOpacity&&this.helper.css("opacity",this._storedOpacity);if(this._storedZIndex)this.helper.css("zIndex",this._storedZIndex=="auto"?"":this._storedZIndex);this.dragging=false;if(this.cancelHelperRemoval){if(!b){this._trigger("beforeStop",
a,this._uiHash());for(e=0;e<c.length;e++)c[e].call(this,a);this._trigger("stop",a,this._uiHash())}return false}b||this._trigger("beforeStop",a,this._uiHash());this.placeholder[0].parentNode.removeChild(this.placeholder[0]);this.helper[0]!=this.currentItem[0]&&this.helper.remove();this.helper=null;if(!b){for(e=0;e<c.length;e++)c[e].call(this,a);this._trigger("stop",a,this._uiHash())}this.fromOutside=false;return true},_trigger:function(){d.Widget.prototype._trigger.apply(this,arguments)===false&&this.cancel()},
_uiHash:function(a){var b=a||this;return{helper:b.helper,placeholder:b.placeholder||d([]),position:b.position,originalPosition:b.originalPosition,offset:b.positionAbs,item:b.currentItem,sender:a?a.element:null}}});d.extend(d.ui.sortable,{version:"1.8.13"})})(jQuery);
;/*
 * jQuery UI Accordion 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Accordion
 *
 * Depends:
 *	jquery.ui.core.js
 *	jquery.ui.widget.js
 */
(function(c){c.widget("ui.accordion",{options:{active:0,animated:"slide",autoHeight:true,clearStyle:false,collapsible:false,event:"click",fillSpace:false,header:"> li > :first-child,> :not(li):even",icons:{header:"ui-icon-triangle-1-e",headerSelected:"ui-icon-triangle-1-s"},navigation:false,navigationFilter:function(){return this.href.toLowerCase()===location.href.toLowerCase()}},_create:function(){var a=this,b=a.options;a.running=0;a.element.addClass("ui-accordion ui-widget ui-helper-reset").children("li").addClass("ui-accordion-li-fix");
a.headers=a.element.find(b.header).addClass("ui-accordion-header ui-helper-reset ui-state-default ui-corner-all").bind("mouseenter.accordion",function(){b.disabled||c(this).addClass("ui-state-hover")}).bind("mouseleave.accordion",function(){b.disabled||c(this).removeClass("ui-state-hover")}).bind("focus.accordion",function(){b.disabled||c(this).addClass("ui-state-focus")}).bind("blur.accordion",function(){b.disabled||c(this).removeClass("ui-state-focus")});a.headers.next().addClass("ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom");
if(b.navigation){var d=a.element.find("a").filter(b.navigationFilter).eq(0);if(d.length){var h=d.closest(".ui-accordion-header");a.active=h.length?h:d.closest(".ui-accordion-content").prev()}}a.active=a._findActive(a.active||b.active).addClass("ui-state-default ui-state-active").toggleClass("ui-corner-all").toggleClass("ui-corner-top");a.active.next().addClass("ui-accordion-content-active");a._createIcons();a.resize();a.element.attr("role","tablist");a.headers.attr("role","tab").bind("keydown.accordion",
function(f){return a._keydown(f)}).next().attr("role","tabpanel");a.headers.not(a.active||"").attr({"aria-expanded":"false","aria-selected":"false",tabIndex:-1}).next().hide();a.active.length?a.active.attr({"aria-expanded":"true","aria-selected":"true",tabIndex:0}):a.headers.eq(0).attr("tabIndex",0);c.browser.safari||a.headers.find("a").attr("tabIndex",-1);b.event&&a.headers.bind(b.event.split(" ").join(".accordion ")+".accordion",function(f){a._clickHandler.call(a,f,this);f.preventDefault()})},_createIcons:function(){var a=
this.options;if(a.icons){c("<span></span>").addClass("ui-icon "+a.icons.header).prependTo(this.headers);this.active.children(".ui-icon").toggleClass(a.icons.header).toggleClass(a.icons.headerSelected);this.element.addClass("ui-accordion-icons")}},_destroyIcons:function(){this.headers.children(".ui-icon").remove();this.element.removeClass("ui-accordion-icons")},destroy:function(){var a=this.options;this.element.removeClass("ui-accordion ui-widget ui-helper-reset").removeAttr("role");this.headers.unbind(".accordion").removeClass("ui-accordion-header ui-accordion-disabled ui-helper-reset ui-state-default ui-corner-all ui-state-active ui-state-disabled ui-corner-top").removeAttr("role").removeAttr("aria-expanded").removeAttr("aria-selected").removeAttr("tabIndex");
this.headers.find("a").removeAttr("tabIndex");this._destroyIcons();var b=this.headers.next().css("display","").removeAttr("role").removeClass("ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content ui-accordion-content-active ui-accordion-disabled ui-state-disabled");if(a.autoHeight||a.fillHeight)b.css("height","");return c.Widget.prototype.destroy.call(this)},_setOption:function(a,b){c.Widget.prototype._setOption.apply(this,arguments);a=="active"&&this.activate(b);if(a=="icons"){this._destroyIcons();
b&&this._createIcons()}if(a=="disabled")this.headers.add(this.headers.next())[b?"addClass":"removeClass"]("ui-accordion-disabled ui-state-disabled")},_keydown:function(a){if(!(this.options.disabled||a.altKey||a.ctrlKey)){var b=c.ui.keyCode,d=this.headers.length,h=this.headers.index(a.target),f=false;switch(a.keyCode){case b.RIGHT:case b.DOWN:f=this.headers[(h+1)%d];break;case b.LEFT:case b.UP:f=this.headers[(h-1+d)%d];break;case b.SPACE:case b.ENTER:this._clickHandler({target:a.target},a.target);
a.preventDefault()}if(f){c(a.target).attr("tabIndex",-1);c(f).attr("tabIndex",0);f.focus();return false}return true}},resize:function(){var a=this.options,b;if(a.fillSpace){if(c.browser.msie){var d=this.element.parent().css("overflow");this.element.parent().css("overflow","hidden")}b=this.element.parent().height();c.browser.msie&&this.element.parent().css("overflow",d);this.headers.each(function(){b-=c(this).outerHeight(true)});this.headers.next().each(function(){c(this).height(Math.max(0,b-c(this).innerHeight()+
c(this).height()))}).css("overflow","auto")}else if(a.autoHeight){b=0;this.headers.next().each(function(){b=Math.max(b,c(this).height("").height())}).height(b)}return this},activate:function(a){this.options.active=a;a=this._findActive(a)[0];this._clickHandler({target:a},a);return this},_findActive:function(a){return a?typeof a==="number"?this.headers.filter(":eq("+a+")"):this.headers.not(this.headers.not(a)):a===false?c([]):this.headers.filter(":eq(0)")},_clickHandler:function(a,b){var d=this.options;
if(!d.disabled)if(a.target){a=c(a.currentTarget||b);b=a[0]===this.active[0];d.active=d.collapsible&&b?false:this.headers.index(a);if(!(this.running||!d.collapsible&&b)){var h=this.active;j=a.next();g=this.active.next();e={options:d,newHeader:b&&d.collapsible?c([]):a,oldHeader:this.active,newContent:b&&d.collapsible?c([]):j,oldContent:g};var f=this.headers.index(this.active[0])>this.headers.index(a[0]);this.active=b?c([]):a;this._toggle(j,g,e,b,f);h.removeClass("ui-state-active ui-corner-top").addClass("ui-state-default ui-corner-all").children(".ui-icon").removeClass(d.icons.headerSelected).addClass(d.icons.header);
if(!b){a.removeClass("ui-state-default ui-corner-all").addClass("ui-state-active ui-corner-top").children(".ui-icon").removeClass(d.icons.header).addClass(d.icons.headerSelected);a.next().addClass("ui-accordion-content-active")}}}else if(d.collapsible){this.active.removeClass("ui-state-active ui-corner-top").addClass("ui-state-default ui-corner-all").children(".ui-icon").removeClass(d.icons.headerSelected).addClass(d.icons.header);this.active.next().addClass("ui-accordion-content-active");var g=this.active.next(),
e={options:d,newHeader:c([]),oldHeader:d.active,newContent:c([]),oldContent:g},j=this.active=c([]);this._toggle(j,g,e)}},_toggle:function(a,b,d,h,f){var g=this,e=g.options;g.toShow=a;g.toHide=b;g.data=d;var j=function(){if(g)return g._completed.apply(g,arguments)};g._trigger("changestart",null,g.data);g.running=b.size()===0?a.size():b.size();if(e.animated){d={};d=e.collapsible&&h?{toShow:c([]),toHide:b,complete:j,down:f,autoHeight:e.autoHeight||e.fillSpace}:{toShow:a,toHide:b,complete:j,down:f,autoHeight:e.autoHeight||
e.fillSpace};if(!e.proxied)e.proxied=e.animated;if(!e.proxiedDuration)e.proxiedDuration=e.duration;e.animated=c.isFunction(e.proxied)?e.proxied(d):e.proxied;e.duration=c.isFunction(e.proxiedDuration)?e.proxiedDuration(d):e.proxiedDuration;h=c.ui.accordion.animations;var i=e.duration,k=e.animated;if(k&&!h[k]&&!c.easing[k])k="slide";h[k]||(h[k]=function(l){this.slide(l,{easing:k,duration:i||700})});h[k](d)}else{if(e.collapsible&&h)a.toggle();else{b.hide();a.show()}j(true)}b.prev().attr({"aria-expanded":"false",
"aria-selected":"false",tabIndex:-1}).blur();a.prev().attr({"aria-expanded":"true","aria-selected":"true",tabIndex:0}).focus()},_completed:function(a){this.running=a?0:--this.running;if(!this.running){this.options.clearStyle&&this.toShow.add(this.toHide).css({height:"",overflow:""});this.toHide.removeClass("ui-accordion-content-active");if(this.toHide.length)this.toHide.parent()[0].className=this.toHide.parent()[0].className;this._trigger("change",null,this.data)}}});c.extend(c.ui.accordion,{version:"1.8.13",
animations:{slide:function(a,b){a=c.extend({easing:"swing",duration:300},a,b);if(a.toHide.size())if(a.toShow.size()){var d=a.toShow.css("overflow"),h=0,f={},g={},e;b=a.toShow;e=b[0].style.width;b.width(parseInt(b.parent().width(),10)-parseInt(b.css("paddingLeft"),10)-parseInt(b.css("paddingRight"),10)-(parseInt(b.css("borderLeftWidth"),10)||0)-(parseInt(b.css("borderRightWidth"),10)||0));c.each(["height","paddingTop","paddingBottom"],function(j,i){g[i]="hide";j=(""+c.css(a.toShow[0],i)).match(/^([\d+-.]+)(.*)$/);
f[i]={value:j[1],unit:j[2]||"px"}});a.toShow.css({height:0,overflow:"hidden"}).show();a.toHide.filter(":hidden").each(a.complete).end().filter(":visible").animate(g,{step:function(j,i){if(i.prop=="height")h=i.end-i.start===0?0:(i.now-i.start)/(i.end-i.start);a.toShow[0].style[i.prop]=h*f[i.prop].value+f[i.prop].unit},duration:a.duration,easing:a.easing,complete:function(){a.autoHeight||a.toShow.css("height","");a.toShow.css({width:e,overflow:d});a.complete()}})}else a.toHide.animate({height:"hide",
paddingTop:"hide",paddingBottom:"hide"},a);else a.toShow.animate({height:"show",paddingTop:"show",paddingBottom:"show"},a)},bounceslide:function(a){this.slide(a,{easing:a.down?"easeOutBounce":"swing",duration:a.down?1E3:200})}}})})(jQuery);
;/*
 * jQuery UI Autocomplete 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Autocomplete
 *
 * Depends:
 *	jquery.ui.core.js
 *	jquery.ui.widget.js
 *	jquery.ui.position.js
 */
(function(d){var e=0;d.widget("ui.autocomplete",{options:{appendTo:"body",autoFocus:false,delay:300,minLength:1,position:{my:"left top",at:"left bottom",collision:"none"},source:null},pending:0,_create:function(){var a=this,b=this.element[0].ownerDocument,g;this.element.addClass("ui-autocomplete-input").attr("autocomplete","off").attr({role:"textbox","aria-autocomplete":"list","aria-haspopup":"true"}).bind("keydown.autocomplete",function(c){if(!(a.options.disabled||a.element.attr("readonly"))){g=
false;var f=d.ui.keyCode;switch(c.keyCode){case f.PAGE_UP:a._move("previousPage",c);break;case f.PAGE_DOWN:a._move("nextPage",c);break;case f.UP:a._move("previous",c);c.preventDefault();break;case f.DOWN:a._move("next",c);c.preventDefault();break;case f.ENTER:case f.NUMPAD_ENTER:if(a.menu.active){g=true;c.preventDefault()}case f.TAB:if(!a.menu.active)return;a.menu.select(c);break;case f.ESCAPE:a.element.val(a.term);a.close(c);break;default:clearTimeout(a.searching);a.searching=setTimeout(function(){if(a.term!=
a.element.val()){a.selectedItem=null;a.search(null,c)}},a.options.delay);break}}}).bind("keypress.autocomplete",function(c){if(g){g=false;c.preventDefault()}}).bind("focus.autocomplete",function(){if(!a.options.disabled){a.selectedItem=null;a.previous=a.element.val()}}).bind("blur.autocomplete",function(c){if(!a.options.disabled){clearTimeout(a.searching);a.closing=setTimeout(function(){a.close(c);a._change(c)},150)}});this._initSource();this.response=function(){return a._response.apply(a,arguments)};
this.menu=d("<ul></ul>").addClass("ui-autocomplete").appendTo(d(this.options.appendTo||"body",b)[0]).mousedown(function(c){var f=a.menu.element[0];d(c.target).closest(".ui-menu-item").length||setTimeout(function(){d(document).one("mousedown",function(h){h.target!==a.element[0]&&h.target!==f&&!d.ui.contains(f,h.target)&&a.close()})},1);setTimeout(function(){clearTimeout(a.closing)},13)}).menu({focus:function(c,f){f=f.item.data("item.autocomplete");false!==a._trigger("focus",c,{item:f})&&/^key/.test(c.originalEvent.type)&&
a.element.val(f.value)},selected:function(c,f){var h=f.item.data("item.autocomplete"),i=a.previous;if(a.element[0]!==b.activeElement){a.element.focus();a.previous=i;setTimeout(function(){a.previous=i;a.selectedItem=h},1)}false!==a._trigger("select",c,{item:h})&&a.element.val(h.value);a.term=a.element.val();a.close(c);a.selectedItem=h},blur:function(){a.menu.element.is(":visible")&&a.element.val()!==a.term&&a.element.val(a.term)}}).zIndex(this.element.zIndex()+1).css({top:0,left:0}).hide().data("menu");
d.fn.bgiframe&&this.menu.element.bgiframe()},destroy:function(){this.element.removeClass("ui-autocomplete-input").removeAttr("autocomplete").removeAttr("role").removeAttr("aria-autocomplete").removeAttr("aria-haspopup");this.menu.element.remove();d.Widget.prototype.destroy.call(this)},_setOption:function(a,b){d.Widget.prototype._setOption.apply(this,arguments);a==="source"&&this._initSource();if(a==="appendTo")this.menu.element.appendTo(d(b||"body",this.element[0].ownerDocument)[0]);a==="disabled"&&
b&&this.xhr&&this.xhr.abort()},_initSource:function(){var a=this,b,g;if(d.isArray(this.options.source)){b=this.options.source;this.source=function(c,f){f(d.ui.autocomplete.filter(b,c.term))}}else if(typeof this.options.source==="string"){g=this.options.source;this.source=function(c,f){a.xhr&&a.xhr.abort();a.xhr=d.ajax({url:g,data:c,dataType:"json",autocompleteRequest:++e,success:function(h){this.autocompleteRequest===e&&f(h)},error:function(){this.autocompleteRequest===e&&f([])}})}}else this.source=
this.options.source},search:function(a,b){a=a!=null?a:this.element.val();this.term=this.element.val();if(a.length<this.options.minLength)return this.close(b);clearTimeout(this.closing);if(this._trigger("search",b)!==false)return this._search(a)},_search:function(a){this.pending++;this.element.addClass("ui-autocomplete-loading");this.source({term:a},this.response)},_response:function(a){if(!this.options.disabled&&a&&a.length){a=this._normalize(a);this._suggest(a);this._trigger("open")}else this.close();
this.pending--;this.pending||this.element.removeClass("ui-autocomplete-loading")},close:function(a){clearTimeout(this.closing);if(this.menu.element.is(":visible")){this.menu.element.hide();this.menu.deactivate();this._trigger("close",a)}},_change:function(a){this.previous!==this.element.val()&&this._trigger("change",a,{item:this.selectedItem})},_normalize:function(a){if(a.length&&a[0].label&&a[0].value)return a;return d.map(a,function(b){if(typeof b==="string")return{label:b,value:b};return d.extend({label:b.label||
b.value,value:b.value||b.label},b)})},_suggest:function(a){var b=this.menu.element.empty().zIndex(this.element.zIndex()+1);this._renderMenu(b,a);this.menu.deactivate();this.menu.refresh();b.show();this._resizeMenu();b.position(d.extend({of:this.element},this.options.position));this.options.autoFocus&&this.menu.next(new d.Event("mouseover"))},_resizeMenu:function(){var a=this.menu.element;a.outerWidth(Math.max(a.width("").outerWidth(),this.element.outerWidth()))},_renderMenu:function(a,b){var g=this;
d.each(b,function(c,f){g._renderItem(a,f)})},_renderItem:function(a,b){return d("<li></li>").data("item.autocomplete",b).append(d("<a></a>").text(b.label)).appendTo(a)},_move:function(a,b){if(this.menu.element.is(":visible"))if(this.menu.first()&&/^previous/.test(a)||this.menu.last()&&/^next/.test(a)){this.element.val(this.term);this.menu.deactivate()}else this.menu[a](b);else this.search(null,b)},widget:function(){return this.menu.element}});d.extend(d.ui.autocomplete,{escapeRegex:function(a){return a.replace(/[-[\]{}()*+?.,\\^$|#\s]/g,
"\\$&")},filter:function(a,b){var g=new RegExp(d.ui.autocomplete.escapeRegex(b),"i");return d.grep(a,function(c){return g.test(c.label||c.value||c)})}})})(jQuery);
(function(d){d.widget("ui.menu",{_create:function(){var e=this;this.element.addClass("ui-menu ui-widget ui-widget-content ui-corner-all").attr({role:"listbox","aria-activedescendant":"ui-active-menuitem"}).click(function(a){if(d(a.target).closest(".ui-menu-item a").length){a.preventDefault();e.select(a)}});this.refresh()},refresh:function(){var e=this;this.element.children("li:not(.ui-menu-item):has(a)").addClass("ui-menu-item").attr("role","menuitem").children("a").addClass("ui-corner-all").attr("tabindex",
-1).mouseenter(function(a){e.activate(a,d(this).parent())}).mouseleave(function(){e.deactivate()})},activate:function(e,a){this.deactivate();if(this.hasScroll()){var b=a.offset().top-this.element.offset().top,g=this.element.scrollTop(),c=this.element.height();if(b<0)this.element.scrollTop(g+b);else b>=c&&this.element.scrollTop(g+b-c+a.height())}this.active=a.eq(0).children("a").addClass("ui-state-hover").attr("id","ui-active-menuitem").end();this._trigger("focus",e,{item:a})},deactivate:function(){if(this.active){this.active.children("a").removeClass("ui-state-hover").removeAttr("id");
this._trigger("blur");this.active=null}},next:function(e){this.move("next",".ui-menu-item:first",e)},previous:function(e){this.move("prev",".ui-menu-item:last",e)},first:function(){return this.active&&!this.active.prevAll(".ui-menu-item").length},last:function(){return this.active&&!this.active.nextAll(".ui-menu-item").length},move:function(e,a,b){if(this.active){e=this.active[e+"All"](".ui-menu-item").eq(0);e.length?this.activate(b,e):this.activate(b,this.element.children(a))}else this.activate(b,
this.element.children(a))},nextPage:function(e){if(this.hasScroll())if(!this.active||this.last())this.activate(e,this.element.children(".ui-menu-item:first"));else{var a=this.active.offset().top,b=this.element.height(),g=this.element.children(".ui-menu-item").filter(function(){var c=d(this).offset().top-a-b+d(this).height();return c<10&&c>-10});g.length||(g=this.element.children(".ui-menu-item:last"));this.activate(e,g)}else this.activate(e,this.element.children(".ui-menu-item").filter(!this.active||
this.last()?":first":":last"))},previousPage:function(e){if(this.hasScroll())if(!this.active||this.first())this.activate(e,this.element.children(".ui-menu-item:last"));else{var a=this.active.offset().top,b=this.element.height();result=this.element.children(".ui-menu-item").filter(function(){var g=d(this).offset().top-a+b-d(this).height();return g<10&&g>-10});result.length||(result=this.element.children(".ui-menu-item:first"));this.activate(e,result)}else this.activate(e,this.element.children(".ui-menu-item").filter(!this.active||
this.first()?":last":":first"))},hasScroll:function(){return this.element.height()<this.element[d.fn.prop?"prop":"attr"]("scrollHeight")},select:function(e){this._trigger("selected",e,{item:this.active})}})})(jQuery);
;/*
 * jQuery UI Button 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Button
 *
 * Depends:
 *	jquery.ui.core.js
 *	jquery.ui.widget.js
 */
(function(a){var g,i=function(b){a(":ui-button",b.target.form).each(function(){var c=a(this).data("button");setTimeout(function(){c.refresh()},1)})},h=function(b){var c=b.name,d=b.form,f=a([]);if(c)f=d?a(d).find("[name='"+c+"']"):a("[name='"+c+"']",b.ownerDocument).filter(function(){return!this.form});return f};a.widget("ui.button",{options:{disabled:null,text:true,label:null,icons:{primary:null,secondary:null}},_create:function(){this.element.closest("form").unbind("reset.button").bind("reset.button",
i);if(typeof this.options.disabled!=="boolean")this.options.disabled=this.element.attr("disabled");this._determineButtonType();this.hasTitle=!!this.buttonElement.attr("title");var b=this,c=this.options,d=this.type==="checkbox"||this.type==="radio",f="ui-state-hover"+(!d?" ui-state-active":"");if(c.label===null)c.label=this.buttonElement.html();if(this.element.is(":disabled"))c.disabled=true;this.buttonElement.addClass("ui-button ui-widget ui-state-default ui-corner-all").attr("role","button").bind("mouseenter.button",
function(){if(!c.disabled){a(this).addClass("ui-state-hover");this===g&&a(this).addClass("ui-state-active")}}).bind("mouseleave.button",function(){c.disabled||a(this).removeClass(f)}).bind("focus.button",function(){a(this).addClass("ui-state-focus")}).bind("blur.button",function(){a(this).removeClass("ui-state-focus")}).bind("click.button",function(e){c.disabled&&e.stopImmediatePropagation()});d&&this.element.bind("change.button",function(){b.refresh()});if(this.type==="checkbox")this.buttonElement.bind("click.button",
function(){if(c.disabled)return false;a(this).toggleClass("ui-state-active");b.buttonElement.attr("aria-pressed",b.element[0].checked)});else if(this.type==="radio")this.buttonElement.bind("click.button",function(){if(c.disabled)return false;a(this).addClass("ui-state-active");b.buttonElement.attr("aria-pressed",true);var e=b.element[0];h(e).not(e).map(function(){return a(this).button("widget")[0]}).removeClass("ui-state-active").attr("aria-pressed",false)});else{this.buttonElement.bind("mousedown.button",
function(){if(c.disabled)return false;a(this).addClass("ui-state-active");g=this;a(document).one("mouseup",function(){g=null})}).bind("mouseup.button",function(){if(c.disabled)return false;a(this).removeClass("ui-state-active")}).bind("keydown.button",function(e){if(c.disabled)return false;if(e.keyCode==a.ui.keyCode.SPACE||e.keyCode==a.ui.keyCode.ENTER)a(this).addClass("ui-state-active")}).bind("keyup.button",function(){a(this).removeClass("ui-state-active")});this.buttonElement.is("a")&&this.buttonElement.keyup(function(e){e.keyCode===
a.ui.keyCode.SPACE&&a(this).click()})}this._setOption("disabled",c.disabled)},_determineButtonType:function(){this.type=this.element.is(":checkbox")?"checkbox":this.element.is(":radio")?"radio":this.element.is("input")?"input":"button";if(this.type==="checkbox"||this.type==="radio"){var b=this.element.parents().filter(":last"),c="label[for="+this.element.attr("id")+"]";this.buttonElement=b.find(c);if(!this.buttonElement.length){b=b.length?b.siblings():this.element.siblings();this.buttonElement=b.filter(c);
if(!this.buttonElement.length)this.buttonElement=b.find(c)}this.element.addClass("ui-helper-hidden-accessible");(b=this.element.is(":checked"))&&this.buttonElement.addClass("ui-state-active");this.buttonElement.attr("aria-pressed",b)}else this.buttonElement=this.element},widget:function(){return this.buttonElement},destroy:function(){this.element.removeClass("ui-helper-hidden-accessible");this.buttonElement.removeClass("ui-button ui-widget ui-state-default ui-corner-all ui-state-hover ui-state-active  ui-button-icons-only ui-button-icon-only ui-button-text-icons ui-button-text-icon-primary ui-button-text-icon-secondary ui-button-text-only").removeAttr("role").removeAttr("aria-pressed").html(this.buttonElement.find(".ui-button-text").html());
this.hasTitle||this.buttonElement.removeAttr("title");a.Widget.prototype.destroy.call(this)},_setOption:function(b,c){a.Widget.prototype._setOption.apply(this,arguments);if(b==="disabled")c?this.element.attr("disabled",true):this.element.removeAttr("disabled");this._resetButton()},refresh:function(){var b=this.element.is(":disabled");b!==this.options.disabled&&this._setOption("disabled",b);if(this.type==="radio")h(this.element[0]).each(function(){a(this).is(":checked")?a(this).button("widget").addClass("ui-state-active").attr("aria-pressed",
true):a(this).button("widget").removeClass("ui-state-active").attr("aria-pressed",false)});else if(this.type==="checkbox")this.element.is(":checked")?this.buttonElement.addClass("ui-state-active").attr("aria-pressed",true):this.buttonElement.removeClass("ui-state-active").attr("aria-pressed",false)},_resetButton:function(){if(this.type==="input")this.options.label&&this.element.val(this.options.label);else{var b=this.buttonElement.removeClass("ui-button-icons-only ui-button-icon-only ui-button-text-icons ui-button-text-icon-primary ui-button-text-icon-secondary ui-button-text-only"),
c=a("<span></span>").addClass("ui-button-text").html(this.options.label).appendTo(b.empty()).text(),d=this.options.icons,f=d.primary&&d.secondary,e=[];if(d.primary||d.secondary){if(this.options.text)e.push("ui-button-text-icon"+(f?"s":d.primary?"-primary":"-secondary"));d.primary&&b.prepend("<span class='ui-button-icon-primary ui-icon "+d.primary+"'></span>");d.secondary&&b.append("<span class='ui-button-icon-secondary ui-icon "+d.secondary+"'></span>");if(!this.options.text){e.push(f?"ui-button-icons-only":
"ui-button-icon-only");this.hasTitle||b.attr("title",c)}}else e.push("ui-button-text-only");b.addClass(e.join(" "))}}});a.widget("ui.buttonset",{options:{items:":button, :submit, :reset, :checkbox, :radio, a, :data(button)"},_create:function(){this.element.addClass("ui-buttonset")},_init:function(){this.refresh()},_setOption:function(b,c){b==="disabled"&&this.buttons.button("option",b,c);a.Widget.prototype._setOption.apply(this,arguments)},refresh:function(){this.buttons=this.element.find(this.options.items).filter(":ui-button").button("refresh").end().not(":ui-button").button().end().map(function(){return a(this).button("widget")[0]}).removeClass("ui-corner-all ui-corner-left ui-corner-right").filter(":first").addClass("ui-corner-left").end().filter(":last").addClass("ui-corner-right").end().end()},
destroy:function(){this.element.removeClass("ui-buttonset");this.buttons.map(function(){return a(this).button("widget")[0]}).removeClass("ui-corner-left ui-corner-right").end().button("destroy");a.Widget.prototype.destroy.call(this)}})})(jQuery);
;/*
 * jQuery UI Dialog 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Dialog
 *
 * Depends:
 *	jquery.ui.core.js
 *	jquery.ui.widget.js
 *  jquery.ui.button.js
 *	jquery.ui.draggable.js
 *	jquery.ui.mouse.js
 *	jquery.ui.position.js
 *	jquery.ui.resizable.js
 */
(function(c,l){var m={buttons:true,height:true,maxHeight:true,maxWidth:true,minHeight:true,minWidth:true,width:true},n={maxHeight:true,maxWidth:true,minHeight:true,minWidth:true},o=c.attrFn||{val:true,css:true,html:true,text:true,data:true,width:true,height:true,offset:true,click:true};c.widget("ui.dialog",{options:{autoOpen:true,buttons:{},closeOnEscape:true,closeText:"close",dialogClass:"",draggable:true,hide:null,height:"auto",maxHeight:false,maxWidth:false,minHeight:150,minWidth:150,modal:false,
position:{my:"center",at:"center",collision:"fit",using:function(a){var b=c(this).css(a).offset().top;b<0&&c(this).css("top",a.top-b)}},resizable:true,show:null,stack:true,title:"",width:300,zIndex:1E3},_create:function(){this.originalTitle=this.element.attr("title");if(typeof this.originalTitle!=="string")this.originalTitle="";this.options.title=this.options.title||this.originalTitle;var a=this,b=a.options,d=b.title||"&#160;",e=c.ui.dialog.getTitleId(a.element),g=(a.uiDialog=c("<div></div>")).appendTo(document.body).hide().addClass("ui-dialog ui-widget ui-widget-content ui-corner-all "+
b.dialogClass).css({zIndex:b.zIndex}).attr("tabIndex",-1).css("outline",0).keydown(function(i){if(b.closeOnEscape&&i.keyCode&&i.keyCode===c.ui.keyCode.ESCAPE){a.close(i);i.preventDefault()}}).attr({role:"dialog","aria-labelledby":e}).mousedown(function(i){a.moveToTop(false,i)});a.element.show().removeAttr("title").addClass("ui-dialog-content ui-widget-content").appendTo(g);var f=(a.uiDialogTitlebar=c("<div></div>")).addClass("ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix").prependTo(g),
h=c('<a href="#"></a>').addClass("ui-dialog-titlebar-close ui-corner-all").attr("role","button").hover(function(){h.addClass("ui-state-hover")},function(){h.removeClass("ui-state-hover")}).focus(function(){h.addClass("ui-state-focus")}).blur(function(){h.removeClass("ui-state-focus")}).click(function(i){a.close(i);return false}).appendTo(f);(a.uiDialogTitlebarCloseText=c("<span></span>")).addClass("ui-icon ui-icon-closethick").text(b.closeText).appendTo(h);c("<span></span>").addClass("ui-dialog-title").attr("id",
e).html(d).prependTo(f);if(c.isFunction(b.beforeclose)&&!c.isFunction(b.beforeClose))b.beforeClose=b.beforeclose;f.find("*").add(f).disableSelection();b.draggable&&c.fn.draggable&&a._makeDraggable();b.resizable&&c.fn.resizable&&a._makeResizable();a._createButtons(b.buttons);a._isOpen=false;c.fn.bgiframe&&g.bgiframe()},_init:function(){this.options.autoOpen&&this.open()},destroy:function(){var a=this;a.overlay&&a.overlay.destroy();a.uiDialog.hide();a.element.unbind(".dialog").removeData("dialog").removeClass("ui-dialog-content ui-widget-content").hide().appendTo("body");
a.uiDialog.remove();a.originalTitle&&a.element.attr("title",a.originalTitle);return a},widget:function(){return this.uiDialog},close:function(a){var b=this,d,e;if(false!==b._trigger("beforeClose",a)){b.overlay&&b.overlay.destroy();b.uiDialog.unbind("keypress.ui-dialog");b._isOpen=false;if(b.options.hide)b.uiDialog.hide(b.options.hide,function(){b._trigger("close",a)});else{b.uiDialog.hide();b._trigger("close",a)}c.ui.dialog.overlay.resize();if(b.options.modal){d=0;c(".ui-dialog").each(function(){if(this!==
b.uiDialog[0]){e=c(this).css("z-index");isNaN(e)||(d=Math.max(d,e))}});c.ui.dialog.maxZ=d}return b}},isOpen:function(){return this._isOpen},moveToTop:function(a,b){var d=this,e=d.options;if(e.modal&&!a||!e.stack&&!e.modal)return d._trigger("focus",b);if(e.zIndex>c.ui.dialog.maxZ)c.ui.dialog.maxZ=e.zIndex;if(d.overlay){c.ui.dialog.maxZ+=1;d.overlay.$el.css("z-index",c.ui.dialog.overlay.maxZ=c.ui.dialog.maxZ)}a={scrollTop:d.element.attr("scrollTop"),scrollLeft:d.element.attr("scrollLeft")};c.ui.dialog.maxZ+=
1;d.uiDialog.css("z-index",c.ui.dialog.maxZ);d.element.attr(a);d._trigger("focus",b);return d},open:function(){if(!this._isOpen){var a=this,b=a.options,d=a.uiDialog;a.overlay=b.modal?new c.ui.dialog.overlay(a):null;a._size();a._position(b.position);d.show(b.show);a.moveToTop(true);b.modal&&d.bind("keypress.ui-dialog",function(e){if(e.keyCode===c.ui.keyCode.TAB){var g=c(":tabbable",this),f=g.filter(":first");g=g.filter(":last");if(e.target===g[0]&&!e.shiftKey){f.focus(1);return false}else if(e.target===
f[0]&&e.shiftKey){g.focus(1);return false}}});c(a.element.find(":tabbable").get().concat(d.find(".ui-dialog-buttonpane :tabbable").get().concat(d.get()))).eq(0).focus();a._isOpen=true;a._trigger("open");return a}},_createButtons:function(a){var b=this,d=false,e=c("<div></div>").addClass("ui-dialog-buttonpane ui-widget-content ui-helper-clearfix"),g=c("<div></div>").addClass("ui-dialog-buttonset").appendTo(e);b.uiDialog.find(".ui-dialog-buttonpane").remove();typeof a==="object"&&a!==null&&c.each(a,
function(){return!(d=true)});if(d){c.each(a,function(f,h){h=c.isFunction(h)?{click:h,text:f}:h;var i=c('<button type="button"></button>').click(function(){h.click.apply(b.element[0],arguments)}).appendTo(g);c.each(h,function(j,k){if(j!=="click")j in o?i[j](k):i.attr(j,k)});c.fn.button&&i.button()});e.appendTo(b.uiDialog)}},_makeDraggable:function(){function a(f){return{position:f.position,offset:f.offset}}var b=this,d=b.options,e=c(document),g;b.uiDialog.draggable({cancel:".ui-dialog-content, .ui-dialog-titlebar-close",
handle:".ui-dialog-titlebar",containment:"document",start:function(f,h){g=d.height==="auto"?"auto":c(this).height();c(this).height(c(this).height()).addClass("ui-dialog-dragging");b._trigger("dragStart",f,a(h))},drag:function(f,h){b._trigger("drag",f,a(h))},stop:function(f,h){d.position=[h.position.left-e.scrollLeft(),h.position.top-e.scrollTop()];c(this).removeClass("ui-dialog-dragging").height(g);b._trigger("dragStop",f,a(h));c.ui.dialog.overlay.resize()}})},_makeResizable:function(a){function b(f){return{originalPosition:f.originalPosition,
originalSize:f.originalSize,position:f.position,size:f.size}}a=a===l?this.options.resizable:a;var d=this,e=d.options,g=d.uiDialog.css("position");a=typeof a==="string"?a:"n,e,s,w,se,sw,ne,nw";d.uiDialog.resizable({cancel:".ui-dialog-content",containment:"document",alsoResize:d.element,maxWidth:e.maxWidth,maxHeight:e.maxHeight,minWidth:e.minWidth,minHeight:d._minHeight(),handles:a,start:function(f,h){c(this).addClass("ui-dialog-resizing");d._trigger("resizeStart",f,b(h))},resize:function(f,h){d._trigger("resize",
f,b(h))},stop:function(f,h){c(this).removeClass("ui-dialog-resizing");e.height=c(this).height();e.width=c(this).width();d._trigger("resizeStop",f,b(h));c.ui.dialog.overlay.resize()}}).css("position",g).find(".ui-resizable-se").addClass("ui-icon ui-icon-grip-diagonal-se")},_minHeight:function(){var a=this.options;return a.height==="auto"?a.minHeight:Math.min(a.minHeight,a.height)},_position:function(a){var b=[],d=[0,0],e;if(a){if(typeof a==="string"||typeof a==="object"&&"0"in a){b=a.split?a.split(" "):
[a[0],a[1]];if(b.length===1)b[1]=b[0];c.each(["left","top"],function(g,f){if(+b[g]===b[g]){d[g]=b[g];b[g]=f}});a={my:b.join(" "),at:b.join(" "),offset:d.join(" ")}}a=c.extend({},c.ui.dialog.prototype.options.position,a)}else a=c.ui.dialog.prototype.options.position;(e=this.uiDialog.is(":visible"))||this.uiDialog.show();this.uiDialog.css({top:0,left:0}).position(c.extend({of:window},a));e||this.uiDialog.hide()},_setOptions:function(a){var b=this,d={},e=false;c.each(a,function(g,f){b._setOption(g,f);
if(g in m)e=true;if(g in n)d[g]=f});e&&this._size();this.uiDialog.is(":data(resizable)")&&this.uiDialog.resizable("option",d)},_setOption:function(a,b){var d=this,e=d.uiDialog;switch(a){case "beforeclose":a="beforeClose";break;case "buttons":d._createButtons(b);break;case "closeText":d.uiDialogTitlebarCloseText.text(""+b);break;case "dialogClass":e.removeClass(d.options.dialogClass).addClass("ui-dialog ui-widget ui-widget-content ui-corner-all "+b);break;case "disabled":b?e.addClass("ui-dialog-disabled"):
e.removeClass("ui-dialog-disabled");break;case "draggable":var g=e.is(":data(draggable)");g&&!b&&e.draggable("destroy");!g&&b&&d._makeDraggable();break;case "position":d._position(b);break;case "resizable":(g=e.is(":data(resizable)"))&&!b&&e.resizable("destroy");g&&typeof b==="string"&&e.resizable("option","handles",b);!g&&b!==false&&d._makeResizable(b);break;case "title":c(".ui-dialog-title",d.uiDialogTitlebar).html(""+(b||"&#160;"));break}c.Widget.prototype._setOption.apply(d,arguments)},_size:function(){var a=
this.options,b,d,e=this.uiDialog.is(":visible");this.element.show().css({width:"auto",minHeight:0,height:0});if(a.minWidth>a.width)a.width=a.minWidth;b=this.uiDialog.css({height:"auto",width:a.width}).height();d=Math.max(0,a.minHeight-b);if(a.height==="auto")if(c.support.minHeight)this.element.css({minHeight:d,height:"auto"});else{this.uiDialog.show();a=this.element.css("height","auto").height();e||this.uiDialog.hide();this.element.height(Math.max(a,d))}else this.element.height(Math.max(a.height-
b,0));this.uiDialog.is(":data(resizable)")&&this.uiDialog.resizable("option","minHeight",this._minHeight())}});c.extend(c.ui.dialog,{version:"1.8.13",uuid:0,maxZ:0,getTitleId:function(a){a=a.attr("id");if(!a){this.uuid+=1;a=this.uuid}return"ui-dialog-title-"+a},overlay:function(a){this.$el=c.ui.dialog.overlay.create(a)}});c.extend(c.ui.dialog.overlay,{instances:[],oldInstances:[],maxZ:0,events:c.map("focus,mousedown,mouseup,keydown,keypress,click".split(","),function(a){return a+".dialog-overlay"}).join(" "),
create:function(a){if(this.instances.length===0){setTimeout(function(){c.ui.dialog.overlay.instances.length&&c(document).bind(c.ui.dialog.overlay.events,function(d){if(c(d.target).zIndex()<c.ui.dialog.overlay.maxZ)return false})},1);c(document).bind("keydown.dialog-overlay",function(d){if(a.options.closeOnEscape&&d.keyCode&&d.keyCode===c.ui.keyCode.ESCAPE){a.close(d);d.preventDefault()}});c(window).bind("resize.dialog-overlay",c.ui.dialog.overlay.resize)}var b=(this.oldInstances.pop()||c("<div></div>").addClass("ui-widget-overlay")).appendTo(document.body).css({width:this.width(),
height:this.height()});c.fn.bgiframe&&b.bgiframe();this.instances.push(b);return b},destroy:function(a){var b=c.inArray(a,this.instances);b!=-1&&this.oldInstances.push(this.instances.splice(b,1)[0]);this.instances.length===0&&c([document,window]).unbind(".dialog-overlay");a.remove();var d=0;c.each(this.instances,function(){d=Math.max(d,this.css("z-index"))});this.maxZ=d},height:function(){var a,b;if(c.browser.msie&&c.browser.version<7){a=Math.max(document.documentElement.scrollHeight,document.body.scrollHeight);
b=Math.max(document.documentElement.offsetHeight,document.body.offsetHeight);return a<b?c(window).height()+"px":a+"px"}else return c(document).height()+"px"},width:function(){var a,b;if(c.browser.msie&&c.browser.version<7){a=Math.max(document.documentElement.scrollWidth,document.body.scrollWidth);b=Math.max(document.documentElement.offsetWidth,document.body.offsetWidth);return a<b?c(window).width()+"px":a+"px"}else return c(document).width()+"px"},resize:function(){var a=c([]);c.each(c.ui.dialog.overlay.instances,
function(){a=a.add(this)});a.css({width:0,height:0}).css({width:c.ui.dialog.overlay.width(),height:c.ui.dialog.overlay.height()})}});c.extend(c.ui.dialog.overlay.prototype,{destroy:function(){c.ui.dialog.overlay.destroy(this.$el)}})})(jQuery);
;/*
 * jQuery UI Slider 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Slider
 *
 * Depends:
 *	jquery.ui.core.js
 *	jquery.ui.mouse.js
 *	jquery.ui.widget.js
 */
(function(d){d.widget("ui.slider",d.ui.mouse,{widgetEventPrefix:"slide",options:{animate:false,distance:0,max:100,min:0,orientation:"horizontal",range:false,step:1,value:0,values:null},_create:function(){var b=this,a=this.options,c=this.element.find(".ui-slider-handle").addClass("ui-state-default ui-corner-all"),f=a.values&&a.values.length||1,e=[];this._mouseSliding=this._keySliding=false;this._animateOff=true;this._handleIndex=null;this._detectOrientation();this._mouseInit();this.element.addClass("ui-slider ui-slider-"+
this.orientation+" ui-widget ui-widget-content ui-corner-all"+(a.disabled?" ui-slider-disabled ui-disabled":""));this.range=d([]);if(a.range){if(a.range===true){if(!a.values)a.values=[this._valueMin(),this._valueMin()];if(a.values.length&&a.values.length!==2)a.values=[a.values[0],a.values[0]]}this.range=d("<div></div>").appendTo(this.element).addClass("ui-slider-range ui-widget-header"+(a.range==="min"||a.range==="max"?" ui-slider-range-"+a.range:""))}for(var j=c.length;j<f;j+=1)e.push("<a class='ui-slider-handle ui-state-default ui-corner-all' href='#'></a>");
this.handles=c.add(d(e.join("")).appendTo(b.element));this.handle=this.handles.eq(0);this.handles.add(this.range).filter("a").click(function(g){g.preventDefault()}).hover(function(){a.disabled||d(this).addClass("ui-state-hover")},function(){d(this).removeClass("ui-state-hover")}).focus(function(){if(a.disabled)d(this).blur();else{d(".ui-slider .ui-state-focus").removeClass("ui-state-focus");d(this).addClass("ui-state-focus")}}).blur(function(){d(this).removeClass("ui-state-focus")});this.handles.each(function(g){d(this).data("index.ui-slider-handle",
g)});this.handles.keydown(function(g){var k=true,l=d(this).data("index.ui-slider-handle"),i,h,m;if(!b.options.disabled){switch(g.keyCode){case d.ui.keyCode.HOME:case d.ui.keyCode.END:case d.ui.keyCode.PAGE_UP:case d.ui.keyCode.PAGE_DOWN:case d.ui.keyCode.UP:case d.ui.keyCode.RIGHT:case d.ui.keyCode.DOWN:case d.ui.keyCode.LEFT:k=false;if(!b._keySliding){b._keySliding=true;d(this).addClass("ui-state-active");i=b._start(g,l);if(i===false)return}break}m=b.options.step;i=b.options.values&&b.options.values.length?
(h=b.values(l)):(h=b.value());switch(g.keyCode){case d.ui.keyCode.HOME:h=b._valueMin();break;case d.ui.keyCode.END:h=b._valueMax();break;case d.ui.keyCode.PAGE_UP:h=b._trimAlignValue(i+(b._valueMax()-b._valueMin())/5);break;case d.ui.keyCode.PAGE_DOWN:h=b._trimAlignValue(i-(b._valueMax()-b._valueMin())/5);break;case d.ui.keyCode.UP:case d.ui.keyCode.RIGHT:if(i===b._valueMax())return;h=b._trimAlignValue(i+m);break;case d.ui.keyCode.DOWN:case d.ui.keyCode.LEFT:if(i===b._valueMin())return;h=b._trimAlignValue(i-
m);break}b._slide(g,l,h);return k}}).keyup(function(g){var k=d(this).data("index.ui-slider-handle");if(b._keySliding){b._keySliding=false;b._stop(g,k);b._change(g,k);d(this).removeClass("ui-state-active")}});this._refreshValue();this._animateOff=false},destroy:function(){this.handles.remove();this.range.remove();this.element.removeClass("ui-slider ui-slider-horizontal ui-slider-vertical ui-slider-disabled ui-widget ui-widget-content ui-corner-all").removeData("slider").unbind(".slider");this._mouseDestroy();
return this},_mouseCapture:function(b){var a=this.options,c,f,e,j,g;if(a.disabled)return false;this.elementSize={width:this.element.outerWidth(),height:this.element.outerHeight()};this.elementOffset=this.element.offset();c=this._normValueFromMouse({x:b.pageX,y:b.pageY});f=this._valueMax()-this._valueMin()+1;j=this;this.handles.each(function(k){var l=Math.abs(c-j.values(k));if(f>l){f=l;e=d(this);g=k}});if(a.range===true&&this.values(1)===a.min){g+=1;e=d(this.handles[g])}if(this._start(b,g)===false)return false;
this._mouseSliding=true;j._handleIndex=g;e.addClass("ui-state-active").focus();a=e.offset();this._clickOffset=!d(b.target).parents().andSelf().is(".ui-slider-handle")?{left:0,top:0}:{left:b.pageX-a.left-e.width()/2,top:b.pageY-a.top-e.height()/2-(parseInt(e.css("borderTopWidth"),10)||0)-(parseInt(e.css("borderBottomWidth"),10)||0)+(parseInt(e.css("marginTop"),10)||0)};this.handles.hasClass("ui-state-hover")||this._slide(b,g,c);return this._animateOff=true},_mouseStart:function(){return true},_mouseDrag:function(b){var a=
this._normValueFromMouse({x:b.pageX,y:b.pageY});this._slide(b,this._handleIndex,a);return false},_mouseStop:function(b){this.handles.removeClass("ui-state-active");this._mouseSliding=false;this._stop(b,this._handleIndex);this._change(b,this._handleIndex);this._clickOffset=this._handleIndex=null;return this._animateOff=false},_detectOrientation:function(){this.orientation=this.options.orientation==="vertical"?"vertical":"horizontal"},_normValueFromMouse:function(b){var a;if(this.orientation==="horizontal"){a=
this.elementSize.width;b=b.x-this.elementOffset.left-(this._clickOffset?this._clickOffset.left:0)}else{a=this.elementSize.height;b=b.y-this.elementOffset.top-(this._clickOffset?this._clickOffset.top:0)}a=b/a;if(a>1)a=1;if(a<0)a=0;if(this.orientation==="vertical")a=1-a;b=this._valueMax()-this._valueMin();return this._trimAlignValue(this._valueMin()+a*b)},_start:function(b,a){var c={handle:this.handles[a],value:this.value()};if(this.options.values&&this.options.values.length){c.value=this.values(a);
c.values=this.values()}return this._trigger("start",b,c)},_slide:function(b,a,c){var f;if(this.options.values&&this.options.values.length){f=this.values(a?0:1);if(this.options.values.length===2&&this.options.range===true&&(a===0&&c>f||a===1&&c<f))c=f;if(c!==this.values(a)){f=this.values();f[a]=c;b=this._trigger("slide",b,{handle:this.handles[a],value:c,values:f});this.values(a?0:1);b!==false&&this.values(a,c,true)}}else if(c!==this.value()){b=this._trigger("slide",b,{handle:this.handles[a],value:c});
b!==false&&this.value(c)}},_stop:function(b,a){var c={handle:this.handles[a],value:this.value()};if(this.options.values&&this.options.values.length){c.value=this.values(a);c.values=this.values()}this._trigger("stop",b,c)},_change:function(b,a){if(!this._keySliding&&!this._mouseSliding){var c={handle:this.handles[a],value:this.value()};if(this.options.values&&this.options.values.length){c.value=this.values(a);c.values=this.values()}this._trigger("change",b,c)}},value:function(b){if(arguments.length){this.options.value=
this._trimAlignValue(b);this._refreshValue();this._change(null,0)}else return this._value()},values:function(b,a){var c,f,e;if(arguments.length>1){this.options.values[b]=this._trimAlignValue(a);this._refreshValue();this._change(null,b)}else if(arguments.length)if(d.isArray(arguments[0])){c=this.options.values;f=arguments[0];for(e=0;e<c.length;e+=1){c[e]=this._trimAlignValue(f[e]);this._change(null,e)}this._refreshValue()}else return this.options.values&&this.options.values.length?this._values(b):
this.value();else return this._values()},_setOption:function(b,a){var c,f=0;if(d.isArray(this.options.values))f=this.options.values.length;d.Widget.prototype._setOption.apply(this,arguments);switch(b){case "disabled":if(a){this.handles.filter(".ui-state-focus").blur();this.handles.removeClass("ui-state-hover");this.handles.attr("disabled","disabled");this.element.addClass("ui-disabled")}else{this.handles.removeAttr("disabled");this.element.removeClass("ui-disabled")}break;case "orientation":this._detectOrientation();
this.element.removeClass("ui-slider-horizontal ui-slider-vertical").addClass("ui-slider-"+this.orientation);this._refreshValue();break;case "value":this._animateOff=true;this._refreshValue();this._change(null,0);this._animateOff=false;break;case "values":this._animateOff=true;this._refreshValue();for(c=0;c<f;c+=1)this._change(null,c);this._animateOff=false;break}},_value:function(){var b=this.options.value;return b=this._trimAlignValue(b)},_values:function(b){var a,c;if(arguments.length){a=this.options.values[b];
return a=this._trimAlignValue(a)}else{a=this.options.values.slice();for(c=0;c<a.length;c+=1)a[c]=this._trimAlignValue(a[c]);return a}},_trimAlignValue:function(b){if(b<=this._valueMin())return this._valueMin();if(b>=this._valueMax())return this._valueMax();var a=this.options.step>0?this.options.step:1,c=(b-this._valueMin())%a;alignValue=b-c;if(Math.abs(c)*2>=a)alignValue+=c>0?a:-a;return parseFloat(alignValue.toFixed(5))},_valueMin:function(){return this.options.min},_valueMax:function(){return this.options.max},
_refreshValue:function(){var b=this.options.range,a=this.options,c=this,f=!this._animateOff?a.animate:false,e,j={},g,k,l,i;if(this.options.values&&this.options.values.length)this.handles.each(function(h){e=(c.values(h)-c._valueMin())/(c._valueMax()-c._valueMin())*100;j[c.orientation==="horizontal"?"left":"bottom"]=e+"%";d(this).stop(1,1)[f?"animate":"css"](j,a.animate);if(c.options.range===true)if(c.orientation==="horizontal"){if(h===0)c.range.stop(1,1)[f?"animate":"css"]({left:e+"%"},a.animate);
if(h===1)c.range[f?"animate":"css"]({width:e-g+"%"},{queue:false,duration:a.animate})}else{if(h===0)c.range.stop(1,1)[f?"animate":"css"]({bottom:e+"%"},a.animate);if(h===1)c.range[f?"animate":"css"]({height:e-g+"%"},{queue:false,duration:a.animate})}g=e});else{k=this.value();l=this._valueMin();i=this._valueMax();e=i!==l?(k-l)/(i-l)*100:0;j[c.orientation==="horizontal"?"left":"bottom"]=e+"%";this.handle.stop(1,1)[f?"animate":"css"](j,a.animate);if(b==="min"&&this.orientation==="horizontal")this.range.stop(1,
1)[f?"animate":"css"]({width:e+"%"},a.animate);if(b==="max"&&this.orientation==="horizontal")this.range[f?"animate":"css"]({width:100-e+"%"},{queue:false,duration:a.animate});if(b==="min"&&this.orientation==="vertical")this.range.stop(1,1)[f?"animate":"css"]({height:e+"%"},a.animate);if(b==="max"&&this.orientation==="vertical")this.range[f?"animate":"css"]({height:100-e+"%"},{queue:false,duration:a.animate})}}});d.extend(d.ui.slider,{version:"1.8.13"})})(jQuery);
;/*
 * jQuery UI Tabs 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Tabs
 *
 * Depends:
 *	jquery.ui.core.js
 *	jquery.ui.widget.js
 */
(function(d,p){function u(){return++v}function w(){return++x}var v=0,x=0;d.widget("ui.tabs",{options:{add:null,ajaxOptions:null,cache:false,cookie:null,collapsible:false,disable:null,disabled:[],enable:null,event:"click",fx:null,idPrefix:"ui-tabs-",load:null,panelTemplate:"<div></div>",remove:null,select:null,show:null,spinner:"<em>Loading&#8230;</em>",tabTemplate:"<li><a href='#{href}'><span>#{label}</span></a></li>"},_create:function(){this._tabify(true)},_setOption:function(b,e){if(b=="selected")this.options.collapsible&&
e==this.options.selected||this.select(e);else{this.options[b]=e;this._tabify()}},_tabId:function(b){return b.title&&b.title.replace(/\s/g,"_").replace(/[^\w\u00c0-\uFFFF-]/g,"")||this.options.idPrefix+u()},_sanitizeSelector:function(b){return b.replace(/:/g,"\\:")},_cookie:function(){var b=this.cookie||(this.cookie=this.options.cookie.name||"ui-tabs-"+w());return d.cookie.apply(null,[b].concat(d.makeArray(arguments)))},_ui:function(b,e){return{tab:b,panel:e,index:this.anchors.index(b)}},_cleanup:function(){this.lis.filter(".ui-state-processing").removeClass("ui-state-processing").find("span:data(label.tabs)").each(function(){var b=
d(this);b.html(b.data("label.tabs")).removeData("label.tabs")})},_tabify:function(b){function e(g,f){g.css("display","");!d.support.opacity&&f.opacity&&g[0].style.removeAttribute("filter")}var a=this,c=this.options,h=/^#.+/;this.list=this.element.find("ol,ul").eq(0);this.lis=d(" > li:has(a[href])",this.list);this.anchors=this.lis.map(function(){return d("a",this)[0]});this.panels=d([]);this.anchors.each(function(g,f){var i=d(f).attr("href"),l=i.split("#")[0],q;if(l&&(l===location.toString().split("#")[0]||
(q=d("base")[0])&&l===q.href)){i=f.hash;f.href=i}if(h.test(i))a.panels=a.panels.add(a.element.find(a._sanitizeSelector(i)));else if(i&&i!=="#"){d.data(f,"href.tabs",i);d.data(f,"load.tabs",i.replace(/#.*$/,""));i=a._tabId(f);f.href="#"+i;f=a.element.find("#"+i);if(!f.length){f=d(c.panelTemplate).attr("id",i).addClass("ui-tabs-panel ui-widget-content ui-corner-bottom").insertAfter(a.panels[g-1]||a.list);f.data("destroy.tabs",true)}a.panels=a.panels.add(f)}else c.disabled.push(g)});if(b){this.element.addClass("ui-tabs ui-widget ui-widget-content ui-corner-all");
this.list.addClass("ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all");this.lis.addClass("ui-state-default ui-corner-top");this.panels.addClass("ui-tabs-panel ui-widget-content ui-corner-bottom");if(c.selected===p){location.hash&&this.anchors.each(function(g,f){if(f.hash==location.hash){c.selected=g;return false}});if(typeof c.selected!=="number"&&c.cookie)c.selected=parseInt(a._cookie(),10);if(typeof c.selected!=="number"&&this.lis.filter(".ui-tabs-selected").length)c.selected=
this.lis.index(this.lis.filter(".ui-tabs-selected"));c.selected=c.selected||(this.lis.length?0:-1)}else if(c.selected===null)c.selected=-1;c.selected=c.selected>=0&&this.anchors[c.selected]||c.selected<0?c.selected:0;c.disabled=d.unique(c.disabled.concat(d.map(this.lis.filter(".ui-state-disabled"),function(g){return a.lis.index(g)}))).sort();d.inArray(c.selected,c.disabled)!=-1&&c.disabled.splice(d.inArray(c.selected,c.disabled),1);this.panels.addClass("ui-tabs-hide");this.lis.removeClass("ui-tabs-selected ui-state-active");
if(c.selected>=0&&this.anchors.length){a.element.find(a._sanitizeSelector(a.anchors[c.selected].hash)).removeClass("ui-tabs-hide");this.lis.eq(c.selected).addClass("ui-tabs-selected ui-state-active");a.element.queue("tabs",function(){a._trigger("show",null,a._ui(a.anchors[c.selected],a.element.find(a._sanitizeSelector(a.anchors[c.selected].hash))[0]))});this.load(c.selected)}d(window).bind("unload",function(){a.lis.add(a.anchors).unbind(".tabs");a.lis=a.anchors=a.panels=null})}else c.selected=this.lis.index(this.lis.filter(".ui-tabs-selected"));
this.element[c.collapsible?"addClass":"removeClass"]("ui-tabs-collapsible");c.cookie&&this._cookie(c.selected,c.cookie);b=0;for(var j;j=this.lis[b];b++)d(j)[d.inArray(b,c.disabled)!=-1&&!d(j).hasClass("ui-tabs-selected")?"addClass":"removeClass"]("ui-state-disabled");c.cache===false&&this.anchors.removeData("cache.tabs");this.lis.add(this.anchors).unbind(".tabs");if(c.event!=="mouseover"){var k=function(g,f){f.is(":not(.ui-state-disabled)")&&f.addClass("ui-state-"+g)},n=function(g,f){f.removeClass("ui-state-"+
g)};this.lis.bind("mouseover.tabs",function(){k("hover",d(this))});this.lis.bind("mouseout.tabs",function(){n("hover",d(this))});this.anchors.bind("focus.tabs",function(){k("focus",d(this).closest("li"))});this.anchors.bind("blur.tabs",function(){n("focus",d(this).closest("li"))})}var m,o;if(c.fx)if(d.isArray(c.fx)){m=c.fx[0];o=c.fx[1]}else m=o=c.fx;var r=o?function(g,f){d(g).closest("li").addClass("ui-tabs-selected ui-state-active");f.hide().removeClass("ui-tabs-hide").animate(o,o.duration||"normal",
function(){e(f,o);a._trigger("show",null,a._ui(g,f[0]))})}:function(g,f){d(g).closest("li").addClass("ui-tabs-selected ui-state-active");f.removeClass("ui-tabs-hide");a._trigger("show",null,a._ui(g,f[0]))},s=m?function(g,f){f.animate(m,m.duration||"normal",function(){a.lis.removeClass("ui-tabs-selected ui-state-active");f.addClass("ui-tabs-hide");e(f,m);a.element.dequeue("tabs")})}:function(g,f){a.lis.removeClass("ui-tabs-selected ui-state-active");f.addClass("ui-tabs-hide");a.element.dequeue("tabs")};
this.anchors.bind(c.event+".tabs",function(){var g=this,f=d(g).closest("li"),i=a.panels.filter(":not(.ui-tabs-hide)"),l=a.element.find(a._sanitizeSelector(g.hash));if(f.hasClass("ui-tabs-selected")&&!c.collapsible||f.hasClass("ui-state-disabled")||f.hasClass("ui-state-processing")||a.panels.filter(":animated").length||a._trigger("select",null,a._ui(this,l[0]))===false){this.blur();return false}c.selected=a.anchors.index(this);a.abort();if(c.collapsible)if(f.hasClass("ui-tabs-selected")){c.selected=
-1;c.cookie&&a._cookie(c.selected,c.cookie);a.element.queue("tabs",function(){s(g,i)}).dequeue("tabs");this.blur();return false}else if(!i.length){c.cookie&&a._cookie(c.selected,c.cookie);a.element.queue("tabs",function(){r(g,l)});a.load(a.anchors.index(this));this.blur();return false}c.cookie&&a._cookie(c.selected,c.cookie);if(l.length){i.length&&a.element.queue("tabs",function(){s(g,i)});a.element.queue("tabs",function(){r(g,l)});a.load(a.anchors.index(this))}else throw"jQuery UI Tabs: Mismatching fragment identifier.";
d.browser.msie&&this.blur()});this.anchors.bind("click.tabs",function(){return false})},_getIndex:function(b){if(typeof b=="string")b=this.anchors.index(this.anchors.filter("[href$="+b+"]"));return b},destroy:function(){var b=this.options;this.abort();this.element.unbind(".tabs").removeClass("ui-tabs ui-widget ui-widget-content ui-corner-all ui-tabs-collapsible").removeData("tabs");this.list.removeClass("ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all");this.anchors.each(function(){var e=
d.data(this,"href.tabs");if(e)this.href=e;var a=d(this).unbind(".tabs");d.each(["href","load","cache"],function(c,h){a.removeData(h+".tabs")})});this.lis.unbind(".tabs").add(this.panels).each(function(){d.data(this,"destroy.tabs")?d(this).remove():d(this).removeClass("ui-state-default ui-corner-top ui-tabs-selected ui-state-active ui-state-hover ui-state-focus ui-state-disabled ui-tabs-panel ui-widget-content ui-corner-bottom ui-tabs-hide")});b.cookie&&this._cookie(null,b.cookie);return this},add:function(b,
e,a){if(a===p)a=this.anchors.length;var c=this,h=this.options;e=d(h.tabTemplate.replace(/#\{href\}/g,b).replace(/#\{label\}/g,e));b=!b.indexOf("#")?b.replace("#",""):this._tabId(d("a",e)[0]);e.addClass("ui-state-default ui-corner-top").data("destroy.tabs",true);var j=c.element.find("#"+b);j.length||(j=d(h.panelTemplate).attr("id",b).data("destroy.tabs",true));j.addClass("ui-tabs-panel ui-widget-content ui-corner-bottom ui-tabs-hide");if(a>=this.lis.length){e.appendTo(this.list);j.appendTo(this.list[0].parentNode)}else{e.insertBefore(this.lis[a]);
j.insertBefore(this.panels[a])}h.disabled=d.map(h.disabled,function(k){return k>=a?++k:k});this._tabify();if(this.anchors.length==1){h.selected=0;e.addClass("ui-tabs-selected ui-state-active");j.removeClass("ui-tabs-hide");this.element.queue("tabs",function(){c._trigger("show",null,c._ui(c.anchors[0],c.panels[0]))});this.load(0)}this._trigger("add",null,this._ui(this.anchors[a],this.panels[a]));return this},remove:function(b){b=this._getIndex(b);var e=this.options,a=this.lis.eq(b).remove(),c=this.panels.eq(b).remove();
if(a.hasClass("ui-tabs-selected")&&this.anchors.length>1)this.select(b+(b+1<this.anchors.length?1:-1));e.disabled=d.map(d.grep(e.disabled,function(h){return h!=b}),function(h){return h>=b?--h:h});this._tabify();this._trigger("remove",null,this._ui(a.find("a")[0],c[0]));return this},enable:function(b){b=this._getIndex(b);var e=this.options;if(d.inArray(b,e.disabled)!=-1){this.lis.eq(b).removeClass("ui-state-disabled");e.disabled=d.grep(e.disabled,function(a){return a!=b});this._trigger("enable",null,
this._ui(this.anchors[b],this.panels[b]));return this}},disable:function(b){b=this._getIndex(b);var e=this.options;if(b!=e.selected){this.lis.eq(b).addClass("ui-state-disabled");e.disabled.push(b);e.disabled.sort();this._trigger("disable",null,this._ui(this.anchors[b],this.panels[b]))}return this},select:function(b){b=this._getIndex(b);if(b==-1)if(this.options.collapsible&&this.options.selected!=-1)b=this.options.selected;else return this;this.anchors.eq(b).trigger(this.options.event+".tabs");return this},
load:function(b){b=this._getIndex(b);var e=this,a=this.options,c=this.anchors.eq(b)[0],h=d.data(c,"load.tabs");this.abort();if(!h||this.element.queue("tabs").length!==0&&d.data(c,"cache.tabs"))this.element.dequeue("tabs");else{this.lis.eq(b).addClass("ui-state-processing");if(a.spinner){var j=d("span",c);j.data("label.tabs",j.html()).html(a.spinner)}this.xhr=d.ajax(d.extend({},a.ajaxOptions,{url:h,success:function(k,n){e.element.find(e._sanitizeSelector(c.hash)).html(k);e._cleanup();a.cache&&d.data(c,
"cache.tabs",true);e._trigger("load",null,e._ui(e.anchors[b],e.panels[b]));try{a.ajaxOptions.success(k,n)}catch(m){}},error:function(k,n){e._cleanup();e._trigger("load",null,e._ui(e.anchors[b],e.panels[b]));try{a.ajaxOptions.error(k,n,b,c)}catch(m){}}}));e.element.dequeue("tabs");return this}},abort:function(){this.element.queue([]);this.panels.stop(false,true);this.element.queue("tabs",this.element.queue("tabs").splice(-2,2));if(this.xhr){this.xhr.abort();delete this.xhr}this._cleanup();return this},
url:function(b,e){this.anchors.eq(b).removeData("cache.tabs").data("load.tabs",e);return this},length:function(){return this.anchors.length}});d.extend(d.ui.tabs,{version:"1.8.13"});d.extend(d.ui.tabs.prototype,{rotation:null,rotate:function(b,e){var a=this,c=this.options,h=a._rotate||(a._rotate=function(j){clearTimeout(a.rotation);a.rotation=setTimeout(function(){var k=c.selected;a.select(++k<a.anchors.length?k:0)},b);j&&j.stopPropagation()});e=a._unrotate||(a._unrotate=!e?function(j){j.clientX&&
a.rotate(null)}:function(){t=c.selected;h()});if(b){this.element.bind("tabsshow",h);this.anchors.bind(c.event+".tabs",e);h()}else{clearTimeout(a.rotation);this.element.unbind("tabsshow",h);this.anchors.unbind(c.event+".tabs",e);delete this._rotate;delete this._unrotate}return this}})})(jQuery);
;/*
 * jQuery UI Datepicker 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Datepicker
 *
 * Depends:
 *	jquery.ui.core.js
 */
(function(d,B){function M(){this.debug=false;this._curInst=null;this._keyEvent=false;this._disabledInputs=[];this._inDialog=this._datepickerShowing=false;this._mainDivId="ui-datepicker-div";this._inlineClass="ui-datepicker-inline";this._appendClass="ui-datepicker-append";this._triggerClass="ui-datepicker-trigger";this._dialogClass="ui-datepicker-dialog";this._disableClass="ui-datepicker-disabled";this._unselectableClass="ui-datepicker-unselectable";this._currentClass="ui-datepicker-current-day";this._dayOverClass=
"ui-datepicker-days-cell-over";this.regional=[];this.regional[""]={closeText:"Done",prevText:"Prev",nextText:"Next",currentText:"Today",monthNames:["January","February","March","April","May","June","July","August","September","October","November","December"],monthNamesShort:["Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"],dayNames:["Sunday","Monday","Tuesday","Wednesday","Thursday","Friday","Saturday"],dayNamesShort:["Sun","Mon","Tue","Wed","Thu","Fri","Sat"],dayNamesMin:["Su",
"Mo","Tu","We","Th","Fr","Sa"],weekHeader:"Wk",dateFormat:"mm/dd/yy",firstDay:0,isRTL:false,showMonthAfterYear:false,yearSuffix:""};this._defaults={showOn:"focus",showAnim:"fadeIn",showOptions:{},defaultDate:null,appendText:"",buttonText:"...",buttonImage:"",buttonImageOnly:false,hideIfNoPrevNext:false,navigationAsDateFormat:false,gotoCurrent:false,changeMonth:false,changeYear:false,yearRange:"c-10:c+10",showOtherMonths:false,selectOtherMonths:false,showWeek:false,calculateWeek:this.iso8601Week,shortYearCutoff:"+10",
minDate:null,maxDate:null,duration:"fast",beforeShowDay:null,beforeShow:null,onSelect:null,onChangeMonthYear:null,onClose:null,numberOfMonths:1,showCurrentAtPos:0,stepMonths:1,stepBigMonths:12,altField:"",altFormat:"",constrainInput:true,showButtonPanel:false,autoSize:false};d.extend(this._defaults,this.regional[""]);this.dpDiv=N(d('<div id="'+this._mainDivId+'" class="ui-datepicker ui-widget ui-widget-content ui-helper-clearfix ui-corner-all"></div>'))}function N(a){return a.delegate("button, .ui-datepicker-prev, .ui-datepicker-next, .ui-datepicker-calendar td a",
"mouseout",function(){d(this).removeClass("ui-state-hover");this.className.indexOf("ui-datepicker-prev")!=-1&&d(this).removeClass("ui-datepicker-prev-hover");this.className.indexOf("ui-datepicker-next")!=-1&&d(this).removeClass("ui-datepicker-next-hover")}).delegate("button, .ui-datepicker-prev, .ui-datepicker-next, .ui-datepicker-calendar td a","mouseover",function(){if(!d.datepicker._isDisabledDatepicker(J.inline?a.parent()[0]:J.input[0])){d(this).parents(".ui-datepicker-calendar").find("a").removeClass("ui-state-hover");
d(this).addClass("ui-state-hover");this.className.indexOf("ui-datepicker-prev")!=-1&&d(this).addClass("ui-datepicker-prev-hover");this.className.indexOf("ui-datepicker-next")!=-1&&d(this).addClass("ui-datepicker-next-hover")}})}function H(a,b){d.extend(a,b);for(var c in b)if(b[c]==null||b[c]==B)a[c]=b[c];return a}d.extend(d.ui,{datepicker:{version:"1.8.13"}});var z=(new Date).getTime(),J;d.extend(M.prototype,{markerClassName:"hasDatepicker",log:function(){this.debug&&console.log.apply("",arguments)},
_widgetDatepicker:function(){return this.dpDiv},setDefaults:function(a){H(this._defaults,a||{});return this},_attachDatepicker:function(a,b){var c=null;for(var e in this._defaults){var f=a.getAttribute("date:"+e);if(f){c=c||{};try{c[e]=eval(f)}catch(h){c[e]=f}}}e=a.nodeName.toLowerCase();f=e=="div"||e=="span";if(!a.id){this.uuid+=1;a.id="dp"+this.uuid}var i=this._newInst(d(a),f);i.settings=d.extend({},b||{},c||{});if(e=="input")this._connectDatepicker(a,i);else f&&this._inlineDatepicker(a,i)},_newInst:function(a,
b){return{id:a[0].id.replace(/([^A-Za-z0-9_-])/g,"\\\\$1"),input:a,selectedDay:0,selectedMonth:0,selectedYear:0,drawMonth:0,drawYear:0,inline:b,dpDiv:!b?this.dpDiv:N(d('<div class="'+this._inlineClass+' ui-datepicker ui-widget ui-widget-content ui-helper-clearfix ui-corner-all"></div>'))}},_connectDatepicker:function(a,b){var c=d(a);b.append=d([]);b.trigger=d([]);if(!c.hasClass(this.markerClassName)){this._attachments(c,b);c.addClass(this.markerClassName).keydown(this._doKeyDown).keypress(this._doKeyPress).keyup(this._doKeyUp).bind("setData.datepicker",
function(e,f,h){b.settings[f]=h}).bind("getData.datepicker",function(e,f){return this._get(b,f)});this._autoSize(b);d.data(a,"datepicker",b)}},_attachments:function(a,b){var c=this._get(b,"appendText"),e=this._get(b,"isRTL");b.append&&b.append.remove();if(c){b.append=d('<span class="'+this._appendClass+'">'+c+"</span>");a[e?"before":"after"](b.append)}a.unbind("focus",this._showDatepicker);b.trigger&&b.trigger.remove();c=this._get(b,"showOn");if(c=="focus"||c=="both")a.focus(this._showDatepicker);
if(c=="button"||c=="both"){c=this._get(b,"buttonText");var f=this._get(b,"buttonImage");b.trigger=d(this._get(b,"buttonImageOnly")?d("<img/>").addClass(this._triggerClass).attr({src:f,alt:c,title:c}):d('<button type="button"></button>').addClass(this._triggerClass).html(f==""?c:d("<img/>").attr({src:f,alt:c,title:c})));a[e?"before":"after"](b.trigger);b.trigger.click(function(){d.datepicker._datepickerShowing&&d.datepicker._lastInput==a[0]?d.datepicker._hideDatepicker():d.datepicker._showDatepicker(a[0]);
return false})}},_autoSize:function(a){if(this._get(a,"autoSize")&&!a.inline){var b=new Date(2009,11,20),c=this._get(a,"dateFormat");if(c.match(/[DM]/)){var e=function(f){for(var h=0,i=0,g=0;g<f.length;g++)if(f[g].length>h){h=f[g].length;i=g}return i};b.setMonth(e(this._get(a,c.match(/MM/)?"monthNames":"monthNamesShort")));b.setDate(e(this._get(a,c.match(/DD/)?"dayNames":"dayNamesShort"))+20-b.getDay())}a.input.attr("size",this._formatDate(a,b).length)}},_inlineDatepicker:function(a,b){var c=d(a);
if(!c.hasClass(this.markerClassName)){c.addClass(this.markerClassName).append(b.dpDiv).bind("setData.datepicker",function(e,f,h){b.settings[f]=h}).bind("getData.datepicker",function(e,f){return this._get(b,f)});d.data(a,"datepicker",b);this._setDate(b,this._getDefaultDate(b),true);this._updateDatepicker(b);this._updateAlternate(b);b.dpDiv.show()}},_dialogDatepicker:function(a,b,c,e,f){a=this._dialogInst;if(!a){this.uuid+=1;this._dialogInput=d('<input type="text" id="'+("dp"+this.uuid)+'" style="position: absolute; top: -100px; width: 0px; z-index: -10;"/>');
this._dialogInput.keydown(this._doKeyDown);d("body").append(this._dialogInput);a=this._dialogInst=this._newInst(this._dialogInput,false);a.settings={};d.data(this._dialogInput[0],"datepicker",a)}H(a.settings,e||{});b=b&&b.constructor==Date?this._formatDate(a,b):b;this._dialogInput.val(b);this._pos=f?f.length?f:[f.pageX,f.pageY]:null;if(!this._pos)this._pos=[document.documentElement.clientWidth/2-100+(document.documentElement.scrollLeft||document.body.scrollLeft),document.documentElement.clientHeight/
2-150+(document.documentElement.scrollTop||document.body.scrollTop)];this._dialogInput.css("left",this._pos[0]+20+"px").css("top",this._pos[1]+"px");a.settings.onSelect=c;this._inDialog=true;this.dpDiv.addClass(this._dialogClass);this._showDatepicker(this._dialogInput[0]);d.blockUI&&d.blockUI(this.dpDiv);d.data(this._dialogInput[0],"datepicker",a);return this},_destroyDatepicker:function(a){var b=d(a),c=d.data(a,"datepicker");if(b.hasClass(this.markerClassName)){var e=a.nodeName.toLowerCase();d.removeData(a,
"datepicker");if(e=="input"){c.append.remove();c.trigger.remove();b.removeClass(this.markerClassName).unbind("focus",this._showDatepicker).unbind("keydown",this._doKeyDown).unbind("keypress",this._doKeyPress).unbind("keyup",this._doKeyUp)}else if(e=="div"||e=="span")b.removeClass(this.markerClassName).empty()}},_enableDatepicker:function(a){var b=d(a),c=d.data(a,"datepicker");if(b.hasClass(this.markerClassName)){var e=a.nodeName.toLowerCase();if(e=="input"){a.disabled=false;c.trigger.filter("button").each(function(){this.disabled=
false}).end().filter("img").css({opacity:"1.0",cursor:""})}else if(e=="div"||e=="span"){b=b.children("."+this._inlineClass);b.children().removeClass("ui-state-disabled");b.find("select.ui-datepicker-month, select.ui-datepicker-year").removeAttr("disabled")}this._disabledInputs=d.map(this._disabledInputs,function(f){return f==a?null:f})}},_disableDatepicker:function(a){var b=d(a),c=d.data(a,"datepicker");if(b.hasClass(this.markerClassName)){var e=a.nodeName.toLowerCase();if(e=="input"){a.disabled=
true;c.trigger.filter("button").each(function(){this.disabled=true}).end().filter("img").css({opacity:"0.5",cursor:"default"})}else if(e=="div"||e=="span"){b=b.children("."+this._inlineClass);b.children().addClass("ui-state-disabled");b.find("select.ui-datepicker-month, select.ui-datepicker-year").attr("disabled","disabled")}this._disabledInputs=d.map(this._disabledInputs,function(f){return f==a?null:f});this._disabledInputs[this._disabledInputs.length]=a}},_isDisabledDatepicker:function(a){if(!a)return false;
for(var b=0;b<this._disabledInputs.length;b++)if(this._disabledInputs[b]==a)return true;return false},_getInst:function(a){try{return d.data(a,"datepicker")}catch(b){throw"Missing instance data for this datepicker";}},_optionDatepicker:function(a,b,c){var e=this._getInst(a);if(arguments.length==2&&typeof b=="string")return b=="defaults"?d.extend({},d.datepicker._defaults):e?b=="all"?d.extend({},e.settings):this._get(e,b):null;var f=b||{};if(typeof b=="string"){f={};f[b]=c}if(e){this._curInst==e&&
this._hideDatepicker();var h=this._getDateDatepicker(a,true),i=this._getMinMaxDate(e,"min"),g=this._getMinMaxDate(e,"max");H(e.settings,f);if(i!==null&&f.dateFormat!==B&&f.minDate===B)e.settings.minDate=this._formatDate(e,i);if(g!==null&&f.dateFormat!==B&&f.maxDate===B)e.settings.maxDate=this._formatDate(e,g);this._attachments(d(a),e);this._autoSize(e);this._setDate(e,h);this._updateAlternate(e);this._updateDatepicker(e)}},_changeDatepicker:function(a,b,c){this._optionDatepicker(a,b,c)},_refreshDatepicker:function(a){(a=
this._getInst(a))&&this._updateDatepicker(a)},_setDateDatepicker:function(a,b){if(a=this._getInst(a)){this._setDate(a,b);this._updateDatepicker(a);this._updateAlternate(a)}},_getDateDatepicker:function(a,b){(a=this._getInst(a))&&!a.inline&&this._setDateFromField(a,b);return a?this._getDate(a):null},_doKeyDown:function(a){var b=d.datepicker._getInst(a.target),c=true,e=b.dpDiv.is(".ui-datepicker-rtl");b._keyEvent=true;if(d.datepicker._datepickerShowing)switch(a.keyCode){case 9:d.datepicker._hideDatepicker();
c=false;break;case 13:c=d("td."+d.datepicker._dayOverClass+":not(."+d.datepicker._currentClass+")",b.dpDiv);c[0]?d.datepicker._selectDay(a.target,b.selectedMonth,b.selectedYear,c[0]):d.datepicker._hideDatepicker();return false;case 27:d.datepicker._hideDatepicker();break;case 33:d.datepicker._adjustDate(a.target,a.ctrlKey?-d.datepicker._get(b,"stepBigMonths"):-d.datepicker._get(b,"stepMonths"),"M");break;case 34:d.datepicker._adjustDate(a.target,a.ctrlKey?+d.datepicker._get(b,"stepBigMonths"):+d.datepicker._get(b,
"stepMonths"),"M");break;case 35:if(a.ctrlKey||a.metaKey)d.datepicker._clearDate(a.target);c=a.ctrlKey||a.metaKey;break;case 36:if(a.ctrlKey||a.metaKey)d.datepicker._gotoToday(a.target);c=a.ctrlKey||a.metaKey;break;case 37:if(a.ctrlKey||a.metaKey)d.datepicker._adjustDate(a.target,e?+1:-1,"D");c=a.ctrlKey||a.metaKey;if(a.originalEvent.altKey)d.datepicker._adjustDate(a.target,a.ctrlKey?-d.datepicker._get(b,"stepBigMonths"):-d.datepicker._get(b,"stepMonths"),"M");break;case 38:if(a.ctrlKey||a.metaKey)d.datepicker._adjustDate(a.target,
-7,"D");c=a.ctrlKey||a.metaKey;break;case 39:if(a.ctrlKey||a.metaKey)d.datepicker._adjustDate(a.target,e?-1:+1,"D");c=a.ctrlKey||a.metaKey;if(a.originalEvent.altKey)d.datepicker._adjustDate(a.target,a.ctrlKey?+d.datepicker._get(b,"stepBigMonths"):+d.datepicker._get(b,"stepMonths"),"M");break;case 40:if(a.ctrlKey||a.metaKey)d.datepicker._adjustDate(a.target,+7,"D");c=a.ctrlKey||a.metaKey;break;default:c=false}else if(a.keyCode==36&&a.ctrlKey)d.datepicker._showDatepicker(this);else c=false;if(c){a.preventDefault();
a.stopPropagation()}},_doKeyPress:function(a){var b=d.datepicker._getInst(a.target);if(d.datepicker._get(b,"constrainInput")){b=d.datepicker._possibleChars(d.datepicker._get(b,"dateFormat"));var c=String.fromCharCode(a.charCode==B?a.keyCode:a.charCode);return a.ctrlKey||a.metaKey||c<" "||!b||b.indexOf(c)>-1}},_doKeyUp:function(a){a=d.datepicker._getInst(a.target);if(a.input.val()!=a.lastVal)try{if(d.datepicker.parseDate(d.datepicker._get(a,"dateFormat"),a.input?a.input.val():null,d.datepicker._getFormatConfig(a))){d.datepicker._setDateFromField(a);
d.datepicker._updateAlternate(a);d.datepicker._updateDatepicker(a)}}catch(b){d.datepicker.log(b)}return true},_showDatepicker:function(a){a=a.target||a;if(a.nodeName.toLowerCase()!="input")a=d("input",a.parentNode)[0];if(!(d.datepicker._isDisabledDatepicker(a)||d.datepicker._lastInput==a)){var b=d.datepicker._getInst(a);d.datepicker._curInst&&d.datepicker._curInst!=b&&d.datepicker._curInst.dpDiv.stop(true,true);var c=d.datepicker._get(b,"beforeShow");H(b.settings,c?c.apply(a,[a,b]):{});b.lastVal=
null;d.datepicker._lastInput=a;d.datepicker._setDateFromField(b);if(d.datepicker._inDialog)a.value="";if(!d.datepicker._pos){d.datepicker._pos=d.datepicker._findPos(a);d.datepicker._pos[1]+=a.offsetHeight}var e=false;d(a).parents().each(function(){e|=d(this).css("position")=="fixed";return!e});if(e&&d.browser.opera){d.datepicker._pos[0]-=document.documentElement.scrollLeft;d.datepicker._pos[1]-=document.documentElement.scrollTop}c={left:d.datepicker._pos[0],top:d.datepicker._pos[1]};d.datepicker._pos=
null;b.dpDiv.empty();b.dpDiv.css({position:"absolute",display:"block",top:"-1000px"});d.datepicker._updateDatepicker(b);c=d.datepicker._checkOffset(b,c,e);b.dpDiv.css({position:d.datepicker._inDialog&&d.blockUI?"static":e?"fixed":"absolute",display:"none",left:c.left+"px",top:c.top+"px"});if(!b.inline){c=d.datepicker._get(b,"showAnim");var f=d.datepicker._get(b,"duration"),h=function(){var i=b.dpDiv.find("iframe.ui-datepicker-cover");if(i.length){var g=d.datepicker._getBorders(b.dpDiv);i.css({left:-g[0],
top:-g[1],width:b.dpDiv.outerWidth(),height:b.dpDiv.outerHeight()})}};b.dpDiv.zIndex(d(a).zIndex()+1);d.datepicker._datepickerShowing=true;d.effects&&d.effects[c]?b.dpDiv.show(c,d.datepicker._get(b,"showOptions"),f,h):b.dpDiv[c||"show"](c?f:null,h);if(!c||!f)h();b.input.is(":visible")&&!b.input.is(":disabled")&&b.input.focus();d.datepicker._curInst=b}}},_updateDatepicker:function(a){var b=d.datepicker._getBorders(a.dpDiv);J=a;a.dpDiv.empty().append(this._generateHTML(a));var c=a.dpDiv.find("iframe.ui-datepicker-cover");
c.length&&c.css({left:-b[0],top:-b[1],width:a.dpDiv.outerWidth(),height:a.dpDiv.outerHeight()});a.dpDiv.find("."+this._dayOverClass+" a").mouseover();b=this._getNumberOfMonths(a);c=b[1];a.dpDiv.removeClass("ui-datepicker-multi-2 ui-datepicker-multi-3 ui-datepicker-multi-4").width("");c>1&&a.dpDiv.addClass("ui-datepicker-multi-"+c).css("width",17*c+"em");a.dpDiv[(b[0]!=1||b[1]!=1?"add":"remove")+"Class"]("ui-datepicker-multi");a.dpDiv[(this._get(a,"isRTL")?"add":"remove")+"Class"]("ui-datepicker-rtl");
a==d.datepicker._curInst&&d.datepicker._datepickerShowing&&a.input&&a.input.is(":visible")&&!a.input.is(":disabled")&&a.input[0]!=document.activeElement&&a.input.focus();if(a.yearshtml){var e=a.yearshtml;setTimeout(function(){e===a.yearshtml&&a.yearshtml&&a.dpDiv.find("select.ui-datepicker-year:first").replaceWith(a.yearshtml);e=a.yearshtml=null},0)}},_getBorders:function(a){var b=function(c){return{thin:1,medium:2,thick:3}[c]||c};return[parseFloat(b(a.css("border-left-width"))),parseFloat(b(a.css("border-top-width")))]},
_checkOffset:function(a,b,c){var e=a.dpDiv.outerWidth(),f=a.dpDiv.outerHeight(),h=a.input?a.input.outerWidth():0,i=a.input?a.input.outerHeight():0,g=document.documentElement.clientWidth+d(document).scrollLeft(),j=document.documentElement.clientHeight+d(document).scrollTop();b.left-=this._get(a,"isRTL")?e-h:0;b.left-=c&&b.left==a.input.offset().left?d(document).scrollLeft():0;b.top-=c&&b.top==a.input.offset().top+i?d(document).scrollTop():0;b.left-=Math.min(b.left,b.left+e>g&&g>e?Math.abs(b.left+e-
g):0);b.top-=Math.min(b.top,b.top+f>j&&j>f?Math.abs(f+i):0);return b},_findPos:function(a){for(var b=this._get(this._getInst(a),"isRTL");a&&(a.type=="hidden"||a.nodeType!=1||d.expr.filters.hidden(a));)a=a[b?"previousSibling":"nextSibling"];a=d(a).offset();return[a.left,a.top]},_hideDatepicker:function(a){var b=this._curInst;if(!(!b||a&&b!=d.data(a,"datepicker")))if(this._datepickerShowing){a=this._get(b,"showAnim");var c=this._get(b,"duration"),e=function(){d.datepicker._tidyDialog(b);this._curInst=
null};d.effects&&d.effects[a]?b.dpDiv.hide(a,d.datepicker._get(b,"showOptions"),c,e):b.dpDiv[a=="slideDown"?"slideUp":a=="fadeIn"?"fadeOut":"hide"](a?c:null,e);a||e();if(a=this._get(b,"onClose"))a.apply(b.input?b.input[0]:null,[b.input?b.input.val():"",b]);this._datepickerShowing=false;this._lastInput=null;if(this._inDialog){this._dialogInput.css({position:"absolute",left:"0",top:"-100px"});if(d.blockUI){d.unblockUI();d("body").append(this.dpDiv)}}this._inDialog=false}},_tidyDialog:function(a){a.dpDiv.removeClass(this._dialogClass).unbind(".ui-datepicker-calendar")},
_checkExternalClick:function(a){if(d.datepicker._curInst){a=d(a.target);a[0].id!=d.datepicker._mainDivId&&a.parents("#"+d.datepicker._mainDivId).length==0&&!a.hasClass(d.datepicker.markerClassName)&&!a.hasClass(d.datepicker._triggerClass)&&d.datepicker._datepickerShowing&&!(d.datepicker._inDialog&&d.blockUI)&&d.datepicker._hideDatepicker()}},_adjustDate:function(a,b,c){a=d(a);var e=this._getInst(a[0]);if(!this._isDisabledDatepicker(a[0])){this._adjustInstDate(e,b+(c=="M"?this._get(e,"showCurrentAtPos"):
0),c);this._updateDatepicker(e)}},_gotoToday:function(a){a=d(a);var b=this._getInst(a[0]);if(this._get(b,"gotoCurrent")&&b.currentDay){b.selectedDay=b.currentDay;b.drawMonth=b.selectedMonth=b.currentMonth;b.drawYear=b.selectedYear=b.currentYear}else{var c=new Date;b.selectedDay=c.getDate();b.drawMonth=b.selectedMonth=c.getMonth();b.drawYear=b.selectedYear=c.getFullYear()}this._notifyChange(b);this._adjustDate(a)},_selectMonthYear:function(a,b,c){a=d(a);var e=this._getInst(a[0]);e._selectingMonthYear=
false;e["selected"+(c=="M"?"Month":"Year")]=e["draw"+(c=="M"?"Month":"Year")]=parseInt(b.options[b.selectedIndex].value,10);this._notifyChange(e);this._adjustDate(a)},_clickMonthYear:function(a){var b=this._getInst(d(a)[0]);b.input&&b._selectingMonthYear&&setTimeout(function(){b.input.focus()},0);b._selectingMonthYear=!b._selectingMonthYear},_selectDay:function(a,b,c,e){var f=d(a);if(!(d(e).hasClass(this._unselectableClass)||this._isDisabledDatepicker(f[0]))){f=this._getInst(f[0]);f.selectedDay=f.currentDay=
d("a",e).html();f.selectedMonth=f.currentMonth=b;f.selectedYear=f.currentYear=c;this._selectDate(a,this._formatDate(f,f.currentDay,f.currentMonth,f.currentYear))}},_clearDate:function(a){a=d(a);this._getInst(a[0]);this._selectDate(a,"")},_selectDate:function(a,b){a=this._getInst(d(a)[0]);b=b!=null?b:this._formatDate(a);a.input&&a.input.val(b);this._updateAlternate(a);var c=this._get(a,"onSelect");if(c)c.apply(a.input?a.input[0]:null,[b,a]);else a.input&&a.input.trigger("change");if(a.inline)this._updateDatepicker(a);
else{this._hideDatepicker();this._lastInput=a.input[0];typeof a.input[0]!="object"&&a.input.focus();this._lastInput=null}},_updateAlternate:function(a){var b=this._get(a,"altField");if(b){var c=this._get(a,"altFormat")||this._get(a,"dateFormat"),e=this._getDate(a),f=this.formatDate(c,e,this._getFormatConfig(a));d(b).each(function(){d(this).val(f)})}},noWeekends:function(a){a=a.getDay();return[a>0&&a<6,""]},iso8601Week:function(a){a=new Date(a.getTime());a.setDate(a.getDate()+4-(a.getDay()||7));var b=
a.getTime();a.setMonth(0);a.setDate(1);return Math.floor(Math.round((b-a)/864E5)/7)+1},parseDate:function(a,b,c){if(a==null||b==null)throw"Invalid arguments";b=typeof b=="object"?b.toString():b+"";if(b=="")return null;var e=(c?c.shortYearCutoff:null)||this._defaults.shortYearCutoff;e=typeof e!="string"?e:(new Date).getFullYear()%100+parseInt(e,10);for(var f=(c?c.dayNamesShort:null)||this._defaults.dayNamesShort,h=(c?c.dayNames:null)||this._defaults.dayNames,i=(c?c.monthNamesShort:null)||this._defaults.monthNamesShort,
g=(c?c.monthNames:null)||this._defaults.monthNames,j=c=-1,l=-1,u=-1,k=false,o=function(p){(p=A+1<a.length&&a.charAt(A+1)==p)&&A++;return p},m=function(p){var C=o(p);p=new RegExp("^\\d{1,"+(p=="@"?14:p=="!"?20:p=="y"&&C?4:p=="o"?3:2)+"}");p=b.substring(s).match(p);if(!p)throw"Missing number at position "+s;s+=p[0].length;return parseInt(p[0],10)},n=function(p,C,K){p=d.map(o(p)?K:C,function(w,x){return[[x,w]]}).sort(function(w,x){return-(w[1].length-x[1].length)});var E=-1;d.each(p,function(w,x){w=
x[1];if(b.substr(s,w.length).toLowerCase()==w.toLowerCase()){E=x[0];s+=w.length;return false}});if(E!=-1)return E+1;else throw"Unknown name at position "+s;},r=function(){if(b.charAt(s)!=a.charAt(A))throw"Unexpected literal at position "+s;s++},s=0,A=0;A<a.length;A++)if(k)if(a.charAt(A)=="'"&&!o("'"))k=false;else r();else switch(a.charAt(A)){case "d":l=m("d");break;case "D":n("D",f,h);break;case "o":u=m("o");break;case "m":j=m("m");break;case "M":j=n("M",i,g);break;case "y":c=m("y");break;case "@":var v=
new Date(m("@"));c=v.getFullYear();j=v.getMonth()+1;l=v.getDate();break;case "!":v=new Date((m("!")-this._ticksTo1970)/1E4);c=v.getFullYear();j=v.getMonth()+1;l=v.getDate();break;case "'":if(o("'"))r();else k=true;break;default:r()}if(c==-1)c=(new Date).getFullYear();else if(c<100)c+=(new Date).getFullYear()-(new Date).getFullYear()%100+(c<=e?0:-100);if(u>-1){j=1;l=u;do{e=this._getDaysInMonth(c,j-1);if(l<=e)break;j++;l-=e}while(1)}v=this._daylightSavingAdjust(new Date(c,j-1,l));if(v.getFullYear()!=
c||v.getMonth()+1!=j||v.getDate()!=l)throw"Invalid date";return v},ATOM:"yy-mm-dd",COOKIE:"D, dd M yy",ISO_8601:"yy-mm-dd",RFC_822:"D, d M y",RFC_850:"DD, dd-M-y",RFC_1036:"D, d M y",RFC_1123:"D, d M yy",RFC_2822:"D, d M yy",RSS:"D, d M y",TICKS:"!",TIMESTAMP:"@",W3C:"yy-mm-dd",_ticksTo1970:(718685+Math.floor(492.5)-Math.floor(19.7)+Math.floor(4.925))*24*60*60*1E7,formatDate:function(a,b,c){if(!b)return"";var e=(c?c.dayNamesShort:null)||this._defaults.dayNamesShort,f=(c?c.dayNames:null)||this._defaults.dayNames,
h=(c?c.monthNamesShort:null)||this._defaults.monthNamesShort;c=(c?c.monthNames:null)||this._defaults.monthNames;var i=function(o){(o=k+1<a.length&&a.charAt(k+1)==o)&&k++;return o},g=function(o,m,n){m=""+m;if(i(o))for(;m.length<n;)m="0"+m;return m},j=function(o,m,n,r){return i(o)?r[m]:n[m]},l="",u=false;if(b)for(var k=0;k<a.length;k++)if(u)if(a.charAt(k)=="'"&&!i("'"))u=false;else l+=a.charAt(k);else switch(a.charAt(k)){case "d":l+=g("d",b.getDate(),2);break;case "D":l+=j("D",b.getDay(),e,f);break;
case "o":l+=g("o",(b.getTime()-(new Date(b.getFullYear(),0,0)).getTime())/864E5,3);break;case "m":l+=g("m",b.getMonth()+1,2);break;case "M":l+=j("M",b.getMonth(),h,c);break;case "y":l+=i("y")?b.getFullYear():(b.getYear()%100<10?"0":"")+b.getYear()%100;break;case "@":l+=b.getTime();break;case "!":l+=b.getTime()*1E4+this._ticksTo1970;break;case "'":if(i("'"))l+="'";else u=true;break;default:l+=a.charAt(k)}return l},_possibleChars:function(a){for(var b="",c=false,e=function(h){(h=f+1<a.length&&a.charAt(f+
1)==h)&&f++;return h},f=0;f<a.length;f++)if(c)if(a.charAt(f)=="'"&&!e("'"))c=false;else b+=a.charAt(f);else switch(a.charAt(f)){case "d":case "m":case "y":case "@":b+="0123456789";break;case "D":case "M":return null;case "'":if(e("'"))b+="'";else c=true;break;default:b+=a.charAt(f)}return b},_get:function(a,b){return a.settings[b]!==B?a.settings[b]:this._defaults[b]},_setDateFromField:function(a,b){if(a.input.val()!=a.lastVal){var c=this._get(a,"dateFormat"),e=a.lastVal=a.input?a.input.val():null,
f,h;f=h=this._getDefaultDate(a);var i=this._getFormatConfig(a);try{f=this.parseDate(c,e,i)||h}catch(g){this.log(g);e=b?"":e}a.selectedDay=f.getDate();a.drawMonth=a.selectedMonth=f.getMonth();a.drawYear=a.selectedYear=f.getFullYear();a.currentDay=e?f.getDate():0;a.currentMonth=e?f.getMonth():0;a.currentYear=e?f.getFullYear():0;this._adjustInstDate(a)}},_getDefaultDate:function(a){return this._restrictMinMax(a,this._determineDate(a,this._get(a,"defaultDate"),new Date))},_determineDate:function(a,b,
c){var e=function(h){var i=new Date;i.setDate(i.getDate()+h);return i},f=function(h){try{return d.datepicker.parseDate(d.datepicker._get(a,"dateFormat"),h,d.datepicker._getFormatConfig(a))}catch(i){}var g=(h.toLowerCase().match(/^c/)?d.datepicker._getDate(a):null)||new Date,j=g.getFullYear(),l=g.getMonth();g=g.getDate();for(var u=/([+-]?[0-9]+)\s*(d|D|w|W|m|M|y|Y)?/g,k=u.exec(h);k;){switch(k[2]||"d"){case "d":case "D":g+=parseInt(k[1],10);break;case "w":case "W":g+=parseInt(k[1],10)*7;break;case "m":case "M":l+=
parseInt(k[1],10);g=Math.min(g,d.datepicker._getDaysInMonth(j,l));break;case "y":case "Y":j+=parseInt(k[1],10);g=Math.min(g,d.datepicker._getDaysInMonth(j,l));break}k=u.exec(h)}return new Date(j,l,g)};if(b=(b=b==null||b===""?c:typeof b=="string"?f(b):typeof b=="number"?isNaN(b)?c:e(b):new Date(b.getTime()))&&b.toString()=="Invalid Date"?c:b){b.setHours(0);b.setMinutes(0);b.setSeconds(0);b.setMilliseconds(0)}return this._daylightSavingAdjust(b)},_daylightSavingAdjust:function(a){if(!a)return null;
a.setHours(a.getHours()>12?a.getHours()+2:0);return a},_setDate:function(a,b,c){var e=!b,f=a.selectedMonth,h=a.selectedYear;b=this._restrictMinMax(a,this._determineDate(a,b,new Date));a.selectedDay=a.currentDay=b.getDate();a.drawMonth=a.selectedMonth=a.currentMonth=b.getMonth();a.drawYear=a.selectedYear=a.currentYear=b.getFullYear();if((f!=a.selectedMonth||h!=a.selectedYear)&&!c)this._notifyChange(a);this._adjustInstDate(a);if(a.input)a.input.val(e?"":this._formatDate(a))},_getDate:function(a){return!a.currentYear||
a.input&&a.input.val()==""?null:this._daylightSavingAdjust(new Date(a.currentYear,a.currentMonth,a.currentDay))},_generateHTML:function(a){var b=new Date;b=this._daylightSavingAdjust(new Date(b.getFullYear(),b.getMonth(),b.getDate()));var c=this._get(a,"isRTL"),e=this._get(a,"showButtonPanel"),f=this._get(a,"hideIfNoPrevNext"),h=this._get(a,"navigationAsDateFormat"),i=this._getNumberOfMonths(a),g=this._get(a,"showCurrentAtPos"),j=this._get(a,"stepMonths"),l=i[0]!=1||i[1]!=1,u=this._daylightSavingAdjust(!a.currentDay?
new Date(9999,9,9):new Date(a.currentYear,a.currentMonth,a.currentDay)),k=this._getMinMaxDate(a,"min"),o=this._getMinMaxDate(a,"max");g=a.drawMonth-g;var m=a.drawYear;if(g<0){g+=12;m--}if(o){var n=this._daylightSavingAdjust(new Date(o.getFullYear(),o.getMonth()-i[0]*i[1]+1,o.getDate()));for(n=k&&n<k?k:n;this._daylightSavingAdjust(new Date(m,g,1))>n;){g--;if(g<0){g=11;m--}}}a.drawMonth=g;a.drawYear=m;n=this._get(a,"prevText");n=!h?n:this.formatDate(n,this._daylightSavingAdjust(new Date(m,g-j,1)),this._getFormatConfig(a));
n=this._canAdjustMonth(a,-1,m,g)?'<a class="ui-datepicker-prev ui-corner-all" onclick="DP_jQuery_'+z+".datepicker._adjustDate('#"+a.id+"', -"+j+", 'M');\" title=\""+n+'"><span class="ui-icon ui-icon-circle-triangle-'+(c?"e":"w")+'">'+n+"</span></a>":f?"":'<a class="ui-datepicker-prev ui-corner-all ui-state-disabled" title="'+n+'"><span class="ui-icon ui-icon-circle-triangle-'+(c?"e":"w")+'">'+n+"</span></a>";var r=this._get(a,"nextText");r=!h?r:this.formatDate(r,this._daylightSavingAdjust(new Date(m,
g+j,1)),this._getFormatConfig(a));f=this._canAdjustMonth(a,+1,m,g)?'<a class="ui-datepicker-next ui-corner-all" onclick="DP_jQuery_'+z+".datepicker._adjustDate('#"+a.id+"', +"+j+", 'M');\" title=\""+r+'"><span class="ui-icon ui-icon-circle-triangle-'+(c?"w":"e")+'">'+r+"</span></a>":f?"":'<a class="ui-datepicker-next ui-corner-all ui-state-disabled" title="'+r+'"><span class="ui-icon ui-icon-circle-triangle-'+(c?"w":"e")+'">'+r+"</span></a>";j=this._get(a,"currentText");r=this._get(a,"gotoCurrent")&&
a.currentDay?u:b;j=!h?j:this.formatDate(j,r,this._getFormatConfig(a));h=!a.inline?'<button type="button" class="ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all" onclick="DP_jQuery_'+z+'.datepicker._hideDatepicker();">'+this._get(a,"closeText")+"</button>":"";e=e?'<div class="ui-datepicker-buttonpane ui-widget-content">'+(c?h:"")+(this._isInRange(a,r)?'<button type="button" class="ui-datepicker-current ui-state-default ui-priority-secondary ui-corner-all" onclick="DP_jQuery_'+
z+".datepicker._gotoToday('#"+a.id+"');\">"+j+"</button>":"")+(c?"":h)+"</div>":"";h=parseInt(this._get(a,"firstDay"),10);h=isNaN(h)?0:h;j=this._get(a,"showWeek");r=this._get(a,"dayNames");this._get(a,"dayNamesShort");var s=this._get(a,"dayNamesMin"),A=this._get(a,"monthNames"),v=this._get(a,"monthNamesShort"),p=this._get(a,"beforeShowDay"),C=this._get(a,"showOtherMonths"),K=this._get(a,"selectOtherMonths");this._get(a,"calculateWeek");for(var E=this._getDefaultDate(a),w="",x=0;x<i[0];x++){for(var O=
"",G=0;G<i[1];G++){var P=this._daylightSavingAdjust(new Date(m,g,a.selectedDay)),t=" ui-corner-all",y="";if(l){y+='<div class="ui-datepicker-group';if(i[1]>1)switch(G){case 0:y+=" ui-datepicker-group-first";t=" ui-corner-"+(c?"right":"left");break;case i[1]-1:y+=" ui-datepicker-group-last";t=" ui-corner-"+(c?"left":"right");break;default:y+=" ui-datepicker-group-middle";t="";break}y+='">'}y+='<div class="ui-datepicker-header ui-widget-header ui-helper-clearfix'+t+'">'+(/all|left/.test(t)&&x==0?c?
f:n:"")+(/all|right/.test(t)&&x==0?c?n:f:"")+this._generateMonthYearHeader(a,g,m,k,o,x>0||G>0,A,v)+'</div><table class="ui-datepicker-calendar"><thead><tr>';var D=j?'<th class="ui-datepicker-week-col">'+this._get(a,"weekHeader")+"</th>":"";for(t=0;t<7;t++){var q=(t+h)%7;D+="<th"+((t+h+6)%7>=5?' class="ui-datepicker-week-end"':"")+'><span title="'+r[q]+'">'+s[q]+"</span></th>"}y+=D+"</tr></thead><tbody>";D=this._getDaysInMonth(m,g);if(m==a.selectedYear&&g==a.selectedMonth)a.selectedDay=Math.min(a.selectedDay,
D);t=(this._getFirstDayOfMonth(m,g)-h+7)%7;D=l?6:Math.ceil((t+D)/7);q=this._daylightSavingAdjust(new Date(m,g,1-t));for(var Q=0;Q<D;Q++){y+="<tr>";var R=!j?"":'<td class="ui-datepicker-week-col">'+this._get(a,"calculateWeek")(q)+"</td>";for(t=0;t<7;t++){var I=p?p.apply(a.input?a.input[0]:null,[q]):[true,""],F=q.getMonth()!=g,L=F&&!K||!I[0]||k&&q<k||o&&q>o;R+='<td class="'+((t+h+6)%7>=5?" ui-datepicker-week-end":"")+(F?" ui-datepicker-other-month":"")+(q.getTime()==P.getTime()&&g==a.selectedMonth&&
a._keyEvent||E.getTime()==q.getTime()&&E.getTime()==P.getTime()?" "+this._dayOverClass:"")+(L?" "+this._unselectableClass+" ui-state-disabled":"")+(F&&!C?"":" "+I[1]+(q.getTime()==u.getTime()?" "+this._currentClass:"")+(q.getTime()==b.getTime()?" ui-datepicker-today":""))+'"'+((!F||C)&&I[2]?' title="'+I[2]+'"':"")+(L?"":' onclick="DP_jQuery_'+z+".datepicker._selectDay('#"+a.id+"',"+q.getMonth()+","+q.getFullYear()+', this);return false;"')+">"+(F&&!C?"&#xa0;":L?'<span class="ui-state-default">'+q.getDate()+
"</span>":'<a class="ui-state-default'+(q.getTime()==b.getTime()?" ui-state-highlight":"")+(q.getTime()==u.getTime()?" ui-state-active":"")+(F?" ui-priority-secondary":"")+'" href="#">'+q.getDate()+"</a>")+"</td>";q.setDate(q.getDate()+1);q=this._daylightSavingAdjust(q)}y+=R+"</tr>"}g++;if(g>11){g=0;m++}y+="</tbody></table>"+(l?"</div>"+(i[0]>0&&G==i[1]-1?'<div class="ui-datepicker-row-break"></div>':""):"");O+=y}w+=O}w+=e+(d.browser.msie&&parseInt(d.browser.version,10)<7&&!a.inline?'<iframe src="javascript:false;" class="ui-datepicker-cover" frameborder="0"></iframe>':
"");a._keyEvent=false;return w},_generateMonthYearHeader:function(a,b,c,e,f,h,i,g){var j=this._get(a,"changeMonth"),l=this._get(a,"changeYear"),u=this._get(a,"showMonthAfterYear"),k='<div class="ui-datepicker-title">',o="";if(h||!j)o+='<span class="ui-datepicker-month">'+i[b]+"</span>";else{i=e&&e.getFullYear()==c;var m=f&&f.getFullYear()==c;o+='<select class="ui-datepicker-month" onchange="DP_jQuery_'+z+".datepicker._selectMonthYear('#"+a.id+"', this, 'M');\" onclick=\"DP_jQuery_"+z+".datepicker._clickMonthYear('#"+
a.id+"');\">";for(var n=0;n<12;n++)if((!i||n>=e.getMonth())&&(!m||n<=f.getMonth()))o+='<option value="'+n+'"'+(n==b?' selected="selected"':"")+">"+g[n]+"</option>";o+="</select>"}u||(k+=o+(h||!(j&&l)?"&#xa0;":""));if(!a.yearshtml){a.yearshtml="";if(h||!l)k+='<span class="ui-datepicker-year">'+c+"</span>";else{g=this._get(a,"yearRange").split(":");var r=(new Date).getFullYear();i=function(s){s=s.match(/c[+-].*/)?c+parseInt(s.substring(1),10):s.match(/[+-].*/)?r+parseInt(s,10):parseInt(s,10);return isNaN(s)?
r:s};b=i(g[0]);g=Math.max(b,i(g[1]||""));b=e?Math.max(b,e.getFullYear()):b;g=f?Math.min(g,f.getFullYear()):g;for(a.yearshtml+='<select class="ui-datepicker-year" onchange="DP_jQuery_'+z+".datepicker._selectMonthYear('#"+a.id+"', this, 'Y');\" onclick=\"DP_jQuery_"+z+".datepicker._clickMonthYear('#"+a.id+"');\">";b<=g;b++)a.yearshtml+='<option value="'+b+'"'+(b==c?' selected="selected"':"")+">"+b+"</option>";a.yearshtml+="</select>";k+=a.yearshtml;a.yearshtml=null}}k+=this._get(a,"yearSuffix");if(u)k+=
(h||!(j&&l)?"&#xa0;":"")+o;k+="</div>";return k},_adjustInstDate:function(a,b,c){var e=a.drawYear+(c=="Y"?b:0),f=a.drawMonth+(c=="M"?b:0);b=Math.min(a.selectedDay,this._getDaysInMonth(e,f))+(c=="D"?b:0);e=this._restrictMinMax(a,this._daylightSavingAdjust(new Date(e,f,b)));a.selectedDay=e.getDate();a.drawMonth=a.selectedMonth=e.getMonth();a.drawYear=a.selectedYear=e.getFullYear();if(c=="M"||c=="Y")this._notifyChange(a)},_restrictMinMax:function(a,b){var c=this._getMinMaxDate(a,"min");a=this._getMinMaxDate(a,
"max");b=c&&b<c?c:b;return b=a&&b>a?a:b},_notifyChange:function(a){var b=this._get(a,"onChangeMonthYear");if(b)b.apply(a.input?a.input[0]:null,[a.selectedYear,a.selectedMonth+1,a])},_getNumberOfMonths:function(a){a=this._get(a,"numberOfMonths");return a==null?[1,1]:typeof a=="number"?[1,a]:a},_getMinMaxDate:function(a,b){return this._determineDate(a,this._get(a,b+"Date"),null)},_getDaysInMonth:function(a,b){return 32-this._daylightSavingAdjust(new Date(a,b,32)).getDate()},_getFirstDayOfMonth:function(a,
b){return(new Date(a,b,1)).getDay()},_canAdjustMonth:function(a,b,c,e){var f=this._getNumberOfMonths(a);c=this._daylightSavingAdjust(new Date(c,e+(b<0?b:f[0]*f[1]),1));b<0&&c.setDate(this._getDaysInMonth(c.getFullYear(),c.getMonth()));return this._isInRange(a,c)},_isInRange:function(a,b){var c=this._getMinMaxDate(a,"min");a=this._getMinMaxDate(a,"max");return(!c||b.getTime()>=c.getTime())&&(!a||b.getTime()<=a.getTime())},_getFormatConfig:function(a){var b=this._get(a,"shortYearCutoff");b=typeof b!=
"string"?b:(new Date).getFullYear()%100+parseInt(b,10);return{shortYearCutoff:b,dayNamesShort:this._get(a,"dayNamesShort"),dayNames:this._get(a,"dayNames"),monthNamesShort:this._get(a,"monthNamesShort"),monthNames:this._get(a,"monthNames")}},_formatDate:function(a,b,c,e){if(!b){a.currentDay=a.selectedDay;a.currentMonth=a.selectedMonth;a.currentYear=a.selectedYear}b=b?typeof b=="object"?b:this._daylightSavingAdjust(new Date(e,c,b)):this._daylightSavingAdjust(new Date(a.currentYear,a.currentMonth,a.currentDay));
return this.formatDate(this._get(a,"dateFormat"),b,this._getFormatConfig(a))}});d.fn.datepicker=function(a){if(!this.length)return this;if(!d.datepicker.initialized){d(document).mousedown(d.datepicker._checkExternalClick).find("body").append(d.datepicker.dpDiv);d.datepicker.initialized=true}var b=Array.prototype.slice.call(arguments,1);if(typeof a=="string"&&(a=="isDisabled"||a=="getDate"||a=="widget"))return d.datepicker["_"+a+"Datepicker"].apply(d.datepicker,[this[0]].concat(b));if(a=="option"&&
arguments.length==2&&typeof arguments[1]=="string")return d.datepicker["_"+a+"Datepicker"].apply(d.datepicker,[this[0]].concat(b));return this.each(function(){typeof a=="string"?d.datepicker["_"+a+"Datepicker"].apply(d.datepicker,[this].concat(b)):d.datepicker._attachDatepicker(this,a)})};d.datepicker=new M;d.datepicker.initialized=false;d.datepicker.uuid=(new Date).getTime();d.datepicker.version="1.8.13";window["DP_jQuery_"+z]=d})(jQuery);
;/*
 * jQuery UI Progressbar 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Progressbar
 *
 * Depends:
 *   jquery.ui.core.js
 *   jquery.ui.widget.js
 */
(function(b,d){b.widget("ui.progressbar",{options:{value:0,max:100},min:0,_create:function(){this.element.addClass("ui-progressbar ui-widget ui-widget-content ui-corner-all").attr({role:"progressbar","aria-valuemin":this.min,"aria-valuemax":this.options.max,"aria-valuenow":this._value()});this.valueDiv=b("<div class='ui-progressbar-value ui-widget-header ui-corner-left'></div>").appendTo(this.element);this.oldValue=this._value();this._refreshValue()},destroy:function(){this.element.removeClass("ui-progressbar ui-widget ui-widget-content ui-corner-all").removeAttr("role").removeAttr("aria-valuemin").removeAttr("aria-valuemax").removeAttr("aria-valuenow");
this.valueDiv.remove();b.Widget.prototype.destroy.apply(this,arguments)},value:function(a){if(a===d)return this._value();this._setOption("value",a);return this},_setOption:function(a,c){if(a==="value"){this.options.value=c;this._refreshValue();this._value()===this.options.max&&this._trigger("complete")}b.Widget.prototype._setOption.apply(this,arguments)},_value:function(){var a=this.options.value;if(typeof a!=="number")a=0;return Math.min(this.options.max,Math.max(this.min,a))},_percentage:function(){return 100*
this._value()/this.options.max},_refreshValue:function(){var a=this.value(),c=this._percentage();if(this.oldValue!==a){this.oldValue=a;this._trigger("change")}this.valueDiv.toggle(a>this.min).toggleClass("ui-corner-right",a===this.options.max).width(c.toFixed(0)+"%");this.element.attr("aria-valuenow",a)}});b.extend(b.ui.progressbar,{version:"1.8.13"})})(jQuery);
;/*
 * jQuery UI Effects 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Effects/
 */
jQuery.effects||function(f,j){function m(c){var a;if(c&&c.constructor==Array&&c.length==3)return c;if(a=/rgb\(\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*\)/.exec(c))return[parseInt(a[1],10),parseInt(a[2],10),parseInt(a[3],10)];if(a=/rgb\(\s*([0-9]+(?:\.[0-9]+)?)\%\s*,\s*([0-9]+(?:\.[0-9]+)?)\%\s*,\s*([0-9]+(?:\.[0-9]+)?)\%\s*\)/.exec(c))return[parseFloat(a[1])*2.55,parseFloat(a[2])*2.55,parseFloat(a[3])*2.55];if(a=/#([a-fA-F0-9]{2})([a-fA-F0-9]{2})([a-fA-F0-9]{2})/.exec(c))return[parseInt(a[1],
16),parseInt(a[2],16),parseInt(a[3],16)];if(a=/#([a-fA-F0-9])([a-fA-F0-9])([a-fA-F0-9])/.exec(c))return[parseInt(a[1]+a[1],16),parseInt(a[2]+a[2],16),parseInt(a[3]+a[3],16)];if(/rgba\(0, 0, 0, 0\)/.exec(c))return n.transparent;return n[f.trim(c).toLowerCase()]}function s(c,a){var b;do{b=f.curCSS(c,a);if(b!=""&&b!="transparent"||f.nodeName(c,"body"))break;a="backgroundColor"}while(c=c.parentNode);return m(b)}function o(){var c=document.defaultView?document.defaultView.getComputedStyle(this,null):this.currentStyle,
a={},b,d;if(c&&c.length&&c[0]&&c[c[0]])for(var e=c.length;e--;){b=c[e];if(typeof c[b]=="string"){d=b.replace(/\-(\w)/g,function(g,h){return h.toUpperCase()});a[d]=c[b]}}else for(b in c)if(typeof c[b]==="string")a[b]=c[b];return a}function p(c){var a,b;for(a in c){b=c[a];if(b==null||f.isFunction(b)||a in t||/scrollbar/.test(a)||!/color/i.test(a)&&isNaN(parseFloat(b)))delete c[a]}return c}function u(c,a){var b={_:0},d;for(d in a)if(c[d]!=a[d])b[d]=a[d];return b}function k(c,a,b,d){if(typeof c=="object"){d=
a;b=null;a=c;c=a.effect}if(f.isFunction(a)){d=a;b=null;a={}}if(typeof a=="number"||f.fx.speeds[a]){d=b;b=a;a={}}if(f.isFunction(b)){d=b;b=null}a=a||{};b=b||a.duration;b=f.fx.off?0:typeof b=="number"?b:b in f.fx.speeds?f.fx.speeds[b]:f.fx.speeds._default;d=d||a.complete;return[c,a,b,d]}function l(c){if(!c||typeof c==="number"||f.fx.speeds[c])return true;if(typeof c==="string"&&!f.effects[c])return true;return false}f.effects={};f.each(["backgroundColor","borderBottomColor","borderLeftColor","borderRightColor",
"borderTopColor","borderColor","color","outlineColor"],function(c,a){f.fx.step[a]=function(b){if(!b.colorInit){b.start=s(b.elem,a);b.end=m(b.end);b.colorInit=true}b.elem.style[a]="rgb("+Math.max(Math.min(parseInt(b.pos*(b.end[0]-b.start[0])+b.start[0],10),255),0)+","+Math.max(Math.min(parseInt(b.pos*(b.end[1]-b.start[1])+b.start[1],10),255),0)+","+Math.max(Math.min(parseInt(b.pos*(b.end[2]-b.start[2])+b.start[2],10),255),0)+")"}});var n={aqua:[0,255,255],azure:[240,255,255],beige:[245,245,220],black:[0,
0,0],blue:[0,0,255],brown:[165,42,42],cyan:[0,255,255],darkblue:[0,0,139],darkcyan:[0,139,139],darkgrey:[169,169,169],darkgreen:[0,100,0],darkkhaki:[189,183,107],darkmagenta:[139,0,139],darkolivegreen:[85,107,47],darkorange:[255,140,0],darkorchid:[153,50,204],darkred:[139,0,0],darksalmon:[233,150,122],darkviolet:[148,0,211],fuchsia:[255,0,255],gold:[255,215,0],green:[0,128,0],indigo:[75,0,130],khaki:[240,230,140],lightblue:[173,216,230],lightcyan:[224,255,255],lightgreen:[144,238,144],lightgrey:[211,
211,211],lightpink:[255,182,193],lightyellow:[255,255,224],lime:[0,255,0],magenta:[255,0,255],maroon:[128,0,0],navy:[0,0,128],olive:[128,128,0],orange:[255,165,0],pink:[255,192,203],purple:[128,0,128],violet:[128,0,128],red:[255,0,0],silver:[192,192,192],white:[255,255,255],yellow:[255,255,0],transparent:[255,255,255]},q=["add","remove","toggle"],t={border:1,borderBottom:1,borderColor:1,borderLeft:1,borderRight:1,borderTop:1,borderWidth:1,margin:1,padding:1};f.effects.animateClass=function(c,a,b,
d){if(f.isFunction(b)){d=b;b=null}return this.queue(function(){var e=f(this),g=e.attr("style")||" ",h=p(o.call(this)),r,v=e.attr("class");f.each(q,function(w,i){c[i]&&e[i+"Class"](c[i])});r=p(o.call(this));e.attr("class",v);e.animate(u(h,r),{queue:false,duration:a,easding:b,complete:function(){f.each(q,function(w,i){c[i]&&e[i+"Class"](c[i])});if(typeof e.attr("style")=="object"){e.attr("style").cssText="";e.attr("style").cssText=g}else e.attr("style",g);d&&d.apply(this,arguments);f.dequeue(this)}})})};
f.fn.extend({_addClass:f.fn.addClass,addClass:function(c,a,b,d){return a?f.effects.animateClass.apply(this,[{add:c},a,b,d]):this._addClass(c)},_removeClass:f.fn.removeClass,removeClass:function(c,a,b,d){return a?f.effects.animateClass.apply(this,[{remove:c},a,b,d]):this._removeClass(c)},_toggleClass:f.fn.toggleClass,toggleClass:function(c,a,b,d,e){return typeof a=="boolean"||a===j?b?f.effects.animateClass.apply(this,[a?{add:c}:{remove:c},b,d,e]):this._toggleClass(c,a):f.effects.animateClass.apply(this,
[{toggle:c},a,b,d])},switchClass:function(c,a,b,d,e){return f.effects.animateClass.apply(this,[{add:a,remove:c},b,d,e])}});f.extend(f.effects,{version:"1.8.13",save:function(c,a){for(var b=0;b<a.length;b++)a[b]!==null&&c.data("ec.storage."+a[b],c[0].style[a[b]])},restore:function(c,a){for(var b=0;b<a.length;b++)a[b]!==null&&c.css(a[b],c.data("ec.storage."+a[b]))},setMode:function(c,a){if(a=="toggle")a=c.is(":hidden")?"show":"hide";return a},getBaseline:function(c,a){var b;switch(c[0]){case "top":b=
0;break;case "middle":b=0.5;break;case "bottom":b=1;break;default:b=c[0]/a.height}switch(c[1]){case "left":c=0;break;case "center":c=0.5;break;case "right":c=1;break;default:c=c[1]/a.width}return{x:c,y:b}},createWrapper:function(c){if(c.parent().is(".ui-effects-wrapper"))return c.parent();var a={width:c.outerWidth(true),height:c.outerHeight(true),"float":c.css("float")},b=f("<div></div>").addClass("ui-effects-wrapper").css({fontSize:"100%",background:"transparent",border:"none",margin:0,padding:0});
c.wrap(b);b=c.parent();if(c.css("position")=="static"){b.css({position:"relative"});c.css({position:"relative"})}else{f.extend(a,{position:c.css("position"),zIndex:c.css("z-index")});f.each(["top","left","bottom","right"],function(d,e){a[e]=c.css(e);if(isNaN(parseInt(a[e],10)))a[e]="auto"});c.css({position:"relative",top:0,left:0,right:"auto",bottom:"auto"})}return b.css(a).show()},removeWrapper:function(c){if(c.parent().is(".ui-effects-wrapper"))return c.parent().replaceWith(c);return c},setTransition:function(c,
a,b,d){d=d||{};f.each(a,function(e,g){unit=c.cssUnit(g);if(unit[0]>0)d[g]=unit[0]*b+unit[1]});return d}});f.fn.extend({effect:function(c){var a=k.apply(this,arguments),b={options:a[1],duration:a[2],callback:a[3]};a=b.options.mode;var d=f.effects[c];if(f.fx.off||!d)return a?this[a](b.duration,b.callback):this.each(function(){b.callback&&b.callback.call(this)});return d.call(this,b)},_show:f.fn.show,show:function(c){if(l(c))return this._show.apply(this,arguments);else{var a=k.apply(this,arguments);
a[1].mode="show";return this.effect.apply(this,a)}},_hide:f.fn.hide,hide:function(c){if(l(c))return this._hide.apply(this,arguments);else{var a=k.apply(this,arguments);a[1].mode="hide";return this.effect.apply(this,a)}},__toggle:f.fn.toggle,toggle:function(c){if(l(c)||typeof c==="boolean"||f.isFunction(c))return this.__toggle.apply(this,arguments);else{var a=k.apply(this,arguments);a[1].mode="toggle";return this.effect.apply(this,a)}},cssUnit:function(c){var a=this.css(c),b=[];f.each(["em","px","%",
"pt"],function(d,e){if(a.indexOf(e)>0)b=[parseFloat(a),e]});return b}});f.easing.jswing=f.easing.swing;f.extend(f.easing,{def:"easeOutQuad",swing:function(c,a,b,d,e){return f.easing[f.easing.def](c,a,b,d,e)},easeInQuad:function(c,a,b,d,e){return d*(a/=e)*a+b},easeOutQuad:function(c,a,b,d,e){return-d*(a/=e)*(a-2)+b},easeInOutQuad:function(c,a,b,d,e){if((a/=e/2)<1)return d/2*a*a+b;return-d/2*(--a*(a-2)-1)+b},easeInCubic:function(c,a,b,d,e){return d*(a/=e)*a*a+b},easeOutCubic:function(c,a,b,d,e){return d*
((a=a/e-1)*a*a+1)+b},easeInOutCubic:function(c,a,b,d,e){if((a/=e/2)<1)return d/2*a*a*a+b;return d/2*((a-=2)*a*a+2)+b},easeInQuart:function(c,a,b,d,e){return d*(a/=e)*a*a*a+b},easeOutQuart:function(c,a,b,d,e){return-d*((a=a/e-1)*a*a*a-1)+b},easeInOutQuart:function(c,a,b,d,e){if((a/=e/2)<1)return d/2*a*a*a*a+b;return-d/2*((a-=2)*a*a*a-2)+b},easeInQuint:function(c,a,b,d,e){return d*(a/=e)*a*a*a*a+b},easeOutQuint:function(c,a,b,d,e){return d*((a=a/e-1)*a*a*a*a+1)+b},easeInOutQuint:function(c,a,b,d,e){if((a/=
e/2)<1)return d/2*a*a*a*a*a+b;return d/2*((a-=2)*a*a*a*a+2)+b},easeInSine:function(c,a,b,d,e){return-d*Math.cos(a/e*(Math.PI/2))+d+b},easeOutSine:function(c,a,b,d,e){return d*Math.sin(a/e*(Math.PI/2))+b},easeInOutSine:function(c,a,b,d,e){return-d/2*(Math.cos(Math.PI*a/e)-1)+b},easeInExpo:function(c,a,b,d,e){return a==0?b:d*Math.pow(2,10*(a/e-1))+b},easeOutExpo:function(c,a,b,d,e){return a==e?b+d:d*(-Math.pow(2,-10*a/e)+1)+b},easeInOutExpo:function(c,a,b,d,e){if(a==0)return b;if(a==e)return b+d;if((a/=
e/2)<1)return d/2*Math.pow(2,10*(a-1))+b;return d/2*(-Math.pow(2,-10*--a)+2)+b},easeInCirc:function(c,a,b,d,e){return-d*(Math.sqrt(1-(a/=e)*a)-1)+b},easeOutCirc:function(c,a,b,d,e){return d*Math.sqrt(1-(a=a/e-1)*a)+b},easeInOutCirc:function(c,a,b,d,e){if((a/=e/2)<1)return-d/2*(Math.sqrt(1-a*a)-1)+b;return d/2*(Math.sqrt(1-(a-=2)*a)+1)+b},easeInElastic:function(c,a,b,d,e){c=1.70158;var g=0,h=d;if(a==0)return b;if((a/=e)==1)return b+d;g||(g=e*0.3);if(h<Math.abs(d)){h=d;c=g/4}else c=g/(2*Math.PI)*Math.asin(d/
h);return-(h*Math.pow(2,10*(a-=1))*Math.sin((a*e-c)*2*Math.PI/g))+b},easeOutElastic:function(c,a,b,d,e){c=1.70158;var g=0,h=d;if(a==0)return b;if((a/=e)==1)return b+d;g||(g=e*0.3);if(h<Math.abs(d)){h=d;c=g/4}else c=g/(2*Math.PI)*Math.asin(d/h);return h*Math.pow(2,-10*a)*Math.sin((a*e-c)*2*Math.PI/g)+d+b},easeInOutElastic:function(c,a,b,d,e){c=1.70158;var g=0,h=d;if(a==0)return b;if((a/=e/2)==2)return b+d;g||(g=e*0.3*1.5);if(h<Math.abs(d)){h=d;c=g/4}else c=g/(2*Math.PI)*Math.asin(d/h);if(a<1)return-0.5*
h*Math.pow(2,10*(a-=1))*Math.sin((a*e-c)*2*Math.PI/g)+b;return h*Math.pow(2,-10*(a-=1))*Math.sin((a*e-c)*2*Math.PI/g)*0.5+d+b},easeInBack:function(c,a,b,d,e,g){if(g==j)g=1.70158;return d*(a/=e)*a*((g+1)*a-g)+b},easeOutBack:function(c,a,b,d,e,g){if(g==j)g=1.70158;return d*((a=a/e-1)*a*((g+1)*a+g)+1)+b},easeInOutBack:function(c,a,b,d,e,g){if(g==j)g=1.70158;if((a/=e/2)<1)return d/2*a*a*(((g*=1.525)+1)*a-g)+b;return d/2*((a-=2)*a*(((g*=1.525)+1)*a+g)+2)+b},easeInBounce:function(c,a,b,d,e){return d-f.easing.easeOutBounce(c,
e-a,0,d,e)+b},easeOutBounce:function(c,a,b,d,e){return(a/=e)<1/2.75?d*7.5625*a*a+b:a<2/2.75?d*(7.5625*(a-=1.5/2.75)*a+0.75)+b:a<2.5/2.75?d*(7.5625*(a-=2.25/2.75)*a+0.9375)+b:d*(7.5625*(a-=2.625/2.75)*a+0.984375)+b},easeInOutBounce:function(c,a,b,d,e){if(a<e/2)return f.easing.easeInBounce(c,a*2,0,d,e)*0.5+b;return f.easing.easeOutBounce(c,a*2-e,0,d,e)*0.5+d*0.5+b}})}(jQuery);
;/*
 * jQuery UI Effects Blind 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Effects/Blind
 *
 * Depends:
 *	jquery.effects.core.js
 */
(function(b){b.effects.blind=function(c){return this.queue(function(){var a=b(this),g=["position","top","bottom","left","right"],f=b.effects.setMode(a,c.options.mode||"hide"),d=c.options.direction||"vertical";b.effects.save(a,g);a.show();var e=b.effects.createWrapper(a).css({overflow:"hidden"}),h=d=="vertical"?"height":"width";d=d=="vertical"?e.height():e.width();f=="show"&&e.css(h,0);var i={};i[h]=f=="show"?d:0;e.animate(i,c.duration,c.options.easing,function(){f=="hide"&&a.hide();b.effects.restore(a,
g);b.effects.removeWrapper(a);c.callback&&c.callback.apply(a[0],arguments);a.dequeue()})})}})(jQuery);
;/*
 * jQuery UI Effects Bounce 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Effects/Bounce
 *
 * Depends:
 *	jquery.effects.core.js
 */
(function(e){e.effects.bounce=function(b){return this.queue(function(){var a=e(this),l=["position","top","bottom","left","right"],h=e.effects.setMode(a,b.options.mode||"effect"),d=b.options.direction||"up",c=b.options.distance||20,m=b.options.times||5,i=b.duration||250;/show|hide/.test(h)&&l.push("opacity");e.effects.save(a,l);a.show();e.effects.createWrapper(a);var f=d=="up"||d=="down"?"top":"left";d=d=="up"||d=="left"?"pos":"neg";c=b.options.distance||(f=="top"?a.outerHeight({margin:true})/3:a.outerWidth({margin:true})/
3);if(h=="show")a.css("opacity",0).css(f,d=="pos"?-c:c);if(h=="hide")c/=m*2;h!="hide"&&m--;if(h=="show"){var g={opacity:1};g[f]=(d=="pos"?"+=":"-=")+c;a.animate(g,i/2,b.options.easing);c/=2;m--}for(g=0;g<m;g++){var j={},k={};j[f]=(d=="pos"?"-=":"+=")+c;k[f]=(d=="pos"?"+=":"-=")+c;a.animate(j,i/2,b.options.easing).animate(k,i/2,b.options.easing);c=h=="hide"?c*2:c/2}if(h=="hide"){g={opacity:0};g[f]=(d=="pos"?"-=":"+=")+c;a.animate(g,i/2,b.options.easing,function(){a.hide();e.effects.restore(a,l);e.effects.removeWrapper(a);
b.callback&&b.callback.apply(this,arguments)})}else{j={};k={};j[f]=(d=="pos"?"-=":"+=")+c;k[f]=(d=="pos"?"+=":"-=")+c;a.animate(j,i/2,b.options.easing).animate(k,i/2,b.options.easing,function(){e.effects.restore(a,l);e.effects.removeWrapper(a);b.callback&&b.callback.apply(this,arguments)})}a.queue("fx",function(){a.dequeue()});a.dequeue()})}})(jQuery);
;/*
 * jQuery UI Effects Clip 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Effects/Clip
 *
 * Depends:
 *	jquery.effects.core.js
 */
(function(b){b.effects.clip=function(e){return this.queue(function(){var a=b(this),i=["position","top","bottom","left","right","height","width"],f=b.effects.setMode(a,e.options.mode||"hide"),c=e.options.direction||"vertical";b.effects.save(a,i);a.show();var d=b.effects.createWrapper(a).css({overflow:"hidden"});d=a[0].tagName=="IMG"?d:a;var g={size:c=="vertical"?"height":"width",position:c=="vertical"?"top":"left"};c=c=="vertical"?d.height():d.width();if(f=="show"){d.css(g.size,0);d.css(g.position,
c/2)}var h={};h[g.size]=f=="show"?c:0;h[g.position]=f=="show"?0:c/2;d.animate(h,{queue:false,duration:e.duration,easing:e.options.easing,complete:function(){f=="hide"&&a.hide();b.effects.restore(a,i);b.effects.removeWrapper(a);e.callback&&e.callback.apply(a[0],arguments);a.dequeue()}})})}})(jQuery);
;/*
 * jQuery UI Effects Drop 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Effects/Drop
 *
 * Depends:
 *	jquery.effects.core.js
 */
(function(c){c.effects.drop=function(d){return this.queue(function(){var a=c(this),h=["position","top","bottom","left","right","opacity"],e=c.effects.setMode(a,d.options.mode||"hide"),b=d.options.direction||"left";c.effects.save(a,h);a.show();c.effects.createWrapper(a);var f=b=="up"||b=="down"?"top":"left";b=b=="up"||b=="left"?"pos":"neg";var g=d.options.distance||(f=="top"?a.outerHeight({margin:true})/2:a.outerWidth({margin:true})/2);if(e=="show")a.css("opacity",0).css(f,b=="pos"?-g:g);var i={opacity:e==
"show"?1:0};i[f]=(e=="show"?b=="pos"?"+=":"-=":b=="pos"?"-=":"+=")+g;a.animate(i,{queue:false,duration:d.duration,easing:d.options.easing,complete:function(){e=="hide"&&a.hide();c.effects.restore(a,h);c.effects.removeWrapper(a);d.callback&&d.callback.apply(this,arguments);a.dequeue()}})})}})(jQuery);
;/*
 * jQuery UI Effects Explode 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Effects/Explode
 *
 * Depends:
 *	jquery.effects.core.js
 */
(function(j){j.effects.explode=function(a){return this.queue(function(){var c=a.options.pieces?Math.round(Math.sqrt(a.options.pieces)):3,d=a.options.pieces?Math.round(Math.sqrt(a.options.pieces)):3;a.options.mode=a.options.mode=="toggle"?j(this).is(":visible")?"hide":"show":a.options.mode;var b=j(this).show().css("visibility","hidden"),g=b.offset();g.top-=parseInt(b.css("marginTop"),10)||0;g.left-=parseInt(b.css("marginLeft"),10)||0;for(var h=b.outerWidth(true),i=b.outerHeight(true),e=0;e<c;e++)for(var f=
0;f<d;f++)b.clone().appendTo("body").wrap("<div></div>").css({position:"absolute",visibility:"visible",left:-f*(h/d),top:-e*(i/c)}).parent().addClass("ui-effects-explode").css({position:"absolute",overflow:"hidden",width:h/d,height:i/c,left:g.left+f*(h/d)+(a.options.mode=="show"?(f-Math.floor(d/2))*(h/d):0),top:g.top+e*(i/c)+(a.options.mode=="show"?(e-Math.floor(c/2))*(i/c):0),opacity:a.options.mode=="show"?0:1}).animate({left:g.left+f*(h/d)+(a.options.mode=="show"?0:(f-Math.floor(d/2))*(h/d)),top:g.top+
e*(i/c)+(a.options.mode=="show"?0:(e-Math.floor(c/2))*(i/c)),opacity:a.options.mode=="show"?1:0},a.duration||500);setTimeout(function(){a.options.mode=="show"?b.css({visibility:"visible"}):b.css({visibility:"visible"}).hide();a.callback&&a.callback.apply(b[0]);b.dequeue();j("div.ui-effects-explode").remove()},a.duration||500)})}})(jQuery);
;/*
 * jQuery UI Effects Fade 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Effects/Fade
 *
 * Depends:
 *	jquery.effects.core.js
 */
(function(b){b.effects.fade=function(a){return this.queue(function(){var c=b(this),d=b.effects.setMode(c,a.options.mode||"hide");c.animate({opacity:d},{queue:false,duration:a.duration,easing:a.options.easing,complete:function(){a.callback&&a.callback.apply(this,arguments);c.dequeue()}})})}})(jQuery);
;/*
 * jQuery UI Effects Fold 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Effects/Fold
 *
 * Depends:
 *	jquery.effects.core.js
 */
(function(c){c.effects.fold=function(a){return this.queue(function(){var b=c(this),j=["position","top","bottom","left","right"],d=c.effects.setMode(b,a.options.mode||"hide"),g=a.options.size||15,h=!!a.options.horizFirst,k=a.duration?a.duration/2:c.fx.speeds._default/2;c.effects.save(b,j);b.show();var e=c.effects.createWrapper(b).css({overflow:"hidden"}),f=d=="show"!=h,l=f?["width","height"]:["height","width"];f=f?[e.width(),e.height()]:[e.height(),e.width()];var i=/([0-9]+)%/.exec(g);if(i)g=parseInt(i[1],
10)/100*f[d=="hide"?0:1];if(d=="show")e.css(h?{height:0,width:g}:{height:g,width:0});h={};i={};h[l[0]]=d=="show"?f[0]:g;i[l[1]]=d=="show"?f[1]:0;e.animate(h,k,a.options.easing).animate(i,k,a.options.easing,function(){d=="hide"&&b.hide();c.effects.restore(b,j);c.effects.removeWrapper(b);a.callback&&a.callback.apply(b[0],arguments);b.dequeue()})})}})(jQuery);
;/*
 * jQuery UI Effects Highlight 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Effects/Highlight
 *
 * Depends:
 *	jquery.effects.core.js
 */
(function(b){b.effects.highlight=function(c){return this.queue(function(){var a=b(this),e=["backgroundImage","backgroundColor","opacity"],d=b.effects.setMode(a,c.options.mode||"show"),f={backgroundColor:a.css("backgroundColor")};if(d=="hide")f.opacity=0;b.effects.save(a,e);a.show().css({backgroundImage:"none",backgroundColor:c.options.color||"#ffff99"}).animate(f,{queue:false,duration:c.duration,easing:c.options.easing,complete:function(){d=="hide"&&a.hide();b.effects.restore(a,e);d=="show"&&!b.support.opacity&&
this.style.removeAttribute("filter");c.callback&&c.callback.apply(this,arguments);a.dequeue()}})})}})(jQuery);
;/*
 * jQuery UI Effects Pulsate 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Effects/Pulsate
 *
 * Depends:
 *	jquery.effects.core.js
 */
(function(d){d.effects.pulsate=function(a){return this.queue(function(){var b=d(this),c=d.effects.setMode(b,a.options.mode||"show");times=(a.options.times||5)*2-1;duration=a.duration?a.duration/2:d.fx.speeds._default/2;isVisible=b.is(":visible");animateTo=0;if(!isVisible){b.css("opacity",0).show();animateTo=1}if(c=="hide"&&isVisible||c=="show"&&!isVisible)times--;for(c=0;c<times;c++){b.animate({opacity:animateTo},duration,a.options.easing);animateTo=(animateTo+1)%2}b.animate({opacity:animateTo},duration,
a.options.easing,function(){animateTo==0&&b.hide();a.callback&&a.callback.apply(this,arguments)});b.queue("fx",function(){b.dequeue()}).dequeue()})}})(jQuery);
;/*
 * jQuery UI Effects Scale 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Effects/Scale
 *
 * Depends:
 *	jquery.effects.core.js
 */
(function(c){c.effects.puff=function(b){return this.queue(function(){var a=c(this),e=c.effects.setMode(a,b.options.mode||"hide"),g=parseInt(b.options.percent,10)||150,h=g/100,i={height:a.height(),width:a.width()};c.extend(b.options,{fade:true,mode:e,percent:e=="hide"?g:100,from:e=="hide"?i:{height:i.height*h,width:i.width*h}});a.effect("scale",b.options,b.duration,b.callback);a.dequeue()})};c.effects.scale=function(b){return this.queue(function(){var a=c(this),e=c.extend(true,{},b.options),g=c.effects.setMode(a,
b.options.mode||"effect"),h=parseInt(b.options.percent,10)||(parseInt(b.options.percent,10)==0?0:g=="hide"?0:100),i=b.options.direction||"both",f=b.options.origin;if(g!="effect"){e.origin=f||["middle","center"];e.restore=true}f={height:a.height(),width:a.width()};a.from=b.options.from||(g=="show"?{height:0,width:0}:f);h={y:i!="horizontal"?h/100:1,x:i!="vertical"?h/100:1};a.to={height:f.height*h.y,width:f.width*h.x};if(b.options.fade){if(g=="show"){a.from.opacity=0;a.to.opacity=1}if(g=="hide"){a.from.opacity=
1;a.to.opacity=0}}e.from=a.from;e.to=a.to;e.mode=g;a.effect("size",e,b.duration,b.callback);a.dequeue()})};c.effects.size=function(b){return this.queue(function(){var a=c(this),e=["position","top","bottom","left","right","width","height","overflow","opacity"],g=["position","top","bottom","left","right","overflow","opacity"],h=["width","height","overflow"],i=["fontSize"],f=["borderTopWidth","borderBottomWidth","paddingTop","paddingBottom"],k=["borderLeftWidth","borderRightWidth","paddingLeft","paddingRight"],
p=c.effects.setMode(a,b.options.mode||"effect"),n=b.options.restore||false,m=b.options.scale||"both",l=b.options.origin,j={height:a.height(),width:a.width()};a.from=b.options.from||j;a.to=b.options.to||j;if(l){l=c.effects.getBaseline(l,j);a.from.top=(j.height-a.from.height)*l.y;a.from.left=(j.width-a.from.width)*l.x;a.to.top=(j.height-a.to.height)*l.y;a.to.left=(j.width-a.to.width)*l.x}var d={from:{y:a.from.height/j.height,x:a.from.width/j.width},to:{y:a.to.height/j.height,x:a.to.width/j.width}};
if(m=="box"||m=="both"){if(d.from.y!=d.to.y){e=e.concat(f);a.from=c.effects.setTransition(a,f,d.from.y,a.from);a.to=c.effects.setTransition(a,f,d.to.y,a.to)}if(d.from.x!=d.to.x){e=e.concat(k);a.from=c.effects.setTransition(a,k,d.from.x,a.from);a.to=c.effects.setTransition(a,k,d.to.x,a.to)}}if(m=="content"||m=="both")if(d.from.y!=d.to.y){e=e.concat(i);a.from=c.effects.setTransition(a,i,d.from.y,a.from);a.to=c.effects.setTransition(a,i,d.to.y,a.to)}c.effects.save(a,n?e:g);a.show();c.effects.createWrapper(a);
a.css("overflow","hidden").css(a.from);if(m=="content"||m=="both"){f=f.concat(["marginTop","marginBottom"]).concat(i);k=k.concat(["marginLeft","marginRight"]);h=e.concat(f).concat(k);a.find("*[width]").each(function(){child=c(this);n&&c.effects.save(child,h);var o={height:child.height(),width:child.width()};child.from={height:o.height*d.from.y,width:o.width*d.from.x};child.to={height:o.height*d.to.y,width:o.width*d.to.x};if(d.from.y!=d.to.y){child.from=c.effects.setTransition(child,f,d.from.y,child.from);
child.to=c.effects.setTransition(child,f,d.to.y,child.to)}if(d.from.x!=d.to.x){child.from=c.effects.setTransition(child,k,d.from.x,child.from);child.to=c.effects.setTransition(child,k,d.to.x,child.to)}child.css(child.from);child.animate(child.to,b.duration,b.options.easing,function(){n&&c.effects.restore(child,h)})})}a.animate(a.to,{queue:false,duration:b.duration,easing:b.options.easing,complete:function(){a.to.opacity===0&&a.css("opacity",a.from.opacity);p=="hide"&&a.hide();c.effects.restore(a,
n?e:g);c.effects.removeWrapper(a);b.callback&&b.callback.apply(this,arguments);a.dequeue()}})})}})(jQuery);
;/*
 * jQuery UI Effects Shake 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Effects/Shake
 *
 * Depends:
 *	jquery.effects.core.js
 */
(function(d){d.effects.shake=function(a){return this.queue(function(){var b=d(this),j=["position","top","bottom","left","right"];d.effects.setMode(b,a.options.mode||"effect");var c=a.options.direction||"left",e=a.options.distance||20,l=a.options.times||3,f=a.duration||a.options.duration||140;d.effects.save(b,j);b.show();d.effects.createWrapper(b);var g=c=="up"||c=="down"?"top":"left",h=c=="up"||c=="left"?"pos":"neg";c={};var i={},k={};c[g]=(h=="pos"?"-=":"+=")+e;i[g]=(h=="pos"?"+=":"-=")+e*2;k[g]=
(h=="pos"?"-=":"+=")+e*2;b.animate(c,f,a.options.easing);for(e=1;e<l;e++)b.animate(i,f,a.options.easing).animate(k,f,a.options.easing);b.animate(i,f,a.options.easing).animate(c,f/2,a.options.easing,function(){d.effects.restore(b,j);d.effects.removeWrapper(b);a.callback&&a.callback.apply(this,arguments)});b.queue("fx",function(){b.dequeue()});b.dequeue()})}})(jQuery);
;/*
 * jQuery UI Effects Slide 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Effects/Slide
 *
 * Depends:
 *	jquery.effects.core.js
 */
(function(c){c.effects.slide=function(d){return this.queue(function(){var a=c(this),h=["position","top","bottom","left","right"],f=c.effects.setMode(a,d.options.mode||"show"),b=d.options.direction||"left";c.effects.save(a,h);a.show();c.effects.createWrapper(a).css({overflow:"hidden"});var g=b=="up"||b=="down"?"top":"left";b=b=="up"||b=="left"?"pos":"neg";var e=d.options.distance||(g=="top"?a.outerHeight({margin:true}):a.outerWidth({margin:true}));if(f=="show")a.css(g,b=="pos"?isNaN(e)?"-"+e:-e:e);
var i={};i[g]=(f=="show"?b=="pos"?"+=":"-=":b=="pos"?"-=":"+=")+e;a.animate(i,{queue:false,duration:d.duration,easing:d.options.easing,complete:function(){f=="hide"&&a.hide();c.effects.restore(a,h);c.effects.removeWrapper(a);d.callback&&d.callback.apply(this,arguments);a.dequeue()}})})}})(jQuery);
;/*
 * jQuery UI Effects Transfer 1.8.13
 *
 * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Effects/Transfer
 *
 * Depends:
 *	jquery.effects.core.js
 */
(function(e){e.effects.transfer=function(a){return this.queue(function(){var b=e(this),c=e(a.options.to),d=c.offset();c={top:d.top,left:d.left,height:c.innerHeight(),width:c.innerWidth()};d=b.offset();var f=e('<div class="ui-effects-transfer"></div>').appendTo(document.body).addClass(a.options.className).css({top:d.top,left:d.left,height:b.innerHeight(),width:b.innerWidth(),position:"absolute"}).animate(c,a.duration,a.options.easing,function(){f.remove();a.callback&&a.callback.apply(b[0],arguments);
b.dequeue()})})}})(jQuery);
;


/*
* jQuery JavaScript Library v1.3.2
* http://jquery.com/
*
* Copyright (c) 2009 John Resig
* Dual licensed under the MIT and GPL licenses.
* http://docs.jquery.com/License
*
* Date: 2009-02-19 17:34:21 -0500 (Thu, 19 Feb 2009)
* Revision: 6246
*/
(function () {
    var l = this, g, y = l.jQuery, p = l.$, o = l.jQuery = l.$ = function (E, F) { return new o.fn.init(E, F) }, D = /^[^<]*(<(.|\s)+>)[^>]*$|^#([\w-]+)$/, f = /^.[^:#\[\.,]*$/; o.fn = o.prototype = { init: function (E, H) { E = E || document; if (E.nodeType) { this[0] = E; this.length = 1; this.context = E; return this } if (typeof E === "string") { var G = D.exec(E); if (G && (G[1] || !H)) { if (G[1]) { E = o.clean([G[1]], H) } else { var I = document.getElementById(G[3]); if (I && I.id != G[3]) { return o().find(E) } var F = o(I || []); F.context = document; F.selector = E; return F } } else { return o(H).find(E) } } else { if (o.isFunction(E)) { return o(document).ready(E) } } if (E.selector && E.context) { this.selector = E.selector; this.context = E.context } return this.setArray(o.isArray(E) ? E : o.makeArray(E)) }, selector: "", jquery: "1.3.2", size: function () { return this.length }, get: function (E) { return E === g ? Array.prototype.slice.call(this) : this[E] }, pushStack: function (F, H, E) { var G = o(F); G.prevObject = this; G.context = this.context; if (H === "find") { G.selector = this.selector + (this.selector ? " " : "") + E } else { if (H) { G.selector = this.selector + "." + H + "(" + E + ")" } } return G }, setArray: function (E) { this.length = 0; Array.prototype.push.apply(this, E); return this }, each: function (F, E) { return o.each(this, F, E) }, index: function (E) { return o.inArray(E && E.jquery ? E[0] : E, this) }, attr: function (F, H, G) { var E = F; if (typeof F === "string") { if (H === g) { return this[0] && o[G || "attr"](this[0], F) } else { E = {}; E[F] = H } } return this.each(function (I) { for (F in E) { o.attr(G ? this.style : this, F, o.prop(this, E[F], G, I, F)) } }) }, css: function (E, F) { if ((E == "width" || E == "height") && parseFloat(F) < 0) { F = g } return this.attr(E, F, "curCSS") }, text: function (F) { if (typeof F !== "object" && F != null) { return this.empty().append((this[0] && this[0].ownerDocument || document).createTextNode(F)) } var E = ""; o.each(F || this, function () { o.each(this.childNodes, function () { if (this.nodeType != 8) { E += this.nodeType != 1 ? this.nodeValue : o.fn.text([this]) } }) }); return E }, wrapAll: function (E) { if (this[0]) { var F = o(E, this[0].ownerDocument).clone(); if (this[0].parentNode) { F.insertBefore(this[0]) } F.map(function () { var G = this; while (G.firstChild) { G = G.firstChild } return G }).append(this) } return this }, wrapInner: function (E) { return this.each(function () { o(this).contents().wrapAll(E) }) }, wrap: function (E) { return this.each(function () { o(this).wrapAll(E) }) }, append: function () { return this.domManip(arguments, true, function (E) { if (this.nodeType == 1) { this.appendChild(E) } }) }, prepend: function () { return this.domManip(arguments, true, function (E) { if (this.nodeType == 1) { this.insertBefore(E, this.firstChild) } }) }, before: function () { return this.domManip(arguments, false, function (E) { this.parentNode.insertBefore(E, this) }) }, after: function () { return this.domManip(arguments, false, function (E) { this.parentNode.insertBefore(E, this.nextSibling) }) }, end: function () { return this.prevObject || o([]) }, push: [].push, sort: [].sort, splice: [].splice, find: function (E) { if (this.length === 1) { var F = this.pushStack([], "find", E); F.length = 0; o.find(E, this[0], F); return F } else { return this.pushStack(o.unique(o.map(this, function (G) { return o.find(E, G) })), "find", E) } }, clone: function (G) { var E = this.map(function () { if (!o.support.noCloneEvent && !o.isXMLDoc(this)) { var I = this.outerHTML; if (!I) { var J = this.ownerDocument.createElement("div"); J.appendChild(this.cloneNode(true)); I = J.innerHTML } return o.clean([I.replace(/ jQuery\d+="(?:\d+|null)"/g, "").replace(/^\s*/, "")])[0] } else { return this.cloneNode(true) } }); if (G === true) { var H = this.find("*").andSelf(), F = 0; E.find("*").andSelf().each(function () { if (this.nodeName !== H[F].nodeName) { return } var I = o.data(H[F], "events"); for (var K in I) { for (var J in I[K]) { o.event.add(this, K, I[K][J], I[K][J].data) } } F++ }) } return E }, filter: function (E) { return this.pushStack(o.isFunction(E) && o.grep(this, function (G, F) { return E.call(G, F) }) || o.multiFilter(E, o.grep(this, function (F) { return F.nodeType === 1 })), "filter", E) }, closest: function (E) { var G = o.expr.match.POS.test(E) ? o(E) : null, F = 0; return this.map(function () { var H = this; while (H && H.ownerDocument) { if (G ? G.index(H) > -1 : o(H).is(E)) { o.data(H, "closest", F); return H } H = H.parentNode; F++ } }) }, not: function (E) { if (typeof E === "string") { if (f.test(E)) { return this.pushStack(o.multiFilter(E, this, true), "not", E) } else { E = o.multiFilter(E, this) } } var F = E.length && E[E.length - 1] !== g && !E.nodeType; return this.filter(function () { return F ? o.inArray(this, E) < 0 : this != E }) }, add: function (E) { return this.pushStack(o.unique(o.merge(this.get(), typeof E === "string" ? o(E) : o.makeArray(E)))) }, is: function (E) { return !!E && o.multiFilter(E, this).length > 0 }, hasClass: function (E) { return !!E && this.is("." + E) }, val: function (K) { if (K === g) { var E = this[0]; if (E) { if (o.nodeName(E, "option")) { return (E.attributes.value || {}).specified ? E.value : E.text } if (o.nodeName(E, "select")) { var I = E.selectedIndex, L = [], M = E.options, H = E.type == "select-one"; if (I < 0) { return null } for (var F = H ? I : 0, J = H ? I + 1 : M.length; F < J; F++) { var G = M[F]; if (G.selected) { K = o(G).val(); if (H) { return K } L.push(K) } } return L } return (E.value || "").replace(/\r/g, "") } return g } if (typeof K === "number") { K += "" } return this.each(function () { if (this.nodeType != 1) { return } if (o.isArray(K) && /radio|checkbox/.test(this.type)) { this.checked = (o.inArray(this.value, K) >= 0 || o.inArray(this.name, K) >= 0) } else { if (o.nodeName(this, "select")) { var N = o.makeArray(K); o("option", this).each(function () { this.selected = (o.inArray(this.value, N) >= 0 || o.inArray(this.text, N) >= 0) }); if (!N.length) { this.selectedIndex = -1 } } else { this.value = K } } }) }, html: function (E) { return E === g ? (this[0] ? this[0].innerHTML.replace(/ jQuery\d+="(?:\d+|null)"/g, "") : null) : this.empty().append(E) }, replaceWith: function (E) { return this.after(E).remove() }, eq: function (E) { return this.slice(E, +E + 1) }, slice: function () { return this.pushStack(Array.prototype.slice.apply(this, arguments), "slice", Array.prototype.slice.call(arguments).join(",")) }, map: function (E) { return this.pushStack(o.map(this, function (G, F) { return E.call(G, F, G) })) }, andSelf: function () { return this.add(this.prevObject) }, domManip: function (J, M, L) { if (this[0]) { var I = (this[0].ownerDocument || this[0]).createDocumentFragment(), F = o.clean(J, (this[0].ownerDocument || this[0]), I), H = I.firstChild; if (H) { for (var G = 0, E = this.length; G < E; G++) { L.call(K(this[G], H), this.length > 1 || G > 0 ? I.cloneNode(true) : I) } } if (F) { o.each(F, z) } } return this; function K(N, O) { return M && o.nodeName(N, "table") && o.nodeName(O, "tr") ? (N.getElementsByTagName("tbody")[0] || N.appendChild(N.ownerDocument.createElement("tbody"))) : N } } }; o.fn.init.prototype = o.fn; function z(E, F) { if (F.src) { o.ajax({ url: F.src, async: false, dataType: "script" }) } else { o.globalEval(F.text || F.textContent || F.innerHTML || "") } if (F.parentNode) { F.parentNode.removeChild(F) } } function e() { return +new Date } o.extend = o.fn.extend = function () { var J = arguments[0] || {}, H = 1, I = arguments.length, E = false, G; if (typeof J === "boolean") { E = J; J = arguments[1] || {}; H = 2 } if (typeof J !== "object" && !o.isFunction(J)) { J = {} } if (I == H) { J = this; --H } for (; H < I; H++) { if ((G = arguments[H]) != null) { for (var F in G) { var K = J[F], L = G[F]; if (J === L) { continue } if (E && L && typeof L === "object" && !L.nodeType) { J[F] = o.extend(E, K || (L.length != null ? [] : {}), L) } else { if (L !== g) { J[F] = L } } } } } return J }; var b = /z-?index|font-?weight|opacity|zoom|line-?height/i, q = document.defaultView || {}, s = Object.prototype.toString; o.extend({ noConflict: function (E) { l.$ = p; if (E) { l.jQuery = y } return o }, isFunction: function (E) { return s.call(E) === "[object Function]" }, isArray: function (E) { return s.call(E) === "[object Array]" }, isXMLDoc: function (E) { return E.nodeType === 9 && E.documentElement.nodeName !== "HTML" || !!E.ownerDocument && o.isXMLDoc(E.ownerDocument) }, globalEval: function (G) { if (G && /\S/.test(G)) { var F = document.getElementsByTagName("head")[0] || document.documentElement, E = document.createElement("script"); E.type = "text/javascript"; if (o.support.scriptEval) { E.appendChild(document.createTextNode(G)) } else { E.text = G } F.insertBefore(E, F.firstChild); F.removeChild(E) } }, nodeName: function (F, E) { return F.nodeName && F.nodeName.toUpperCase() == E.toUpperCase() }, each: function (G, K, F) { var E, H = 0, I = G.length; if (F) { if (I === g) { for (E in G) { if (K.apply(G[E], F) === false) { break } } } else { for (; H < I; ) { if (K.apply(G[H++], F) === false) { break } } } } else { if (I === g) { for (E in G) { if (K.call(G[E], E, G[E]) === false) { break } } } else { for (var J = G[0]; H < I && K.call(J, H, J) !== false; J = G[++H]) { } } } return G }, prop: function (H, I, G, F, E) { if (o.isFunction(I)) { I = I.call(H, F) } return typeof I === "number" && G == "curCSS" && !b.test(E) ? I + "px" : I }, className: { add: function (E, F) { o.each((F || "").split(/\s+/), function (G, H) { if (E.nodeType == 1 && !o.className.has(E.className, H)) { E.className += (E.className ? " " : "") + H } }) }, remove: function (E, F) { if (E.nodeType == 1) { E.className = F !== g ? o.grep(E.className.split(/\s+/), function (G) { return !o.className.has(F, G) }).join(" ") : "" } }, has: function (F, E) { return F && o.inArray(E, (F.className || F).toString().split(/\s+/)) > -1 } }, swap: function (H, G, I) { var E = {}; for (var F in G) { E[F] = H.style[F]; H.style[F] = G[F] } I.call(H); for (var F in G) { H.style[F] = E[F] } }, css: function (H, F, J, E) { if (F == "width" || F == "height") { var L, G = { position: "absolute", visibility: "hidden", display: "block" }, K = F == "width" ? ["Left", "Right"] : ["Top", "Bottom"]; function I() { L = F == "width" ? H.offsetWidth : H.offsetHeight; if (E === "border") { return } o.each(K, function () { if (!E) { L -= parseFloat(o.curCSS(H, "padding" + this, true)) || 0 } if (E === "margin") { L += parseFloat(o.curCSS(H, "margin" + this, true)) || 0 } else { L -= parseFloat(o.curCSS(H, "border" + this + "Width", true)) || 0 } }) } if (H.offsetWidth !== 0) { I() } else { o.swap(H, G, I) } return Math.max(0, Math.round(L)) } return o.curCSS(H, F, J) }, curCSS: function (I, F, G) { var L, E = I.style; if (F == "opacity" && !o.support.opacity) { L = o.attr(E, "opacity"); return L == "" ? "1" : L } if (F.match(/float/i)) { F = w } if (!G && E && E[F]) { L = E[F] } else { if (q.getComputedStyle) { if (F.match(/float/i)) { F = "float" } F = F.replace(/([A-Z])/g, "-$1").toLowerCase(); var M = q.getComputedStyle(I, null); if (M) { L = M.getPropertyValue(F) } if (F == "opacity" && L == "") { L = "1" } } else { if (I.currentStyle) { var J = F.replace(/\-(\w)/g, function (N, O) { return O.toUpperCase() }); L = I.currentStyle[F] || I.currentStyle[J]; if (!/^\d+(px)?$/i.test(L) && /^\d/.test(L)) { var H = E.left, K = I.runtimeStyle.left; I.runtimeStyle.left = I.currentStyle.left; E.left = L || 0; L = E.pixelLeft + "px"; E.left = H; I.runtimeStyle.left = K } } } } return L }, clean: function (F, K, I) { K = K || document; if (typeof K.createElement === "undefined") { K = K.ownerDocument || K[0] && K[0].ownerDocument || document } if (!I && F.length === 1 && typeof F[0] === "string") { var H = /^<(\w+)\s*\/?>$/.exec(F[0]); if (H) { return [K.createElement(H[1])] } } var G = [], E = [], L = K.createElement("div"); o.each(F, function (P, S) { if (typeof S === "number") { S += "" } if (!S) { return } if (typeof S === "string") { S = S.replace(/(<(\w+)[^>]*?)\/>/g, function (U, V, T) { return T.match(/^(abbr|br|col|img|input|link|meta|param|hr|area|embed)$/i) ? U : V + "></" + T + ">" }); var O = S.replace(/^\s+/, "").substring(0, 10).toLowerCase(); var Q = !O.indexOf("<opt") && [1, "<select multiple='multiple'>", "</select>"] || !O.indexOf("<leg") && [1, "<fieldset>", "</fieldset>"] || O.match(/^<(thead|tbody|tfoot|colg|cap)/) && [1, "<table>", "</table>"] || !O.indexOf("<tr") && [2, "<table><tbody>", "</tbody></table>"] || (!O.indexOf("<td") || !O.indexOf("<th")) && [3, "<table><tbody><tr>", "</tr></tbody></table>"] || !O.indexOf("<col") && [2, "<table><tbody></tbody><colgroup>", "</colgroup></table>"] || !o.support.htmlSerialize && [1, "div<div>", "</div>"] || [0, "", ""]; L.innerHTML = Q[1] + S + Q[2]; while (Q[0]--) { L = L.lastChild } if (!o.support.tbody) { var R = /<tbody/i.test(S), N = !O.indexOf("<table") && !R ? L.firstChild && L.firstChild.childNodes : Q[1] == "<table>" && !R ? L.childNodes : []; for (var M = N.length - 1; M >= 0; --M) { if (o.nodeName(N[M], "tbody") && !N[M].childNodes.length) { N[M].parentNode.removeChild(N[M]) } } } if (!o.support.leadingWhitespace && /^\s/.test(S)) { L.insertBefore(K.createTextNode(S.match(/^\s*/)[0]), L.firstChild) } S = o.makeArray(L.childNodes) } if (S.nodeType) { G.push(S) } else { G = o.merge(G, S) } }); if (I) { for (var J = 0; G[J]; J++) { if (o.nodeName(G[J], "script") && (!G[J].type || G[J].type.toLowerCase() === "text/javascript")) { E.push(G[J].parentNode ? G[J].parentNode.removeChild(G[J]) : G[J]) } else { if (G[J].nodeType === 1) { G.splice.apply(G, [J + 1, 0].concat(o.makeArray(G[J].getElementsByTagName("script")))) } I.appendChild(G[J]) } } return E } return G }, attr: function (J, G, K) { if (!J || J.nodeType == 3 || J.nodeType == 8) { return g } var H = !o.isXMLDoc(J), L = K !== g; G = H && o.props[G] || G; if (J.tagName) { var F = /href|src|style/.test(G); if (G == "selected" && J.parentNode) { J.parentNode.selectedIndex } if (G in J && H && !F) { if (L) { if (G == "type" && o.nodeName(J, "input") && J.parentNode) { throw "type property can't be changed" } J[G] = K } if (o.nodeName(J, "form") && J.getAttributeNode(G)) { return J.getAttributeNode(G).nodeValue } if (G == "tabIndex") { var I = J.getAttributeNode("tabIndex"); return I && I.specified ? I.value : J.nodeName.match(/(button|input|object|select|textarea)/i) ? 0 : J.nodeName.match(/^(a|area)$/i) && J.href ? 0 : g } return J[G] } if (!o.support.style && H && G == "style") { return o.attr(J.style, "cssText", K) } if (L) { J.setAttribute(G, "" + K) } var E = !o.support.hrefNormalized && H && F ? J.getAttribute(G, 2) : J.getAttribute(G); return E === null ? g : E } if (!o.support.opacity && G == "opacity") { if (L) { J.zoom = 1; J.filter = (J.filter || "").replace(/alpha\([^)]*\)/, "") + (parseInt(K) + "" == "NaN" ? "" : "alpha(opacity=" + K * 100 + ")") } return J.filter && J.filter.indexOf("opacity=") >= 0 ? (parseFloat(J.filter.match(/opacity=([^)]*)/)[1]) / 100) + "" : "" } G = G.replace(/-([a-z])/ig, function (M, N) { return N.toUpperCase() }); if (L) { J[G] = K } return J[G] }, trim: function (E) { return (E || "").replace(/^\s+|\s+$/g, "") }, makeArray: function (G) { var E = []; if (G != null) { var F = G.length; if (F == null || typeof G === "string" || o.isFunction(G) || G.setInterval) { E[0] = G } else { while (F) { E[--F] = G[F] } } } return E }, inArray: function (G, H) { for (var E = 0, F = H.length; E < F; E++) { if (H[E] === G) { return E } } return -1 }, merge: function (H, E) { var F = 0, G, I = H.length; if (!o.support.getAll) { while ((G = E[F++]) != null) { if (G.nodeType != 8) { H[I++] = G } } } else { while ((G = E[F++]) != null) { H[I++] = G } } return H }, unique: function (K) { var F = [], E = {}; try { for (var G = 0, H = K.length; G < H; G++) { var J = o.data(K[G]); if (!E[J]) { E[J] = true; F.push(K[G]) } } } catch (I) { F = K } return F }, grep: function (F, J, E) { var G = []; for (var H = 0, I = F.length; H < I; H++) { if (!E != !J(F[H], H)) { G.push(F[H]) } } return G }, map: function (E, J) { var F = []; for (var G = 0, H = E.length; G < H; G++) { var I = J(E[G], G); if (I != null) { F[F.length] = I } } return F.concat.apply([], F) } }); var C = navigator.userAgent.toLowerCase(); o.browser = { version: (C.match(/.+(?:rv|it|ra|ie)[\/: ]([\d.]+)/) || [0, "0"])[1], safari: /webkit/.test(C), opera: /opera/.test(C), msie: /msie/.test(C) && !/opera/.test(C), mozilla: /mozilla/.test(C) && !/(compatible|webkit)/.test(C) }; o.each({ parent: function (E) { return E.parentNode }, parents: function (E) { return o.dir(E, "parentNode") }, next: function (E) { return o.nth(E, 2, "nextSibling") }, prev: function (E) { return o.nth(E, 2, "previousSibling") }, nextAll: function (E) { return o.dir(E, "nextSibling") }, prevAll: function (E) { return o.dir(E, "previousSibling") }, siblings: function (E) { return o.sibling(E.parentNode.firstChild, E) }, children: function (E) { return o.sibling(E.firstChild) }, contents: function (E) { return o.nodeName(E, "iframe") ? E.contentDocument || E.contentWindow.document : o.makeArray(E.childNodes) } }, function (E, F) { o.fn[E] = function (G) { var H = o.map(this, F); if (G && typeof G == "string") { H = o.multiFilter(G, H) } return this.pushStack(o.unique(H), E, G) } }); o.each({ appendTo: "append", prependTo: "prepend", insertBefore: "before", insertAfter: "after", replaceAll: "replaceWith" }, function (E, F) { o.fn[E] = function (G) { var J = [], L = o(G); for (var K = 0, H = L.length; K < H; K++) { var I = (K > 0 ? this.clone(true) : this).get(); o.fn[F].apply(o(L[K]), I); J = J.concat(I) } return this.pushStack(J, E, G) } }); o.each({ removeAttr: function (E) { o.attr(this, E, ""); if (this.nodeType == 1) { this.removeAttribute(E) } }, addClass: function (E) { o.className.add(this, E) }, removeClass: function (E) { o.className.remove(this, E) }, toggleClass: function (F, E) { if (typeof E !== "boolean") { E = !o.className.has(this, F) } o.className[E ? "add" : "remove"](this, F) }, remove: function (E) { if (!E || o.filter(E, [this]).length) { o("*", this).add([this]).each(function () { o.event.remove(this); o.removeData(this) }); if (this.parentNode) { this.parentNode.removeChild(this) } } }, empty: function () { o(this).children().remove(); while (this.firstChild) { this.removeChild(this.firstChild) } } }, function (E, F) { o.fn[E] = function () { return this.each(F, arguments) } }); function j(E, F) { return E[0] && parseInt(o.curCSS(E[0], F, true), 10) || 0 } var h = "jQuery" + e(), v = 0, A = {}; o.extend({ cache: {}, data: function (F, E, G) { F = F == l ? A : F; var H = F[h]; if (!H) { H = F[h] = ++v } if (E && !o.cache[H]) { o.cache[H] = {} } if (G !== g) { o.cache[H][E] = G } return E ? o.cache[H][E] : H }, removeData: function (F, E) { F = F == l ? A : F; var H = F[h]; if (E) { if (o.cache[H]) { delete o.cache[H][E]; E = ""; for (E in o.cache[H]) { break } if (!E) { o.removeData(F) } } } else { try { delete F[h] } catch (G) { if (F.removeAttribute) { F.removeAttribute(h) } } delete o.cache[H] } }, queue: function (F, E, H) { if (F) { E = (E || "fx") + "queue"; var G = o.data(F, E); if (!G || o.isArray(H)) { G = o.data(F, E, o.makeArray(H)) } else { if (H) { G.push(H) } } } return G }, dequeue: function (H, G) { var E = o.queue(H, G), F = E.shift(); if (!G || G === "fx") { F = E[0] } if (F !== g) { F.call(H) } } }); o.fn.extend({ data: function (E, G) { var H = E.split("."); H[1] = H[1] ? "." + H[1] : ""; if (G === g) { var F = this.triggerHandler("getData" + H[1] + "!", [H[0]]); if (F === g && this.length) { F = o.data(this[0], E) } return F === g && H[1] ? this.data(H[0]) : F } else { return this.trigger("setData" + H[1] + "!", [H[0], G]).each(function () { o.data(this, E, G) }) } }, removeData: function (E) { return this.each(function () { o.removeData(this, E) }) }, queue: function (E, F) { if (typeof E !== "string") { F = E; E = "fx" } if (F === g) { return o.queue(this[0], E) } return this.each(function () { var G = o.queue(this, E, F); if (E == "fx" && G.length == 1) { G[0].call(this) } }) }, dequeue: function (E) { return this.each(function () { o.dequeue(this, E) }) } });
    /*
    * Sizzle CSS Selector Engine - v0.9.3
    *  Copyright 2009, The Dojo Foundation
    *  Released under the MIT, BSD, and GPL Licenses.
    *  More information: http://sizzlejs.com/
    */
    (function () { var R = /((?:\((?:\([^()]+\)|[^()]+)+\)|\[(?:\[[^[\]]*\]|['"][^'"]*['"]|[^[\]'"]+)+\]|\\.|[^ >+~,(\[\\]+)+|[>+~])(\s*,\s*)?/g, L = 0, H = Object.prototype.toString; var F = function (Y, U, ab, ac) { ab = ab || []; U = U || document; if (U.nodeType !== 1 && U.nodeType !== 9) { return [] } if (!Y || typeof Y !== "string") { return ab } var Z = [], W, af, ai, T, ad, V, X = true; R.lastIndex = 0; while ((W = R.exec(Y)) !== null) { Z.push(W[1]); if (W[2]) { V = RegExp.rightContext; break } } if (Z.length > 1 && M.exec(Y)) { if (Z.length === 2 && I.relative[Z[0]]) { af = J(Z[0] + Z[1], U) } else { af = I.relative[Z[0]] ? [U] : F(Z.shift(), U); while (Z.length) { Y = Z.shift(); if (I.relative[Y]) { Y += Z.shift() } af = J(Y, af) } } } else { var ae = ac ? { expr: Z.pop(), set: E(ac)} : F.find(Z.pop(), Z.length === 1 && U.parentNode ? U.parentNode : U, Q(U)); af = F.filter(ae.expr, ae.set); if (Z.length > 0) { ai = E(af) } else { X = false } while (Z.length) { var ah = Z.pop(), ag = ah; if (!I.relative[ah]) { ah = "" } else { ag = Z.pop() } if (ag == null) { ag = U } I.relative[ah](ai, ag, Q(U)) } } if (!ai) { ai = af } if (!ai) { throw "Syntax error, unrecognized expression: " + (ah || Y) } if (H.call(ai) === "[object Array]") { if (!X) { ab.push.apply(ab, ai) } else { if (U.nodeType === 1) { for (var aa = 0; ai[aa] != null; aa++) { if (ai[aa] && (ai[aa] === true || ai[aa].nodeType === 1 && K(U, ai[aa]))) { ab.push(af[aa]) } } } else { for (var aa = 0; ai[aa] != null; aa++) { if (ai[aa] && ai[aa].nodeType === 1) { ab.push(af[aa]) } } } } } else { E(ai, ab) } if (V) { F(V, U, ab, ac); if (G) { hasDuplicate = false; ab.sort(G); if (hasDuplicate) { for (var aa = 1; aa < ab.length; aa++) { if (ab[aa] === ab[aa - 1]) { ab.splice(aa--, 1) } } } } } return ab }; F.matches = function (T, U) { return F(T, null, null, U) }; F.find = function (aa, T, ab) { var Z, X; if (!aa) { return [] } for (var W = 0, V = I.order.length; W < V; W++) { var Y = I.order[W], X; if ((X = I.match[Y].exec(aa))) { var U = RegExp.leftContext; if (U.substr(U.length - 1) !== "\\") { X[1] = (X[1] || "").replace(/\\/g, ""); Z = I.find[Y](X, T, ab); if (Z != null) { aa = aa.replace(I.match[Y], ""); break } } } } if (!Z) { Z = T.getElementsByTagName("*") } return { set: Z, expr: aa} }; F.filter = function (ad, ac, ag, W) { var V = ad, ai = [], aa = ac, Y, T, Z = ac && ac[0] && Q(ac[0]); while (ad && ac.length) { for (var ab in I.filter) { if ((Y = I.match[ab].exec(ad)) != null) { var U = I.filter[ab], ah, af; T = false; if (aa == ai) { ai = [] } if (I.preFilter[ab]) { Y = I.preFilter[ab](Y, aa, ag, ai, W, Z); if (!Y) { T = ah = true } else { if (Y === true) { continue } } } if (Y) { for (var X = 0; (af = aa[X]) != null; X++) { if (af) { ah = U(af, Y, X, aa); var ae = W ^ !!ah; if (ag && ah != null) { if (ae) { T = true } else { aa[X] = false } } else { if (ae) { ai.push(af); T = true } } } } } if (ah !== g) { if (!ag) { aa = ai } ad = ad.replace(I.match[ab], ""); if (!T) { return [] } break } } } if (ad == V) { if (T == null) { throw "Syntax error, unrecognized expression: " + ad } else { break } } V = ad } return aa }; var I = F.selectors = { order: ["ID", "NAME", "TAG"], match: { ID: /#((?:[\w\u00c0-\uFFFF_-]|\\.)+)/, CLASS: /\.((?:[\w\u00c0-\uFFFF_-]|\\.)+)/, NAME: /\[name=['"]*((?:[\w\u00c0-\uFFFF_-]|\\.)+)['"]*\]/, ATTR: /\[\s*((?:[\w\u00c0-\uFFFF_-]|\\.)+)\s*(?:(\S?=)\s*(['"]*)(.*?)\3|)\s*\]/, TAG: /^((?:[\w\u00c0-\uFFFF\*_-]|\\.)+)/, CHILD: /:(only|nth|last|first)-child(?:\((even|odd|[\dn+-]*)\))?/, POS: /:(nth|eq|gt|lt|first|last|even|odd)(?:\((\d*)\))?(?=[^-]|$)/, PSEUDO: /:((?:[\w\u00c0-\uFFFF_-]|\\.)+)(?:\((['"]*)((?:\([^\)]+\)|[^\2\(\)]*)+)\2\))?/ }, attrMap: { "class": "className", "for": "htmlFor" }, attrHandle: { href: function (T) { return T.getAttribute("href") } }, relative: { "+": function (aa, T, Z) { var X = typeof T === "string", ab = X && !/\W/.test(T), Y = X && !ab; if (ab && !Z) { T = T.toUpperCase() } for (var W = 0, V = aa.length, U; W < V; W++) { if ((U = aa[W])) { while ((U = U.previousSibling) && U.nodeType !== 1) { } aa[W] = Y || U && U.nodeName === T ? U || false : U === T } } if (Y) { F.filter(T, aa, true) } }, ">": function (Z, U, aa) { var X = typeof U === "string"; if (X && !/\W/.test(U)) { U = aa ? U : U.toUpperCase(); for (var V = 0, T = Z.length; V < T; V++) { var Y = Z[V]; if (Y) { var W = Y.parentNode; Z[V] = W.nodeName === U ? W : false } } } else { for (var V = 0, T = Z.length; V < T; V++) { var Y = Z[V]; if (Y) { Z[V] = X ? Y.parentNode : Y.parentNode === U } } if (X) { F.filter(U, Z, true) } } }, "": function (W, U, Y) { var V = L++, T = S; if (!U.match(/\W/)) { var X = U = Y ? U : U.toUpperCase(); T = P } T("parentNode", U, V, W, X, Y) }, "~": function (W, U, Y) { var V = L++, T = S; if (typeof U === "string" && !U.match(/\W/)) { var X = U = Y ? U : U.toUpperCase(); T = P } T("previousSibling", U, V, W, X, Y) } }, find: { ID: function (U, V, W) { if (typeof V.getElementById !== "undefined" && !W) { var T = V.getElementById(U[1]); return T ? [T] : [] } }, NAME: function (V, Y, Z) { if (typeof Y.getElementsByName !== "undefined") { var U = [], X = Y.getElementsByName(V[1]); for (var W = 0, T = X.length; W < T; W++) { if (X[W].getAttribute("name") === V[1]) { U.push(X[W]) } } return U.length === 0 ? null : U } }, TAG: function (T, U) { return U.getElementsByTagName(T[1]) } }, preFilter: { CLASS: function (W, U, V, T, Z, aa) { W = " " + W[1].replace(/\\/g, "") + " "; if (aa) { return W } for (var X = 0, Y; (Y = U[X]) != null; X++) { if (Y) { if (Z ^ (Y.className && (" " + Y.className + " ").indexOf(W) >= 0)) { if (!V) { T.push(Y) } } else { if (V) { U[X] = false } } } } return false }, ID: function (T) { return T[1].replace(/\\/g, "") }, TAG: function (U, T) { for (var V = 0; T[V] === false; V++) { } return T[V] && Q(T[V]) ? U[1] : U[1].toUpperCase() }, CHILD: function (T) { if (T[1] == "nth") { var U = /(-?)(\d*)n((?:\+|-)?\d*)/.exec(T[2] == "even" && "2n" || T[2] == "odd" && "2n+1" || !/\D/.test(T[2]) && "0n+" + T[2] || T[2]); T[2] = (U[1] + (U[2] || 1)) - 0; T[3] = U[3] - 0 } T[0] = L++; return T }, ATTR: function (X, U, V, T, Y, Z) { var W = X[1].replace(/\\/g, ""); if (!Z && I.attrMap[W]) { X[1] = I.attrMap[W] } if (X[2] === "~=") { X[4] = " " + X[4] + " " } return X }, PSEUDO: function (X, U, V, T, Y) { if (X[1] === "not") { if (X[3].match(R).length > 1 || /^\w/.test(X[3])) { X[3] = F(X[3], null, null, U) } else { var W = F.filter(X[3], U, V, true ^ Y); if (!V) { T.push.apply(T, W) } return false } } else { if (I.match.POS.test(X[0]) || I.match.CHILD.test(X[0])) { return true } } return X }, POS: function (T) { T.unshift(true); return T } }, filters: { enabled: function (T) { return T.disabled === false && T.type !== "hidden" }, disabled: function (T) { return T.disabled === true }, checked: function (T) { return T.checked === true }, selected: function (T) { T.parentNode.selectedIndex; return T.selected === true }, parent: function (T) { return !!T.firstChild }, empty: function (T) { return !T.firstChild }, has: function (V, U, T) { return !!F(T[3], V).length }, header: function (T) { return /h\d/i.test(T.nodeName) }, text: function (T) { return "text" === T.type }, radio: function (T) { return "radio" === T.type }, checkbox: function (T) { return "checkbox" === T.type }, file: function (T) { return "file" === T.type }, password: function (T) { return "password" === T.type }, submit: function (T) { return "submit" === T.type }, image: function (T) { return "image" === T.type }, reset: function (T) { return "reset" === T.type }, button: function (T) { return "button" === T.type || T.nodeName.toUpperCase() === "BUTTON" }, input: function (T) { return /input|select|textarea|button/i.test(T.nodeName) } }, setFilters: { first: function (U, T) { return T === 0 }, last: function (V, U, T, W) { return U === W.length - 1 }, even: function (U, T) { return T % 2 === 0 }, odd: function (U, T) { return T % 2 === 1 }, lt: function (V, U, T) { return U < T[3] - 0 }, gt: function (V, U, T) { return U > T[3] - 0 }, nth: function (V, U, T) { return T[3] - 0 == U }, eq: function (V, U, T) { return T[3] - 0 == U } }, filter: { PSEUDO: function (Z, V, W, aa) { var U = V[1], X = I.filters[U]; if (X) { return X(Z, W, V, aa) } else { if (U === "contains") { return (Z.textContent || Z.innerText || "").indexOf(V[3]) >= 0 } else { if (U === "not") { var Y = V[3]; for (var W = 0, T = Y.length; W < T; W++) { if (Y[W] === Z) { return false } } return true } } } }, CHILD: function (T, W) { var Z = W[1], U = T; switch (Z) { case "only": case "first": while (U = U.previousSibling) { if (U.nodeType === 1) { return false } } if (Z == "first") { return true } U = T; case "last": while (U = U.nextSibling) { if (U.nodeType === 1) { return false } } return true; case "nth": var V = W[2], ac = W[3]; if (V == 1 && ac == 0) { return true } var Y = W[0], ab = T.parentNode; if (ab && (ab.sizcache !== Y || !T.nodeIndex)) { var X = 0; for (U = ab.firstChild; U; U = U.nextSibling) { if (U.nodeType === 1) { U.nodeIndex = ++X } } ab.sizcache = Y } var aa = T.nodeIndex - ac; if (V == 0) { return aa == 0 } else { return (aa % V == 0 && aa / V >= 0) } } }, ID: function (U, T) { return U.nodeType === 1 && U.getAttribute("id") === T }, TAG: function (U, T) { return (T === "*" && U.nodeType === 1) || U.nodeName === T }, CLASS: function (U, T) { return (" " + (U.className || U.getAttribute("class")) + " ").indexOf(T) > -1 }, ATTR: function (Y, W) { var V = W[1], T = I.attrHandle[V] ? I.attrHandle[V](Y) : Y[V] != null ? Y[V] : Y.getAttribute(V), Z = T + "", X = W[2], U = W[4]; return T == null ? X === "!=" : X === "=" ? Z === U : X === "*=" ? Z.indexOf(U) >= 0 : X === "~=" ? (" " + Z + " ").indexOf(U) >= 0 : !U ? Z && T !== false : X === "!=" ? Z != U : X === "^=" ? Z.indexOf(U) === 0 : X === "$=" ? Z.substr(Z.length - U.length) === U : X === "|=" ? Z === U || Z.substr(0, U.length + 1) === U + "-" : false }, POS: function (X, U, V, Y) { var T = U[2], W = I.setFilters[T]; if (W) { return W(X, V, U, Y) } } } }; var M = I.match.POS; for (var O in I.match) { I.match[O] = RegExp(I.match[O].source + /(?![^\[]*\])(?![^\(]*\))/.source) } var E = function (U, T) { U = Array.prototype.slice.call(U); if (T) { T.push.apply(T, U); return T } return U }; try { Array.prototype.slice.call(document.documentElement.childNodes) } catch (N) { E = function (X, W) { var U = W || []; if (H.call(X) === "[object Array]") { Array.prototype.push.apply(U, X) } else { if (typeof X.length === "number") { for (var V = 0, T = X.length; V < T; V++) { U.push(X[V]) } } else { for (var V = 0; X[V]; V++) { U.push(X[V]) } } } return U } } var G; if (document.documentElement.compareDocumentPosition) { G = function (U, T) { var V = U.compareDocumentPosition(T) & 4 ? -1 : U === T ? 0 : 1; if (V === 0) { hasDuplicate = true } return V } } else { if ("sourceIndex" in document.documentElement) { G = function (U, T) { var V = U.sourceIndex - T.sourceIndex; if (V === 0) { hasDuplicate = true } return V } } else { if (document.createRange) { G = function (W, U) { var V = W.ownerDocument.createRange(), T = U.ownerDocument.createRange(); V.selectNode(W); V.collapse(true); T.selectNode(U); T.collapse(true); var X = V.compareBoundaryPoints(Range.START_TO_END, T); if (X === 0) { hasDuplicate = true } return X } } } } (function () { var U = document.createElement("form"), V = "script" + (new Date).getTime(); U.innerHTML = "<input name='" + V + "'/>"; var T = document.documentElement; T.insertBefore(U, T.firstChild); if (!!document.getElementById(V)) { I.find.ID = function (X, Y, Z) { if (typeof Y.getElementById !== "undefined" && !Z) { var W = Y.getElementById(X[1]); return W ? W.id === X[1] || typeof W.getAttributeNode !== "undefined" && W.getAttributeNode("id").nodeValue === X[1] ? [W] : g : [] } }; I.filter.ID = function (Y, W) { var X = typeof Y.getAttributeNode !== "undefined" && Y.getAttributeNode("id"); return Y.nodeType === 1 && X && X.nodeValue === W } } T.removeChild(U) })(); (function () { var T = document.createElement("div"); T.appendChild(document.createComment("")); if (T.getElementsByTagName("*").length > 0) { I.find.TAG = function (U, Y) { var X = Y.getElementsByTagName(U[1]); if (U[1] === "*") { var W = []; for (var V = 0; X[V]; V++) { if (X[V].nodeType === 1) { W.push(X[V]) } } X = W } return X } } T.innerHTML = "<a href='#'></a>"; if (T.firstChild && typeof T.firstChild.getAttribute !== "undefined" && T.firstChild.getAttribute("href") !== "#") { I.attrHandle.href = function (U) { return U.getAttribute("href", 2) } } })(); if (document.querySelectorAll) { (function () { var T = F, U = document.createElement("div"); U.innerHTML = "<p class='TEST'></p>"; if (U.querySelectorAll && U.querySelectorAll(".TEST").length === 0) { return } F = function (Y, X, V, W) { X = X || document; if (!W && X.nodeType === 9 && !Q(X)) { try { return E(X.querySelectorAll(Y), V) } catch (Z) { } } return T(Y, X, V, W) }; F.find = T.find; F.filter = T.filter; F.selectors = T.selectors; F.matches = T.matches })() } if (document.getElementsByClassName && document.documentElement.getElementsByClassName) { (function () { var T = document.createElement("div"); T.innerHTML = "<div class='test e'></div><div class='test'></div>"; if (T.getElementsByClassName("e").length === 0) { return } T.lastChild.className = "e"; if (T.getElementsByClassName("e").length === 1) { return } I.order.splice(1, 0, "CLASS"); I.find.CLASS = function (U, V, W) { if (typeof V.getElementsByClassName !== "undefined" && !W) { return V.getElementsByClassName(U[1]) } } })() } function P(U, Z, Y, ad, aa, ac) { var ab = U == "previousSibling" && !ac; for (var W = 0, V = ad.length; W < V; W++) { var T = ad[W]; if (T) { if (ab && T.nodeType === 1) { T.sizcache = Y; T.sizset = W } T = T[U]; var X = false; while (T) { if (T.sizcache === Y) { X = ad[T.sizset]; break } if (T.nodeType === 1 && !ac) { T.sizcache = Y; T.sizset = W } if (T.nodeName === Z) { X = T; break } T = T[U] } ad[W] = X } } } function S(U, Z, Y, ad, aa, ac) { var ab = U == "previousSibling" && !ac; for (var W = 0, V = ad.length; W < V; W++) { var T = ad[W]; if (T) { if (ab && T.nodeType === 1) { T.sizcache = Y; T.sizset = W } T = T[U]; var X = false; while (T) { if (T.sizcache === Y) { X = ad[T.sizset]; break } if (T.nodeType === 1) { if (!ac) { T.sizcache = Y; T.sizset = W } if (typeof Z !== "string") { if (T === Z) { X = true; break } } else { if (F.filter(Z, [T]).length > 0) { X = T; break } } } T = T[U] } ad[W] = X } } } var K = document.compareDocumentPosition ? function (U, T) { return U.compareDocumentPosition(T) & 16 } : function (U, T) { return U !== T && (U.contains ? U.contains(T) : true) }; var Q = function (T) { return T.nodeType === 9 && T.documentElement.nodeName !== "HTML" || !!T.ownerDocument && Q(T.ownerDocument) }; var J = function (T, aa) { var W = [], X = "", Y, V = aa.nodeType ? [aa] : aa; while ((Y = I.match.PSEUDO.exec(T))) { X += Y[0]; T = T.replace(I.match.PSEUDO, "") } T = I.relative[T] ? T + "*" : T; for (var Z = 0, U = V.length; Z < U; Z++) { F(T, V[Z], W) } return F.filter(X, W) }; o.find = F; o.filter = F.filter; o.expr = F.selectors; o.expr[":"] = o.expr.filters; F.selectors.filters.hidden = function (T) { return T.offsetWidth === 0 || T.offsetHeight === 0 }; F.selectors.filters.visible = function (T) { return T.offsetWidth > 0 || T.offsetHeight > 0 }; F.selectors.filters.animated = function (T) { return o.grep(o.timers, function (U) { return T === U.elem }).length }; o.multiFilter = function (V, T, U) { if (U) { V = ":not(" + V + ")" } return F.matches(V, T) }; o.dir = function (V, U) { var T = [], W = V[U]; while (W && W != document) { if (W.nodeType == 1) { T.push(W) } W = W[U] } return T }; o.nth = function (X, T, V, W) { T = T || 1; var U = 0; for (; X; X = X[V]) { if (X.nodeType == 1 && ++U == T) { break } } return X }; o.sibling = function (V, U) { var T = []; for (; V; V = V.nextSibling) { if (V.nodeType == 1 && V != U) { T.push(V) } } return T }; return; l.Sizzle = F })(); o.event = { add: function (I, F, H, K) { if (I.nodeType == 3 || I.nodeType == 8) { return } if (I.setInterval && I != l) { I = l } if (!H.guid) { H.guid = this.guid++ } if (K !== g) { var G = H; H = this.proxy(G); H.data = K } var E = o.data(I, "events") || o.data(I, "events", {}), J = o.data(I, "handle") || o.data(I, "handle", function () { return typeof o !== "undefined" && !o.event.triggered ? o.event.handle.apply(arguments.callee.elem, arguments) : g }); J.elem = I; o.each(F.split(/\s+/), function (M, N) { var O = N.split("."); N = O.shift(); H.type = O.slice().sort().join("."); var L = E[N]; if (o.event.specialAll[N]) { o.event.specialAll[N].setup.call(I, K, O) } if (!L) { L = E[N] = {}; if (!o.event.special[N] || o.event.special[N].setup.call(I, K, O) === false) { if (I.addEventListener) { I.addEventListener(N, J, false) } else { if (I.attachEvent) { I.attachEvent("on" + N, J) } } } } L[H.guid] = H; o.event.global[N] = true }); I = null }, guid: 1, global: {}, remove: function (K, H, J) { if (K.nodeType == 3 || K.nodeType == 8) { return } var G = o.data(K, "events"), F, E; if (G) { if (H === g || (typeof H === "string" && H.charAt(0) == ".")) { for (var I in G) { this.remove(K, I + (H || "")) } } else { if (H.type) { J = H.handler; H = H.type } o.each(H.split(/\s+/), function (M, O) { var Q = O.split("."); O = Q.shift(); var N = RegExp("(^|\\.)" + Q.slice().sort().join(".*\\.") + "(\\.|$)"); if (G[O]) { if (J) { delete G[O][J.guid] } else { for (var P in G[O]) { if (N.test(G[O][P].type)) { delete G[O][P] } } } if (o.event.specialAll[O]) { o.event.specialAll[O].teardown.call(K, Q) } for (F in G[O]) { break } if (!F) { if (!o.event.special[O] || o.event.special[O].teardown.call(K, Q) === false) { if (K.removeEventListener) { K.removeEventListener(O, o.data(K, "handle"), false) } else { if (K.detachEvent) { K.detachEvent("on" + O, o.data(K, "handle")) } } } F = null; delete G[O] } } }) } for (F in G) { break } if (!F) { var L = o.data(K, "handle"); if (L) { L.elem = null } o.removeData(K, "events"); o.removeData(K, "handle") } } }, trigger: function (I, K, H, E) { var G = I.type || I; if (!E) { I = typeof I === "object" ? I[h] ? I : o.extend(o.Event(G), I) : o.Event(G); if (G.indexOf("!") >= 0) { I.type = G = G.slice(0, -1); I.exclusive = true } if (!H) { I.stopPropagation(); if (this.global[G]) { o.each(o.cache, function () { if (this.events && this.events[G]) { o.event.trigger(I, K, this.handle.elem) } }) } } if (!H || H.nodeType == 3 || H.nodeType == 8) { return g } I.result = g; I.target = H; K = o.makeArray(K); K.unshift(I) } I.currentTarget = H; var J = o.data(H, "handle"); if (J) { J.apply(H, K) } if ((!H[G] || (o.nodeName(H, "a") && G == "click")) && H["on" + G] && H["on" + G].apply(H, K) === false) { I.result = false } if (!E && H[G] && !I.isDefaultPrevented() && !(o.nodeName(H, "a") && G == "click")) { this.triggered = true; try { H[G]() } catch (L) { } } this.triggered = false; if (!I.isPropagationStopped()) { var F = H.parentNode || H.ownerDocument; if (F) { o.event.trigger(I, K, F, true) } } }, handle: function (K) { var J, E; K = arguments[0] = o.event.fix(K || l.event); K.currentTarget = this; var L = K.type.split("."); K.type = L.shift(); J = !L.length && !K.exclusive; var I = RegExp("(^|\\.)" + L.slice().sort().join(".*\\.") + "(\\.|$)"); E = (o.data(this, "events") || {})[K.type]; for (var G in E) { var H = E[G]; if (J || I.test(H.type)) { K.handler = H; K.data = H.data; var F = H.apply(this, arguments); if (F !== g) { K.result = F; if (F === false) { K.preventDefault(); K.stopPropagation() } } if (K.isImmediatePropagationStopped()) { break } } } }, props: "altKey attrChange attrName bubbles button cancelable charCode clientX clientY ctrlKey currentTarget data detail eventPhase fromElement handler keyCode metaKey newValue originalTarget pageX pageY prevValue relatedNode relatedTarget screenX screenY shiftKey srcElement target toElement view wheelDelta which".split(" "), fix: function (H) { if (H[h]) { return H } var F = H; H = o.Event(F); for (var G = this.props.length, J; G; ) { J = this.props[--G]; H[J] = F[J] } if (!H.target) { H.target = H.srcElement || document } if (H.target.nodeType == 3) { H.target = H.target.parentNode } if (!H.relatedTarget && H.fromElement) { H.relatedTarget = H.fromElement == H.target ? H.toElement : H.fromElement } if (H.pageX == null && H.clientX != null) { var I = document.documentElement, E = document.body; H.pageX = H.clientX + (I && I.scrollLeft || E && E.scrollLeft || 0) - (I.clientLeft || 0); H.pageY = H.clientY + (I && I.scrollTop || E && E.scrollTop || 0) - (I.clientTop || 0) } if (!H.which && ((H.charCode || H.charCode === 0) ? H.charCode : H.keyCode)) { H.which = H.charCode || H.keyCode } if (!H.metaKey && H.ctrlKey) { H.metaKey = H.ctrlKey } if (!H.which && H.button) { H.which = (H.button & 1 ? 1 : (H.button & 2 ? 3 : (H.button & 4 ? 2 : 0))) } return H }, proxy: function (F, E) { E = E || function () { return F.apply(this, arguments) }; E.guid = F.guid = F.guid || E.guid || this.guid++; return E }, special: { ready: { setup: B, teardown: function () { } } }, specialAll: { live: { setup: function (E, F) { o.event.add(this, F[0], c) }, teardown: function (G) { if (G.length) { var E = 0, F = RegExp("(^|\\.)" + G[0] + "(\\.|$)"); o.each((o.data(this, "events").live || {}), function () { if (F.test(this.type)) { E++ } }); if (E < 1) { o.event.remove(this, G[0], c) } } } }} }; o.Event = function (E) { if (!this.preventDefault) { return new o.Event(E) } if (E && E.type) { this.originalEvent = E; this.type = E.type } else { this.type = E } this.timeStamp = e(); this[h] = true }; function k() { return false } function u() { return true } o.Event.prototype = { preventDefault: function () { this.isDefaultPrevented = u; var E = this.originalEvent; if (!E) { return } if (E.preventDefault) { E.preventDefault() } E.returnValue = false }, stopPropagation: function () { this.isPropagationStopped = u; var E = this.originalEvent; if (!E) { return } if (E.stopPropagation) { E.stopPropagation() } E.cancelBubble = true }, stopImmediatePropagation: function () { this.isImmediatePropagationStopped = u; this.stopPropagation() }, isDefaultPrevented: k, isPropagationStopped: k, isImmediatePropagationStopped: k }; var a = function (F) { var E = F.relatedTarget; while (E && E != this) { try { E = E.parentNode } catch (G) { E = this } } if (E != this) { F.type = F.data; o.event.handle.apply(this, arguments) } }; o.each({ mouseover: "mouseenter", mouseout: "mouseleave" }, function (F, E) { o.event.special[E] = { setup: function () { o.event.add(this, F, a, E) }, teardown: function () { o.event.remove(this, F, a) } } }); o.fn.extend({ bind: function (F, G, E) { return F == "unload" ? this.one(F, G, E) : this.each(function () { o.event.add(this, F, E || G, E && G) }) }, one: function (G, H, F) { var E = o.event.proxy(F || H, function (I) { o(this).unbind(I, E); return (F || H).apply(this, arguments) }); return this.each(function () { o.event.add(this, G, E, F && H) }) }, unbind: function (F, E) { return this.each(function () { o.event.remove(this, F, E) }) }, trigger: function (E, F) { return this.each(function () { o.event.trigger(E, F, this) }) }, triggerHandler: function (E, G) { if (this[0]) { var F = o.Event(E); F.preventDefault(); F.stopPropagation(); o.event.trigger(F, G, this[0]); return F.result } }, toggle: function (G) { var E = arguments, F = 1; while (F < E.length) { o.event.proxy(G, E[F++]) } return this.click(o.event.proxy(G, function (H) { this.lastToggle = (this.lastToggle || 0) % F; H.preventDefault(); return E[this.lastToggle++].apply(this, arguments) || false })) }, hover: function (E, F) { return this.mouseenter(E).mouseleave(F) }, ready: function (E) { B(); if (o.isReady) { E.call(document, o) } else { o.readyList.push(E) } return this }, live: function (G, F) { var E = o.event.proxy(F); E.guid += this.selector + G; o(document).bind(i(G, this.selector), this.selector, E); return this }, die: function (F, E) { o(document).unbind(i(F, this.selector), E ? { guid: E.guid + this.selector + F} : null); return this } }); function c(H) { var E = RegExp("(^|\\.)" + H.type + "(\\.|$)"), G = true, F = []; o.each(o.data(this, "events").live || [], function (I, J) { if (E.test(J.type)) { var K = o(H.target).closest(J.data)[0]; if (K) { F.push({ elem: K, fn: J }) } } }); F.sort(function (J, I) { return o.data(J.elem, "closest") - o.data(I.elem, "closest") }); o.each(F, function () { if (this.fn.call(this.elem, H, this.fn.data) === false) { return (G = false) } }); return G } function i(F, E) { return ["live", F, E.replace(/\./g, "`").replace(/ /g, "|")].join(".") } o.extend({ isReady: false, readyList: [], ready: function () { if (!o.isReady) { o.isReady = true; if (o.readyList) { o.each(o.readyList, function () { this.call(document, o) }); o.readyList = null } o(document).triggerHandler("ready") } } }); var x = false; function B() { if (x) { return } x = true; if (document.addEventListener) { document.addEventListener("DOMContentLoaded", function () { document.removeEventListener("DOMContentLoaded", arguments.callee, false); o.ready() }, false) } else { if (document.attachEvent) { document.attachEvent("onreadystatechange", function () { if (document.readyState === "complete") { document.detachEvent("onreadystatechange", arguments.callee); o.ready() } }); if (document.documentElement.doScroll && l == l.top) { (function () { if (o.isReady) { return } try { document.documentElement.doScroll("left") } catch (E) { setTimeout(arguments.callee, 0); return } o.ready() })() } } } o.event.add(l, "load", o.ready) } o.each(("blur,focus,load,resize,scroll,unload,click,dblclick,mousedown,mouseup,mousemove,mouseover,mouseout,mouseenter,mouseleave,change,select,submit,keydown,keypress,keyup,error").split(","), function (F, E) { o.fn[E] = function (G) { return G ? this.bind(E, G) : this.trigger(E) } }); o(l).bind("unload", function () { for (var E in o.cache) { if (E != 1 && o.cache[E].handle) { o.event.remove(o.cache[E].handle.elem) } } }); (function () { o.support = {}; var F = document.documentElement, G = document.createElement("script"), K = document.createElement("div"), J = "script" + (new Date).getTime(); K.style.display = "none"; K.innerHTML = '   <link/><table></table><a href="/a" style="color:red;float:left;opacity:.5;">a</a><select><option>text</option></select><object><param/></object>'; var H = K.getElementsByTagName("*"), E = K.getElementsByTagName("a")[0]; if (!H || !H.length || !E) { return } o.support = { leadingWhitespace: K.firstChild.nodeType == 3, tbody: !K.getElementsByTagName("tbody").length, objectAll: !!K.getElementsByTagName("object")[0].getElementsByTagName("*").length, htmlSerialize: !!K.getElementsByTagName("link").length, style: /red/.test(E.getAttribute("style")), hrefNormalized: E.getAttribute("href") === "/a", opacity: E.style.opacity === "0.5", cssFloat: !!E.style.cssFloat, scriptEval: false, noCloneEvent: true, boxModel: null }; G.type = "text/javascript"; try { G.appendChild(document.createTextNode("window." + J + "=1;")) } catch (I) { } F.insertBefore(G, F.firstChild); if (l[J]) { o.support.scriptEval = true; delete l[J] } F.removeChild(G); if (K.attachEvent && K.fireEvent) { K.attachEvent("onclick", function () { o.support.noCloneEvent = false; K.detachEvent("onclick", arguments.callee) }); K.cloneNode(true).fireEvent("onclick") } o(function () { var L = document.createElement("div"); L.style.width = L.style.paddingLeft = "1px"; document.body.appendChild(L); o.boxModel = o.support.boxModel = L.offsetWidth === 2; document.body.removeChild(L).style.display = "none" }) })(); var w = o.support.cssFloat ? "cssFloat" : "styleFloat"; o.props = { "for": "htmlFor", "class": "className", "float": w, cssFloat: w, styleFloat: w, readonly: "readOnly", maxlength: "maxLength", cellspacing: "cellSpacing", rowspan: "rowSpan", tabindex: "tabIndex" }; o.fn.extend({ _load: o.fn.load, load: function (G, J, K) { if (typeof G !== "string") { return this._load(G) } var I = G.indexOf(" "); if (I >= 0) { var E = G.slice(I, G.length); G = G.slice(0, I) } var H = "GET"; if (J) { if (o.isFunction(J)) { K = J; J = null } else { if (typeof J === "object") { J = o.param(J); H = "POST" } } } var F = this; o.ajax({ url: G, type: H, dataType: "html", data: J, complete: function (M, L) { if (L == "success" || L == "notmodified") { F.html(E ? o("<div/>").append(M.responseText.replace(/<script(.|\s)*?\/script>/g, "")).find(E) : M.responseText) } if (K) { F.each(K, [M.responseText, L, M]) } } }); return this }, serialize: function () { return o.param(this.serializeArray()) }, serializeArray: function () { return this.map(function () { return this.elements ? o.makeArray(this.elements) : this }).filter(function () { return this.name && !this.disabled && (this.checked || /select|textarea/i.test(this.nodeName) || /text|hidden|password|search/i.test(this.type)) }).map(function (E, F) { var G = o(this).val(); return G == null ? null : o.isArray(G) ? o.map(G, function (I, H) { return { name: F.name, value: I} }) : { name: F.name, value: G} }).get() } }); o.each("ajaxStart,ajaxStop,ajaxComplete,ajaxError,ajaxSuccess,ajaxSend".split(","), function (E, F) { o.fn[F] = function (G) { return this.bind(F, G) } }); var r = e(); o.extend({ get: function (E, G, H, F) { if (o.isFunction(G)) { H = G; G = null } return o.ajax({ type: "GET", url: E, data: G, success: H, dataType: F }) }, getScript: function (E, F) { return o.get(E, null, F, "script") }, getJSON: function (E, F, G) { return o.get(E, F, G, "json") }, post: function (E, G, H, F) { if (o.isFunction(G)) { H = G; G = {} } return o.ajax({ type: "POST", url: E, data: G, success: H, dataType: F }) }, ajaxSetup: function (E) { o.extend(o.ajaxSettings, E) }, ajaxSettings: { url: location.href, global: true, type: "GET", contentType: "application/x-www-form-urlencoded", processData: true, async: true, xhr: function () { return l.ActiveXObject ? new ActiveXObject("Microsoft.XMLHTTP") : new XMLHttpRequest() }, accepts: { xml: "application/xml, text/xml", html: "text/html", script: "text/javascript, application/javascript", json: "application/json, text/javascript", text: "text/plain", _default: "*/*"} }, lastModified: {}, ajax: function (M) { M = o.extend(true, M, o.extend(true, {}, o.ajaxSettings, M)); var W, F = /=\?(&|$)/g, R, V, G = M.type.toUpperCase(); if (M.data && M.processData && typeof M.data !== "string") { M.data = o.param(M.data) } if (M.dataType == "jsonp") { if (G == "GET") { if (!M.url.match(F)) { M.url += (M.url.match(/\?/) ? "&" : "?") + (M.jsonp || "callback") + "=?" } } else { if (!M.data || !M.data.match(F)) { M.data = (M.data ? M.data + "&" : "") + (M.jsonp || "callback") + "=?" } } M.dataType = "json" } if (M.dataType == "json" && (M.data && M.data.match(F) || M.url.match(F))) { W = "jsonp" + r++; if (M.data) { M.data = (M.data + "").replace(F, "=" + W + "$1") } M.url = M.url.replace(F, "=" + W + "$1"); M.dataType = "script"; l[W] = function (X) { V = X; I(); L(); l[W] = g; try { delete l[W] } catch (Y) { } if (H) { H.removeChild(T) } } } if (M.dataType == "script" && M.cache == null) { M.cache = false } if (M.cache === false && G == "GET") { var E = e(); var U = M.url.replace(/(\?|&)_=.*?(&|$)/, "$1_=" + E + "$2"); M.url = U + ((U == M.url) ? (M.url.match(/\?/) ? "&" : "?") + "_=" + E : "") } if (M.data && G == "GET") { M.url += (M.url.match(/\?/) ? "&" : "?") + M.data; M.data = null } if (M.global && !o.active++) { o.event.trigger("ajaxStart") } var Q = /^(\w+:)?\/\/([^\/?#]+)/.exec(M.url); if (M.dataType == "script" && G == "GET" && Q && (Q[1] && Q[1] != location.protocol || Q[2] != location.host)) { var H = document.getElementsByTagName("head")[0]; var T = document.createElement("script"); T.src = M.url; if (M.scriptCharset) { T.charset = M.scriptCharset } if (!W) { var O = false; T.onload = T.onreadystatechange = function () { if (!O && (!this.readyState || this.readyState == "loaded" || this.readyState == "complete")) { O = true; I(); L(); T.onload = T.onreadystatechange = null; H.removeChild(T) } } } H.appendChild(T); return g } var K = false; var J = M.xhr(); if (M.username) { J.open(G, M.url, M.async, M.username, M.password) } else { J.open(G, M.url, M.async) } try { if (M.data) { J.setRequestHeader("Content-Type", M.contentType) } if (M.ifModified) { J.setRequestHeader("If-Modified-Since", o.lastModified[M.url] || "Thu, 01 Jan 1970 00:00:00 GMT") } J.setRequestHeader("X-Requested-With", "XMLHttpRequest"); J.setRequestHeader("Accept", M.dataType && M.accepts[M.dataType] ? M.accepts[M.dataType] + ", */*" : M.accepts._default) } catch (S) { } if (M.beforeSend && M.beforeSend(J, M) === false) { if (M.global && ! --o.active) { o.event.trigger("ajaxStop") } J.abort(); return false } if (M.global) { o.event.trigger("ajaxSend", [J, M]) } var N = function (X) { if (J.readyState == 0) { if (P) { clearInterval(P); P = null; if (M.global && ! --o.active) { o.event.trigger("ajaxStop") } } } else { if (!K && J && (J.readyState == 4 || X == "timeout")) { K = true; if (P) { clearInterval(P); P = null } R = X == "timeout" ? "timeout" : !o.httpSuccess(J) ? "error" : M.ifModified && o.httpNotModified(J, M.url) ? "notmodified" : "success"; if (R == "success") { try { V = o.httpData(J, M.dataType, M) } catch (Z) { R = "parsererror" } } if (R == "success") { var Y; try { Y = J.getResponseHeader("Last-Modified") } catch (Z) { } if (M.ifModified && Y) { o.lastModified[M.url] = Y } if (!W) { I() } } else { o.handleError(M, J, R) } L(); if (X) { J.abort() } if (M.async) { J = null } } } }; if (M.async) { var P = setInterval(N, 13); if (M.timeout > 0) { setTimeout(function () { if (J && !K) { N("timeout") } }, M.timeout) } } try { J.send(M.data) } catch (S) { o.handleError(M, J, null, S) } if (!M.async) { N() } function I() { if (M.success) { M.success(V, R) } if (M.global) { o.event.trigger("ajaxSuccess", [J, M]) } } function L() { if (M.complete) { M.complete(J, R) } if (M.global) { o.event.trigger("ajaxComplete", [J, M]) } if (M.global && ! --o.active) { o.event.trigger("ajaxStop") } } return J }, handleError: function (F, H, E, G) { if (F.error) { F.error(H, E, G) } if (F.global) { o.event.trigger("ajaxError", [H, F, G]) } }, active: 0, httpSuccess: function (F) { try { return !F.status && location.protocol == "file:" || (F.status >= 200 && F.status < 300) || F.status == 304 || F.status == 1223 } catch (E) { } return false }, httpNotModified: function (G, E) { try { var H = G.getResponseHeader("Last-Modified"); return G.status == 304 || H == o.lastModified[E] } catch (F) { } return false }, httpData: function (J, H, G) { var F = J.getResponseHeader("content-type"), E = H == "xml" || !H && F && F.indexOf("xml") >= 0, I = E ? J.responseXML : J.responseText; if (E && I.documentElement.tagName == "parsererror") { throw "parsererror" } if (G && G.dataFilter) { I = G.dataFilter(I, H) } if (typeof I === "string") { if (H == "script") { o.globalEval(I) } if (H == "json") { I = l["eval"]("(" + I + ")") } } return I }, param: function (E) { var G = []; function H(I, J) { G[G.length] = encodeURIComponent(I) + "=" + encodeURIComponent(J) } if (o.isArray(E) || E.jquery) { o.each(E, function () { H(this.name, this.value) }) } else { for (var F in E) { if (o.isArray(E[F])) { o.each(E[F], function () { H(F, this) }) } else { H(F, o.isFunction(E[F]) ? E[F]() : E[F]) } } } return G.join("&").replace(/%20/g, "+") } }); var m = {}, n, d = [["height", "marginTop", "marginBottom", "paddingTop", "paddingBottom"], ["width", "marginLeft", "marginRight", "paddingLeft", "paddingRight"], ["opacity"]]; function t(F, E) { var G = {}; o.each(d.concat.apply([], d.slice(0, E)), function () { G[this] = F }); return G } o.fn.extend({ show: function (J, L) { if (J) { return this.animate(t("show", 3), J, L) } else { for (var H = 0, F = this.length; H < F; H++) { var E = o.data(this[H], "olddisplay"); this[H].style.display = E || ""; if (o.css(this[H], "display") === "none") { var G = this[H].tagName, K; if (m[G]) { K = m[G] } else { var I = o("<" + G + " />").appendTo("body"); K = I.css("display"); if (K === "none") { K = "block" } I.remove(); m[G] = K } o.data(this[H], "olddisplay", K) } } for (var H = 0, F = this.length; H < F; H++) { this[H].style.display = o.data(this[H], "olddisplay") || "" } return this } }, hide: function (H, I) { if (H) { return this.animate(t("hide", 3), H, I) } else { for (var G = 0, F = this.length; G < F; G++) { var E = o.data(this[G], "olddisplay"); if (!E && E !== "none") { o.data(this[G], "olddisplay", o.css(this[G], "display")) } } for (var G = 0, F = this.length; G < F; G++) { this[G].style.display = "none" } return this } }, _toggle: o.fn.toggle, toggle: function (G, F) { var E = typeof G === "boolean"; return o.isFunction(G) && o.isFunction(F) ? this._toggle.apply(this, arguments) : G == null || E ? this.each(function () { var H = E ? G : o(this).is(":hidden"); o(this)[H ? "show" : "hide"]() }) : this.animate(t("toggle", 3), G, F) }, fadeTo: function (E, G, F) { return this.animate({ opacity: G }, E, F) }, animate: function (I, F, H, G) { var E = o.speed(F, H, G); return this[E.queue === false ? "each" : "queue"](function () { var K = o.extend({}, E), M, L = this.nodeType == 1 && o(this).is(":hidden"), J = this; for (M in I) { if (I[M] == "hide" && L || I[M] == "show" && !L) { return K.complete.call(this) } if ((M == "height" || M == "width") && this.style) { K.display = o.css(this, "display"); K.overflow = this.style.overflow } } if (K.overflow != null) { this.style.overflow = "hidden" } K.curAnim = o.extend({}, I); o.each(I, function (O, S) { var R = new o.fx(J, K, O); if (/toggle|show|hide/.test(S)) { R[S == "toggle" ? L ? "show" : "hide" : S](I) } else { var Q = S.toString().match(/^([+-]=)?([\d+-.]+)(.*)$/), T = R.cur(true) || 0; if (Q) { var N = parseFloat(Q[2]), P = Q[3] || "px"; if (P != "px") { J.style[O] = (N || 1) + P; T = ((N || 1) / R.cur(true)) * T; J.style[O] = T + P } if (Q[1]) { N = ((Q[1] == "-=" ? -1 : 1) * N) + T } R.custom(T, N, P) } else { R.custom(T, S, "") } } }); return true }) }, stop: function (F, E) { var G = o.timers; if (F) { this.queue([]) } this.each(function () { for (var H = G.length - 1; H >= 0; H--) { if (G[H].elem == this) { if (E) { G[H](true) } G.splice(H, 1) } } }); if (!E) { this.dequeue() } return this } }); o.each({ slideDown: t("show", 1), slideUp: t("hide", 1), slideToggle: t("toggle", 1), fadeIn: { opacity: "show" }, fadeOut: { opacity: "hide"} }, function (E, F) { o.fn[E] = function (G, H) { return this.animate(F, G, H) } }); o.extend({ speed: function (G, H, F) { var E = typeof G === "object" ? G : { complete: F || !F && H || o.isFunction(G) && G, duration: G, easing: F && H || H && !o.isFunction(H) && H }; E.duration = o.fx.off ? 0 : typeof E.duration === "number" ? E.duration : o.fx.speeds[E.duration] || o.fx.speeds._default; E.old = E.complete; E.complete = function () { if (E.queue !== false) { o(this).dequeue() } if (o.isFunction(E.old)) { E.old.call(this) } }; return E }, easing: { linear: function (G, H, E, F) { return E + F * G }, swing: function (G, H, E, F) { return ((-Math.cos(G * Math.PI) / 2) + 0.5) * F + E } }, timers: [], fx: function (F, E, G) { this.options = E; this.elem = F; this.prop = G; if (!E.orig) { E.orig = {} } } }); o.fx.prototype = { update: function () { if (this.options.step) { this.options.step.call(this.elem, this.now, this) } (o.fx.step[this.prop] || o.fx.step._default)(this); if ((this.prop == "height" || this.prop == "width") && this.elem.style) { this.elem.style.display = "block" } }, cur: function (F) { if (this.elem[this.prop] != null && (!this.elem.style || this.elem.style[this.prop] == null)) { return this.elem[this.prop] } var E = parseFloat(o.css(this.elem, this.prop, F)); return E && E > -10000 ? E : parseFloat(o.curCSS(this.elem, this.prop)) || 0 }, custom: function (I, H, G) { this.startTime = e(); this.start = I; this.end = H; this.unit = G || this.unit || "px"; this.now = this.start; this.pos = this.state = 0; var E = this; function F(J) { return E.step(J) } F.elem = this.elem; if (F() && o.timers.push(F) && !n) { n = setInterval(function () { var K = o.timers; for (var J = 0; J < K.length; J++) { if (!K[J]()) { K.splice(J--, 1) } } if (!K.length) { clearInterval(n); n = g } }, 13) } }, show: function () { this.options.orig[this.prop] = o.attr(this.elem.style, this.prop); this.options.show = true; this.custom(this.prop == "width" || this.prop == "height" ? 1 : 0, this.cur()); o(this.elem).show() }, hide: function () { this.options.orig[this.prop] = o.attr(this.elem.style, this.prop); this.options.hide = true; this.custom(this.cur(), 0) }, step: function (H) { var G = e(); if (H || G >= this.options.duration + this.startTime) { this.now = this.end; this.pos = this.state = 1; this.update(); this.options.curAnim[this.prop] = true; var E = true; for (var F in this.options.curAnim) { if (this.options.curAnim[F] !== true) { E = false } } if (E) { if (this.options.display != null) { this.elem.style.overflow = this.options.overflow; this.elem.style.display = this.options.display; if (o.css(this.elem, "display") == "none") { this.elem.style.display = "block" } } if (this.options.hide) { o(this.elem).hide() } if (this.options.hide || this.options.show) { for (var I in this.options.curAnim) { o.attr(this.elem.style, I, this.options.orig[I]) } } this.options.complete.call(this.elem) } return false } else { var J = G - this.startTime; this.state = J / this.options.duration; this.pos = o.easing[this.options.easing || (o.easing.swing ? "swing" : "linear")](this.state, J, 0, 1, this.options.duration); this.now = this.start + ((this.end - this.start) * this.pos); this.update() } return true } }; o.extend(o.fx, { speeds: { slow: 600, fast: 200, _default: 400 }, step: { opacity: function (E) { o.attr(E.elem.style, "opacity", E.now) }, _default: function (E) { if (E.elem.style && E.elem.style[E.prop] != null) { E.elem.style[E.prop] = E.now + E.unit } else { E.elem[E.prop] = E.now } } } }); if (document.documentElement.getBoundingClientRect) { o.fn.offset = function () { if (!this[0]) { return { top: 0, left: 0} } if (this[0] === this[0].ownerDocument.body) { return o.offset.bodyOffset(this[0]) } var G = this[0].getBoundingClientRect(), J = this[0].ownerDocument, F = J.body, E = J.documentElement, L = E.clientTop || F.clientTop || 0, K = E.clientLeft || F.clientLeft || 0, I = G.top + (self.pageYOffset || o.boxModel && E.scrollTop || F.scrollTop) - L, H = G.left + (self.pageXOffset || o.boxModel && E.scrollLeft || F.scrollLeft) - K; return { top: I, left: H} } } else { o.fn.offset = function () { if (!this[0]) { return { top: 0, left: 0} } if (this[0] === this[0].ownerDocument.body) { return o.offset.bodyOffset(this[0]) } o.offset.initialized || o.offset.initialize(); var J = this[0], G = J.offsetParent, F = J, O = J.ownerDocument, M, H = O.documentElement, K = O.body, L = O.defaultView, E = L.getComputedStyle(J, null), N = J.offsetTop, I = J.offsetLeft; while ((J = J.parentNode) && J !== K && J !== H) { M = L.getComputedStyle(J, null); N -= J.scrollTop, I -= J.scrollLeft; if (J === G) { N += J.offsetTop, I += J.offsetLeft; if (o.offset.doesNotAddBorder && !(o.offset.doesAddBorderForTableAndCells && /^t(able|d|h)$/i.test(J.tagName))) { N += parseInt(M.borderTopWidth, 10) || 0, I += parseInt(M.borderLeftWidth, 10) || 0 } F = G, G = J.offsetParent } if (o.offset.subtractsBorderForOverflowNotVisible && M.overflow !== "visible") { N += parseInt(M.borderTopWidth, 10) || 0, I += parseInt(M.borderLeftWidth, 10) || 0 } E = M } if (E.position === "relative" || E.position === "static") { N += K.offsetTop, I += K.offsetLeft } if (E.position === "fixed") { N += Math.max(H.scrollTop, K.scrollTop), I += Math.max(H.scrollLeft, K.scrollLeft) } return { top: N, left: I} } } o.offset = { initialize: function () { if (this.initialized) { return } var L = document.body, F = document.createElement("div"), H, G, N, I, M, E, J = L.style.marginTop, K = '<div style="position:absolute;top:0;left:0;margin:0;border:5px solid #000;padding:0;width:1px;height:1px;"><div></div></div><table style="position:absolute;top:0;left:0;margin:0;border:5px solid #000;padding:0;width:1px;height:1px;" cellpadding="0" cellspacing="0"><tr><td></td></tr></table>'; M = { position: "absolute", top: 0, left: 0, margin: 0, border: 0, width: "1px", height: "1px", visibility: "hidden" }; for (E in M) { F.style[E] = M[E] } F.innerHTML = K; L.insertBefore(F, L.firstChild); H = F.firstChild, G = H.firstChild, I = H.nextSibling.firstChild.firstChild; this.doesNotAddBorder = (G.offsetTop !== 5); this.doesAddBorderForTableAndCells = (I.offsetTop === 5); H.style.overflow = "hidden", H.style.position = "relative"; this.subtractsBorderForOverflowNotVisible = (G.offsetTop === -5); L.style.marginTop = "1px"; this.doesNotIncludeMarginInBodyOffset = (L.offsetTop === 0); L.style.marginTop = J; L.removeChild(F); this.initialized = true }, bodyOffset: function (E) { o.offset.initialized || o.offset.initialize(); var G = E.offsetTop, F = E.offsetLeft; if (o.offset.doesNotIncludeMarginInBodyOffset) { G += parseInt(o.curCSS(E, "marginTop", true), 10) || 0, F += parseInt(o.curCSS(E, "marginLeft", true), 10) || 0 } return { top: G, left: F} } }; o.fn.extend({ position: function () { var I = 0, H = 0, F; if (this[0]) { var G = this.offsetParent(), J = this.offset(), E = /^body|html$/i.test(G[0].tagName) ? { top: 0, left: 0} : G.offset(); J.top -= j(this, "marginTop"); J.left -= j(this, "marginLeft"); E.top += j(G, "borderTopWidth"); E.left += j(G, "borderLeftWidth"); F = { top: J.top - E.top, left: J.left - E.left} } return F }, offsetParent: function () { var E = this[0].offsetParent || document.body; while (E && (!/^body|html$/i.test(E.tagName) && o.css(E, "position") == "static")) { E = E.offsetParent } return o(E) } }); o.each(["Left", "Top"], function (F, E) { var G = "scroll" + E; o.fn[G] = function (H) { if (!this[0]) { return null } return H !== g ? this.each(function () { this == l || this == document ? l.scrollTo(!F ? H : o(l).scrollLeft(), F ? H : o(l).scrollTop()) : this[G] = H }) : this[0] == l || this[0] == document ? self[F ? "pageYOffset" : "pageXOffset"] || o.boxModel && document.documentElement[G] || document.body[G] : this[0][G] } }); o.each(["Height", "Width"], function (I, G) { var E = I ? "Left" : "Top", H = I ? "Right" : "Bottom", F = G.toLowerCase(); o.fn["inner" + G] = function () { return this[0] ? o.css(this[0], F, false, "padding") : null }; o.fn["outer" + G] = function (K) { return this[0] ? o.css(this[0], F, false, K ? "margin" : "border") : null }; var J = G.toLowerCase(); o.fn[J] = function (K) { return this[0] == l ? document.compatMode == "CSS1Compat" && document.documentElement["client" + G] || document.body["client" + G] : this[0] == document ? Math.max(document.documentElement["client" + G], document.body["scroll" + G], document.documentElement["scroll" + G], document.body["offset" + G], document.documentElement["offset" + G]) : K === g ? (this.length ? o.css(this[0], J) : null) : this.css(J, typeof K === "string" ? K : K + "px") } })
})();




; (function ($) {
    $.ui = { plugin: { add: function (module, option, set) { var proto = $.ui[module].prototype; for (var i in set) { proto.plugins[i] = proto.plugins[i] || []; proto.plugins[i].push([option, set[i]]); } }, call: function (instance, name, args) {
        var set = instance.plugins[name]; if (!set) { return; }
        for (var i = 0; i < set.length; i++) { if (instance.options[set[i][0]]) { set[i][1].apply(instance.element, args); } } 
    } 
    }, cssCache: {}, css: function (name) {
        if ($.ui.cssCache[name]) { return $.ui.cssCache[name]; }
        var tmp = $('<div class="ui-gen">').addClass(name).css({ position: 'absolute', top: '-5000px', left: '-5000px', display: 'block' }).appendTo('body'); $.ui.cssCache[name] = !!((!(/auto|default/).test(tmp.css('cursor')) || (/^[1-9]/).test(tmp.css('height')) || (/^[1-9]/).test(tmp.css('width')) || !(/none/).test(tmp.css('backgroundImage')) || !(/transparent|rgba\(0, 0, 0, 0\)/).test(tmp.css('backgroundColor')))); try { $('body').get(0).removeChild(tmp.get(0)); } catch (e) { }
        return $.ui.cssCache[name];
    }, disableSelection: function (el) { $(el).attr('unselectable', 'on').css('MozUserSelect', 'none'); }, enableSelection: function (el) { $(el).attr('unselectable', 'off').css('MozUserSelect', ''); }, hasScroll: function (e, a) { var scroll = /top/.test(a || "top") ? 'scrollTop' : 'scrollLeft', has = false; if (e[scroll] > 0) return true; e[scroll] = 1; has = e[scroll] > 0 ? true : false; e[scroll] = 0; return has; } 
    }; var _remove = $.fn.remove; $.fn.remove = function () { $("*", this).add(this).triggerHandler("remove"); return _remove.apply(this, arguments); }; function getter(namespace, plugin, method) { var methods = $[namespace][plugin].getter || []; methods = (typeof methods == "string" ? methods.split(/,?\s+/) : methods); return ($.inArray(method, methods) != -1); }
    $.widget = function (name, prototype) {
        var namespace = name.split(".")[0]; name = name.split(".")[1]; $.fn[name] = function (options) {
            var isMethodCall = (typeof options == 'string'), args = Array.prototype.slice.call(arguments, 1); if (isMethodCall && getter(namespace, name, options)) { var instance = $.data(this[0], name); return (instance ? instance[options].apply(instance, args) : undefined); }
            return this.each(function () { var instance = $.data(this, name); if (isMethodCall && instance && $.isFunction(instance[options])) { instance[options].apply(instance, args); } else if (!isMethodCall) { $.data(this, name, new $[namespace][name](this, options)); } });
        }; $[namespace][name] = function (element, options) { var self = this; this.widgetName = name; this.widgetBaseClass = namespace + '-' + name; this.options = $.extend({}, $.widget.defaults, $[namespace][name].defaults, options); this.element = $(element).bind('setData.' + name, function (e, key, value) { return self.setData(key, value); }).bind('getData.' + name, function (e, key) { return self.getData(key); }).bind('remove', function () { return self.destroy(); }); this.init(); }; $[namespace][name].prototype = $.extend({}, $.widget.prototype, prototype);
    }; $.widget.prototype = { init: function () { }, destroy: function () { this.element.removeData(this.widgetName); }, getData: function (key) { return this.options[key]; }, setData: function (key, value) { this.options[key] = value; if (key == 'disabled') { this.element[value ? 'addClass' : 'removeClass'](this.widgetBaseClass + '-disabled'); } }, enable: function () { this.setData('disabled', false); }, disable: function () { this.setData('disabled', true); } }; $.widget.defaults = { disabled: false }; $.ui.mouse = { mouseInit: function () {
        var self = this; this.element.bind('mousedown.' + this.widgetName, function (e) { return self.mouseDown(e); }); if ($.browser.msie) { this._mouseUnselectable = this.element.attr('unselectable'); this.element.attr('unselectable', 'on'); }
        this.started = false;
    }, mouseDestroy: function () { this.element.unbind('.' + this.widgetName); ($.browser.msie && this.element.attr('unselectable', this._mouseUnselectable)); }, mouseDown: function (e) {
        (this._mouseStarted && this.mouseUp(e)); this._mouseDownEvent = e; var self = this, btnIsLeft = (e.which == 1), elIsCancel = (typeof this.options.cancel == "string" ? $(e.target).parents().add(e.target).filter(this.options.cancel).length : false); if (!btnIsLeft || elIsCancel || !this.mouseCapture(e)) { return true; }
        this._mouseDelayMet = !this.options.delay; if (!this._mouseDelayMet) { this._mouseDelayTimer = setTimeout(function () { self._mouseDelayMet = true; }, this.options.delay); }
        if (this.mouseDistanceMet(e) && this.mouseDelayMet(e)) { this._mouseStarted = (this.mouseStart(e) !== false); if (!this._mouseStarted) { e.preventDefault(); return true; } }
        this._mouseMoveDelegate = function (e) { return self.mouseMove(e); }; this._mouseUpDelegate = function (e) { return self.mouseUp(e); }; $(document).bind('mousemove.' + this.widgetName, this._mouseMoveDelegate).bind('mouseup.' + this.widgetName, this._mouseUpDelegate); return false;
    }, mouseMove: function (e) {
        if ($.browser.msie && !e.button) { return this.mouseUp(e); }
        if (this._mouseStarted) { this.mouseDrag(e); return false; }
        if (this.mouseDistanceMet(e) && this.mouseDelayMet(e)) { this._mouseStarted = (this.mouseStart(this._mouseDownEvent, e) !== false); (this._mouseStarted ? this.mouseDrag(e) : this.mouseUp(e)); }
        return !this._mouseStarted;
    }, mouseUp: function (e) {
        $(document).unbind('mousemove.' + this.widgetName, this._mouseMoveDelegate).unbind('mouseup.' + this.widgetName, this._mouseUpDelegate); if (this._mouseStarted) { this._mouseStarted = false; this.mouseStop(e); }
        return false;
    }, mouseDistanceMet: function (e) { return (Math.max(Math.abs(this._mouseDownEvent.pageX - e.pageX), Math.abs(this._mouseDownEvent.pageY - e.pageY)) >= this.options.distance); }, mouseDelayMet: function (e) { return this._mouseDelayMet; }, mouseStart: function (e) { }, mouseDrag: function (e) { }, mouseStop: function (e) { }, mouseCapture: function (e) { return true; } 
    }; $.ui.mouse.defaults = { cancel: null, distance: 1, delay: 0 };
})(jQuery); (function ($) {
    $.widget("ui.draggable", $.extend({}, $.ui.mouse, { init: function () {
        var o = this.options; if (o.helper == 'original' && !(/(relative|absolute|fixed)/).test(this.element.css('position')))
            this.element.css('position', 'relative'); this.element.addClass('ui-draggable'); (o.disabled && this.element.addClass('ui-draggable-disabled')); this.mouseInit();
    }, mouseStart: function (e) {
        var o = this.options; if (this.helper || o.disabled || $(e.target).is('.ui-resizable-handle')) return false; var handle = !this.options.handle || !$(this.options.handle, this.element).length ? true : false; $(this.options.handle, this.element).find("*").andSelf().each(function () { if (this == e.target) handle = true; }); if (!handle) return false; if ($.ui.ddmanager) $.ui.ddmanager.current = this; this.helper = $.isFunction(o.helper) ? $(o.helper.apply(this.element[0], [e])) : (o.helper == 'clone' ? this.element.clone() : this.element); if (!this.helper.parents('body').length) this.helper.appendTo((o.appendTo == 'parent' ? this.element[0].parentNode : o.appendTo)); if (this.helper[0] != this.element[0] && !(/(fixed|absolute)/).test(this.helper.css("position"))) this.helper.css("position", "absolute"); this.margins = { left: (parseInt(this.element.css("marginLeft"), 10) || 0), top: (parseInt(this.element.css("marginTop"), 10) || 0) }; this.cssPosition = this.helper.css("position"); this.offset = this.element.offset(); this.offset = { top: this.offset.top - this.margins.top, left: this.offset.left - this.margins.left }; this.offset.click = { left: e.pageX - this.offset.left, top: e.pageY - this.offset.top }; this.offsetParent = this.helper.offsetParent(); var po = this.offsetParent.offset(); if (this.offsetParent[0] == document.body && $.browser.mozilla) po = { top: 0, left: 0 }; this.offset.parent = { top: po.top + (parseInt(this.offsetParent.css("borderTopWidth"), 10) || 0), left: po.left + (parseInt(this.offsetParent.css("borderLeftWidth"), 10) || 0) }; var p = this.element.position(); this.offset.relative = this.cssPosition == "relative" ? { top: p.top - (parseInt(this.helper.css("top"), 10) || 0) + this.offsetParent[0].scrollTop, left: p.left - (parseInt(this.helper.css("left"), 10) || 0) + this.offsetParent[0].scrollLeft} : { top: 0, left: 0 }; this.originalPosition = this.generatePosition(e); this.helperProportions = { width: this.helper.outerWidth(), height: this.helper.outerHeight() }; if (o.cursorAt) { if (o.cursorAt.left != undefined) this.offset.click.left = o.cursorAt.left + this.margins.left; if (o.cursorAt.right != undefined) this.offset.click.left = this.helperProportions.width - o.cursorAt.right + this.margins.left; if (o.cursorAt.top != undefined) this.offset.click.top = o.cursorAt.top + this.margins.top; if (o.cursorAt.bottom != undefined) this.offset.click.top = this.helperProportions.height - o.cursorAt.bottom + this.margins.top; }
        if (o.containment) { if (o.containment == 'parent') o.containment = this.helper[0].parentNode; if (o.containment == 'document' || o.containment == 'window') this.containment = [0 - this.offset.relative.left - this.offset.parent.left, 0 - this.offset.relative.top - this.offset.parent.top, $(o.containment == 'document' ? document : window).width() - this.offset.relative.left - this.offset.parent.left - this.helperProportions.width - this.margins.left - (parseInt(this.element.css("marginRight"), 10) || 0), ($(o.containment == 'document' ? document : window).height() || document.body.parentNode.scrollHeight) - this.offset.relative.top - this.offset.parent.top - this.helperProportions.height - this.margins.top - (parseInt(this.element.css("marginBottom"), 10) || 0)]; if (!(/^(document|window|parent)$/).test(o.containment)) { var ce = $(o.containment)[0]; var co = $(o.containment).offset(); this.containment = [co.left + (parseInt($(ce).css("borderLeftWidth"), 10) || 0) - this.offset.relative.left - this.offset.parent.left, co.top + (parseInt($(ce).css("borderTopWidth"), 10) || 0) - this.offset.relative.top - this.offset.parent.top, co.left + Math.max(ce.scrollWidth, ce.offsetWidth) - (parseInt($(ce).css("borderLeftWidth"), 10) || 0) - this.offset.relative.left - this.offset.parent.left - this.helperProportions.width - this.margins.left - (parseInt(this.element.css("marginRight"), 10) || 0), co.top + Math.max(ce.scrollHeight, ce.offsetHeight) - (parseInt($(ce).css("borderTopWidth"), 10) || 0) - this.offset.relative.top - this.offset.parent.top - this.helperProportions.height - this.margins.top - (parseInt(this.element.css("marginBottom"), 10) || 0)]; } }
        this.propagate("start", e); this.helperProportions = { width: this.helper.outerWidth(), height: this.helper.outerHeight() }; if ($.ui.ddmanager && !o.dropBehaviour) $.ui.ddmanager.prepareOffsets(this, e); this.helper.addClass("ui-draggable-dragging"); this.mouseDrag(e); return true;
    }, convertPositionTo: function (d, pos) {
        if (!pos) pos = this.position; var mod = d == "absolute" ? 1 : -1; return { top: (pos.top
+ this.offset.relative.top * mod
+ this.offset.parent.top * mod
- (this.cssPosition == "fixed" || (this.cssPosition == "absolute" && this.offsetParent[0] == document.body) ? 0 : this.offsetParent[0].scrollTop) * mod
+ (this.cssPosition == "fixed" ? $(document).scrollTop() : 0) * mod
+ this.margins.top * mod), left: (pos.left
+ this.offset.relative.left * mod
+ this.offset.parent.left * mod
- (this.cssPosition == "fixed" || (this.cssPosition == "absolute" && this.offsetParent[0] == document.body) ? 0 : this.offsetParent[0].scrollLeft) * mod
+ (this.cssPosition == "fixed" ? $(document).scrollLeft() : 0) * mod
+ this.margins.left * mod)
        };
    }, generatePosition: function (e) {
        var o = this.options; var position = { top: (e.pageY
- this.offset.click.top
- this.offset.relative.top
- this.offset.parent.top
+ (this.cssPosition == "fixed" || (this.cssPosition == "absolute" && this.offsetParent[0] == document.body) ? 0 : this.offsetParent[0].scrollTop)
- (this.cssPosition == "fixed" ? $(document).scrollTop() : 0)), left: (e.pageX
- this.offset.click.left
- this.offset.relative.left
- this.offset.parent.left
+ (this.cssPosition == "fixed" || (this.cssPosition == "absolute" && this.offsetParent[0] == document.body) ? 0 : this.offsetParent[0].scrollLeft)
- (this.cssPosition == "fixed" ? $(document).scrollLeft() : 0))
        }; if (!this.originalPosition) return position; if (this.containment) { if (position.left < this.containment[0]) position.left = this.containment[0]; if (position.top < this.containment[1]) position.top = this.containment[1]; if (position.left > this.containment[2]) position.left = this.containment[2]; if (position.top > this.containment[3]) position.top = this.containment[3]; }
        if (o.grid) { var top = this.originalPosition.top + Math.round((position.top - this.originalPosition.top) / o.grid[1]) * o.grid[1]; position.top = this.containment ? (!(top < this.containment[1] || top > this.containment[3]) ? top : (!(top < this.containment[1]) ? top - o.grid[1] : top + o.grid[1])) : top; var left = this.originalPosition.left + Math.round((position.left - this.originalPosition.left) / o.grid[0]) * o.grid[0]; position.left = this.containment ? (!(left < this.containment[0] || left > this.containment[2]) ? left : (!(left < this.containment[0]) ? left - o.grid[0] : left + o.grid[0])) : left; }
        return position;
    }, mouseDrag: function (e) { this.position = this.generatePosition(e); this.positionAbs = this.convertPositionTo("absolute"); this.position = this.propagate("drag", e) || this.position; if (!this.options.axis || this.options.axis != "y") this.helper[0].style.left = this.position.left + 'px'; if (!this.options.axis || this.options.axis != "x") this.helper[0].style.top = this.position.top + 'px'; if ($.ui.ddmanager) $.ui.ddmanager.drag(this, e); return false; }, mouseStop: function (e) {
        var dropped = false; if ($.ui.ddmanager && !this.options.dropBehaviour)
            var dropped = $.ui.ddmanager.drop(this, e); if ((this.options.revert == "invalid" && !dropped) || (this.options.revert == "valid" && dropped) || this.options.revert === true) { var self = this; $(this.helper).animate(this.originalPosition, parseInt(this.options.revert, 10) || 500, function () { self.propagate("stop", e); self.clear(); }); } else { this.propagate("stop", e); this.clear(); }
        return false;
    }, clear: function () { this.helper.removeClass("ui-draggable-dragging"); if (this.options.helper != 'original' && !this.cancelHelperRemoval) this.helper.remove(); this.helper = null; this.cancelHelperRemoval = false; }, plugins: {}, uiHash: function (e) { return { helper: this.helper, position: this.position, absolutePosition: this.positionAbs, options: this.options }; }, propagate: function (n, e) { $.ui.plugin.call(this, n, [e, this.uiHash()]); if (n == "drag") this.positionAbs = this.convertPositionTo("absolute"); return this.element.triggerHandler(n == "drag" ? n : "drag" + n, [e, this.uiHash()], this.options[n]); }, destroy: function () { if (!this.element.data('draggable')) return; this.element.removeData("draggable").unbind(".draggable").removeClass('ui-draggable'); this.mouseDestroy(); } 
    })); $.extend($.ui.draggable, { defaults: { appendTo: "parent", axis: false, cancel: ":input", delay: 0, distance: 1, helper: "original"} }); $.ui.plugin.add("draggable", "cursor", { start: function (e, ui) { var t = $('body'); if (t.css("cursor")) ui.options._cursor = t.css("cursor"); t.css("cursor", ui.options.cursor); }, stop: function (e, ui) { if (ui.options._cursor) $('body').css("cursor", ui.options._cursor); } }); $.ui.plugin.add("draggable", "zIndex", { start: function (e, ui) { var t = $(ui.helper); if (t.css("zIndex")) ui.options._zIndex = t.css("zIndex"); t.css('zIndex', ui.options.zIndex); }, stop: function (e, ui) { if (ui.options._zIndex) $(ui.helper).css('zIndex', ui.options._zIndex); } }); $.ui.plugin.add("draggable", "opacity", { start: function (e, ui) { var t = $(ui.helper); if (t.css("opacity")) ui.options._opacity = t.css("opacity"); t.css('opacity', ui.options.opacity); }, stop: function (e, ui) { if (ui.options._opacity) $(ui.helper).css('opacity', ui.options._opacity); } }); $.ui.plugin.add("draggable", "iframeFix", { start: function (e, ui) { $(ui.options.iframeFix === true ? "iframe" : ui.options.iframeFix).each(function () { $('<div class="ui-draggable-iframeFix" style="background: #fff;"></div>').css({ width: this.offsetWidth + "px", height: this.offsetHeight + "px", position: "absolute", opacity: "0.001", zIndex: 1000 }).css($(this).offset()).appendTo("body"); }); }, stop: function (e, ui) { $("div.DragDropIframeFix").each(function () { this.parentNode.removeChild(this); }); } }); $.ui.plugin.add("draggable", "scroll", { start: function (e, ui) { var o = ui.options; var i = $(this).data("draggable"); o.scrollSensitivity = o.scrollSensitivity || 20; o.scrollSpeed = o.scrollSpeed || 20; i.overflowY = function (el) { do { if (/auto|scroll/.test(el.css('overflow')) || (/auto|scroll/).test(el.css('overflow-y'))) return el; el = el.parent(); } while (el[0].parentNode); return $(document); } (this); i.overflowX = function (el) { do { if (/auto|scroll/.test(el.css('overflow')) || (/auto|scroll/).test(el.css('overflow-x'))) return el; el = el.parent(); } while (el[0].parentNode); return $(document); } (this); if (i.overflowY[0] != document && i.overflowY[0].tagName != 'HTML') i.overflowYOffset = i.overflowY.offset(); if (i.overflowX[0] != document && i.overflowX[0].tagName != 'HTML') i.overflowXOffset = i.overflowX.offset(); }, drag: function (e, ui) {
        var o = ui.options; var i = $(this).data("draggable"); if (i.overflowY[0] != document && i.overflowY[0].tagName != 'HTML') {
            if ((i.overflowYOffset.top + i.overflowY[0].offsetHeight) - e.pageY < o.scrollSensitivity)
                i.overflowY[0].scrollTop = i.overflowY[0].scrollTop + o.scrollSpeed; if (e.pageY - i.overflowYOffset.top < o.scrollSensitivity)
                i.overflowY[0].scrollTop = i.overflowY[0].scrollTop - o.scrollSpeed;
        } else {
            if (e.pageY - $(document).scrollTop() < o.scrollSensitivity)
                $(document).scrollTop($(document).scrollTop() - o.scrollSpeed); if ($(window).height() - (e.pageY - $(document).scrollTop()) < o.scrollSensitivity)
                $(document).scrollTop($(document).scrollTop() + o.scrollSpeed);
        }
        if (i.overflowX[0] != document && i.overflowX[0].tagName != 'HTML') {
            if ((i.overflowXOffset.left + i.overflowX[0].offsetWidth) - e.pageX < o.scrollSensitivity)
                i.overflowX[0].scrollLeft = i.overflowX[0].scrollLeft + o.scrollSpeed; if (e.pageX - i.overflowXOffset.left < o.scrollSensitivity)
                i.overflowX[0].scrollLeft = i.overflowX[0].scrollLeft - o.scrollSpeed;
        } else {
            if (e.pageX - $(document).scrollLeft() < o.scrollSensitivity)
                $(document).scrollLeft($(document).scrollLeft() - o.scrollSpeed); if ($(window).width() - (e.pageX - $(document).scrollLeft()) < o.scrollSensitivity)
                $(document).scrollLeft($(document).scrollLeft() + o.scrollSpeed);
        } 
    } 
    }); $.ui.plugin.add("draggable", "snap", { start: function (e, ui) { var inst = $(this).data("draggable"); inst.snapElements = []; $(ui.options.snap === true ? '.ui-draggable' : ui.options.snap).each(function () { var $t = $(this); var $o = $t.offset(); if (this != inst.element[0]) inst.snapElements.push({ item: this, width: $t.outerWidth(), height: $t.outerHeight(), top: $o.top, left: $o.left }); }); }, drag: function (e, ui) {
        var inst = $(this).data("draggable"); var d = ui.options.snapTolerance || 20; var x1 = ui.absolutePosition.left, x2 = x1 + inst.helperProportions.width, y1 = ui.absolutePosition.top, y2 = y1 + inst.helperProportions.height; for (var i = inst.snapElements.length - 1; i >= 0; i--) {
            var l = inst.snapElements[i].left, r = l + inst.snapElements[i].width, t = inst.snapElements[i].top, b = t + inst.snapElements[i].height; if (!((l - d < x1 && x1 < r + d && t - d < y1 && y1 < b + d) || (l - d < x1 && x1 < r + d && t - d < y2 && y2 < b + d) || (l - d < x2 && x2 < r + d && t - d < y1 && y1 < b + d) || (l - d < x2 && x2 < r + d && t - d < y2 && y2 < b + d))) continue; if (ui.options.snapMode != 'inner') { var ts = Math.abs(t - y2) <= 20; var bs = Math.abs(b - y1) <= 20; var ls = Math.abs(l - x2) <= 20; var rs = Math.abs(r - x1) <= 20; if (ts) ui.position.top = inst.convertPositionTo("relative", { top: t - inst.helperProportions.height, left: 0 }).top; if (bs) ui.position.top = inst.convertPositionTo("relative", { top: b, left: 0 }).top; if (ls) ui.position.left = inst.convertPositionTo("relative", { top: 0, left: l - inst.helperProportions.width }).left; if (rs) ui.position.left = inst.convertPositionTo("relative", { top: 0, left: r }).left; }
            if (ui.options.snapMode != 'outer') { var ts = Math.abs(t - y1) <= 20; var bs = Math.abs(b - y2) <= 20; var ls = Math.abs(l - x1) <= 20; var rs = Math.abs(r - x2) <= 20; if (ts) ui.position.top = inst.convertPositionTo("relative", { top: t, left: 0 }).top; if (bs) ui.position.top = inst.convertPositionTo("relative", { top: b - inst.helperProportions.height, left: 0 }).top; if (ls) ui.position.left = inst.convertPositionTo("relative", { top: 0, left: l }).left; if (rs) ui.position.left = inst.convertPositionTo("relative", { top: 0, left: r - inst.helperProportions.width }).left; } 
        };
    } 
    }); $.ui.plugin.add("draggable", "connectToSortable", { start: function (e, ui) { var inst = $(this).data("draggable"); inst.sortables = []; $(ui.options.connectToSortable).each(function () { if ($.data(this, 'sortable')) { var sortable = $.data(this, 'sortable'); inst.sortables.push({ instance: sortable, shouldRevert: sortable.options.revert }); sortable.refreshItems(); sortable.propagate("activate", e, inst); } }); }, stop: function (e, ui) { var inst = $(this).data("draggable"); $.each(inst.sortables, function () { if (this.instance.isOver) { this.instance.isOver = 0; inst.cancelHelperRemoval = true; this.instance.cancelHelperRemoval = false; if (this.shouldRevert) this.instance.options.revert = true; this.instance.mouseStop(e); this.instance.element.triggerHandler("sortreceive", [e, $.extend(this.instance.ui(), { sender: inst.element })], this.instance.options["receive"]); this.instance.options.helper = this.instance.options._helper; } else { this.instance.propagate("deactivate", e, inst); } }); }, drag: function (e, ui) {
        var inst = $(this).data("draggable"), self = this; var checkPos = function (o) { var l = o.left, r = l + o.width, t = o.top, b = t + o.height; return (l < (this.positionAbs.left + this.offset.click.left) && (this.positionAbs.left + this.offset.click.left) < r && t < (this.positionAbs.top + this.offset.click.top) && (this.positionAbs.top + this.offset.click.top) < b); }; $.each(inst.sortables, function (i) {
            if (checkPos.call(inst, this.instance.containerCache)) {
                if (!this.instance.isOver) { this.instance.isOver = 1; this.instance.currentItem = $(self).clone().appendTo(this.instance.element).data("sortable-item", true); this.instance.options._helper = this.instance.options.helper; this.instance.options.helper = function () { return ui.helper[0]; }; e.target = this.instance.currentItem[0]; this.instance.mouseCapture(e, true); this.instance.mouseStart(e, true, true); this.instance.offset.click.top = inst.offset.click.top; this.instance.offset.click.left = inst.offset.click.left; this.instance.offset.parent.left -= inst.offset.parent.left - this.instance.offset.parent.left; this.instance.offset.parent.top -= inst.offset.parent.top - this.instance.offset.parent.top; inst.propagate("toSortable", e); }
                if (this.instance.currentItem) this.instance.mouseDrag(e);
            } else { if (this.instance.isOver) { this.instance.isOver = 0; this.instance.cancelHelperRemoval = true; this.instance.options.revert = false; this.instance.mouseStop(e, true); this.instance.options.helper = this.instance.options._helper; this.instance.currentItem.remove(); if (this.instance.placeholder) this.instance.placeholder.remove(); inst.propagate("fromSortable", e); } };
        });
    } 
    }); $.ui.plugin.add("draggable", "stack", { start: function (e, ui) { var group = $.makeArray($(ui.options.stack.group)).sort(function (a, b) { return (parseInt($(a).css("zIndex"), 10) || ui.options.stack.min) - (parseInt($(b).css("zIndex"), 10) || ui.options.stack.min); }); $(group).each(function (i) { this.style.zIndex = ui.options.stack.min + i; }); this[0].style.zIndex = ui.options.stack.min + group.length; } });
})(jQuery); (function ($) {
    $.widget("ui.droppable", { init: function () { this.element.addClass("ui-droppable"); this.isover = 0; this.isout = 1; var o = this.options, accept = o.accept; o = $.extend(o, { accept: o.accept && o.accept.constructor == Function ? o.accept : function (d) { return $(d).is(accept); } }); this.proportions = { width: this.element[0].offsetWidth, height: this.element[0].offsetHeight }; $.ui.ddmanager.droppables.push(this); }, plugins: {}, ui: function (c) { return { draggable: (c.currentItem || c.element), helper: c.helper, position: c.position, absolutePosition: c.positionAbs, options: this.options, element: this.element }; }, destroy: function () {
        var drop = $.ui.ddmanager.droppables; for (var i = 0; i < drop.length; i++)
            if (drop[i] == this)
                drop.splice(i, 1); this.element.removeClass("ui-droppable ui-droppable-disabled").removeData("droppable").unbind(".droppable");
    }, over: function (e) { var draggable = $.ui.ddmanager.current; if (!draggable || (draggable.currentItem || draggable.element)[0] == this.element[0]) return; if (this.options.accept.call(this.element, (draggable.currentItem || draggable.element))) { $.ui.plugin.call(this, 'over', [e, this.ui(draggable)]); this.element.triggerHandler("dropover", [e, this.ui(draggable)], this.options.over); } }, out: function (e) { var draggable = $.ui.ddmanager.current; if (!draggable || (draggable.currentItem || draggable.element)[0] == this.element[0]) return; if (this.options.accept.call(this.element, (draggable.currentItem || draggable.element))) { $.ui.plugin.call(this, 'out', [e, this.ui(draggable)]); this.element.triggerHandler("dropout", [e, this.ui(draggable)], this.options.out); } }, drop: function (e, custom) {
        var draggable = custom || $.ui.ddmanager.current; if (!draggable || (draggable.currentItem || draggable.element)[0] == this.element[0]) return false; var childrenIntersection = false; this.element.find(".ui-droppable").not(".ui-draggable-dragging").each(function () { var inst = $.data(this, 'droppable'); if (inst.options.greedy && $.ui.intersect(draggable, $.extend(inst, { offset: inst.element.offset() }), inst.options.tolerance)) { childrenIntersection = true; return false; } }); if (childrenIntersection) return false; if (this.options.accept.call(this.element, (draggable.currentItem || draggable.element))) { $.ui.plugin.call(this, 'drop', [e, this.ui(draggable)]); this.element.triggerHandler("drop", [e, this.ui(draggable)], this.options.drop); return true; }
        return false;
    }, activate: function (e) { var draggable = $.ui.ddmanager.current; $.ui.plugin.call(this, 'activate', [e, this.ui(draggable)]); if (draggable) this.element.triggerHandler("dropactivate", [e, this.ui(draggable)], this.options.activate); }, deactivate: function (e) { var draggable = $.ui.ddmanager.current; $.ui.plugin.call(this, 'deactivate', [e, this.ui(draggable)]); if (draggable) this.element.triggerHandler("dropdeactivate", [e, this.ui(draggable)], this.options.deactivate); } 
    }); $.extend($.ui.droppable, { defaults: { disabled: false, tolerance: 'intersect'} }); $.ui.intersect = function (draggable, droppable, toleranceMode) { if (!droppable.offset) return false; var x1 = (draggable.positionAbs || draggable.position.absolute).left, x2 = x1 + draggable.helperProportions.width, y1 = (draggable.positionAbs || draggable.position.absolute).top, y2 = y1 + draggable.helperProportions.height; var l = droppable.offset.left, r = l + droppable.proportions.width, t = droppable.offset.top, b = t + droppable.proportions.height; switch (toleranceMode) { case 'fit': return (l < x1 && x2 < r && t < y1 && y2 < b); break; case 'intersect': return (l < x1 + (draggable.helperProportions.width / 2) && x2 - (draggable.helperProportions.width / 2) < r && t < y1 + (draggable.helperProportions.height / 2) && y2 - (draggable.helperProportions.height / 2) < b); break; case 'pointer': return (l < ((draggable.positionAbs || draggable.position.absolute).left + (draggable.clickOffset || draggable.offset.click).left) && ((draggable.positionAbs || draggable.position.absolute).left + (draggable.clickOffset || draggable.offset.click).left) < r && t < ((draggable.positionAbs || draggable.position.absolute).top + (draggable.clickOffset || draggable.offset.click).top) && ((draggable.positionAbs || draggable.position.absolute).top + (draggable.clickOffset || draggable.offset.click).top) < b); break; case 'touch': return ((y1 >= t && y1 <= b) || (y2 >= t && y2 <= b) || (y1 < t && y2 > b)) && ((x1 >= l && x1 <= r) || (x2 >= l && x2 <= r) || (x1 < l && x2 > r)); break; default: return false; break; } }; $.ui.ddmanager = { current: null, droppables: [], prepareOffsets: function (t, e) { var m = $.ui.ddmanager.droppables; var type = e ? e.type : null; for (var i = 0; i < m.length; i++) { if (m[i].options.disabled || (t && !m[i].options.accept.call(m[i].element, (t.currentItem || t.element)))) continue; m[i].visible = m[i].element.css("display") != "none"; if (!m[i].visible) continue; m[i].offset = m[i].element.offset(); m[i].proportions = { width: m[i].element[0].offsetWidth, height: m[i].element[0].offsetHeight }; if (type == "dragstart" || type == "sortactivate") m[i].activate.call(m[i], e); } }, drop: function (draggable, e) {
        var dropped = false; $.each($.ui.ddmanager.droppables, function () {
            if (!this.options) return; if (!this.options.disabled && this.visible && $.ui.intersect(draggable, this, this.options.tolerance))
                dropped = this.drop.call(this, e); if (!this.options.disabled && this.visible && this.options.accept.call(this.element, (draggable.currentItem || draggable.element))) { this.isout = 1; this.isover = 0; this.deactivate.call(this, e); } 
        }); return dropped;
    }, drag: function (draggable, e) {
        if (draggable.options.refreshPositions) $.ui.ddmanager.prepareOffsets(draggable, e); $.each($.ui.ddmanager.droppables, function () {
            if (this.options.disabled || this.greedyChild || !this.visible) return; var intersects = $.ui.intersect(draggable, this, this.options.tolerance); var c = !intersects && this.isover == 1 ? 'isout' : (intersects && this.isover == 0 ? 'isover' : null); if (!c) return; var parentInstance; if (this.options.greedy) { var parent = this.element.parents('.ui-droppable:eq(0)'); if (parent.length) { parentInstance = $.data(parent[0], 'droppable'); parentInstance.greedyChild = (c == 'isover' ? 1 : 0); } }
            if (parentInstance && c == 'isover') { parentInstance['isover'] = 0; parentInstance['isout'] = 1; parentInstance.out.call(parentInstance, e); }
            this[c] = 1; this[c == 'isout' ? 'isover' : 'isout'] = 0; this[c == "isover" ? "over" : "out"].call(this, e); if (parentInstance && c == 'isout') { parentInstance['isout'] = 0; parentInstance['isover'] = 1; parentInstance.over.call(parentInstance, e); } 
        });
    } 
    }; $.ui.plugin.add("droppable", "activeClass", { activate: function (e, ui) { $(this).addClass(ui.options.activeClass); }, deactivate: function (e, ui) { $(this).removeClass(ui.options.activeClass); }, drop: function (e, ui) { $(this).removeClass(ui.options.activeClass); } }); $.ui.plugin.add("droppable", "hoverClass", { over: function (e, ui) { $(this).addClass(ui.options.hoverClass); }, out: function (e, ui) { $(this).removeClass(ui.options.hoverClass); }, drop: function (e, ui) { $(this).removeClass(ui.options.hoverClass); } });
})(jQuery); (function ($) {
    $.widget("ui.resizable", $.extend({}, $.ui.mouse, { init: function () {
        var self = this, o = this.options; var elpos = this.element.css('position'); this.originalElement = this.element; this.element.addClass("ui-resizable").css({ position: /static/.test(elpos) ? 'relative' : elpos }); $.extend(o, { _aspectRatio: !!(o.aspectRatio), helper: o.helper || o.ghost || o.animate ? o.helper || 'proxy' : null, knobHandles: o.knobHandles === true ? 'ui-resizable-knob-handle' : o.knobHandles }); var aBorder = '1px solid #DEDEDE'; o.defaultTheme = { 'ui-resizable': { display: 'block' }, 'ui-resizable-handle': { position: 'absolute', background: '#F2F2F2', fontSize: '0.1px' }, 'ui-resizable-n': { cursor: 'n-resize', height: '4px', left: '0px', right: '0px', borderTop: aBorder }, 'ui-resizable-s': { cursor: 's-resize', height: '4px', left: '0px', right: '0px', borderBottom: aBorder }, 'ui-resizable-e': { cursor: 'e-resize', width: '4px', top: '0px', bottom: '0px', borderRight: aBorder }, 'ui-resizable-w': { cursor: 'w-resize', width: '4px', top: '0px', bottom: '0px', borderLeft: aBorder }, 'ui-resizable-se': { cursor: 'se-resize', width: '4px', height: '4px', borderRight: aBorder, borderBottom: aBorder }, 'ui-resizable-sw': { cursor: 'sw-resize', width: '4px', height: '4px', borderBottom: aBorder, borderLeft: aBorder }, 'ui-resizable-ne': { cursor: 'ne-resize', width: '4px', height: '4px', borderRight: aBorder, borderTop: aBorder }, 'ui-resizable-nw': { cursor: 'nw-resize', width: '4px', height: '4px', borderLeft: aBorder, borderTop: aBorder} }; o.knobTheme = { 'ui-resizable-handle': { background: '#F2F2F2', border: '1px solid #808080', height: '8px', width: '8px' }, 'ui-resizable-n': { cursor: 'n-resize', top: '0px', left: '45%' }, 'ui-resizable-s': { cursor: 's-resize', bottom: '0px', left: '45%' }, 'ui-resizable-e': { cursor: 'e-resize', right: '0px', top: '45%' }, 'ui-resizable-w': { cursor: 'w-resize', left: '0px', top: '45%' }, 'ui-resizable-se': { cursor: 'se-resize', right: '0px', bottom: '0px' }, 'ui-resizable-sw': { cursor: 'sw-resize', left: '0px', bottom: '0px' }, 'ui-resizable-nw': { cursor: 'nw-resize', left: '0px', top: '0px' }, 'ui-resizable-ne': { cursor: 'ne-resize', right: '0px', top: '0px'} }; o._nodeName = this.element[0].nodeName; if (o._nodeName.match(/canvas|textarea|input|select|button|img/i)) {
            var el = this.element; if (/relative/.test(el.css('position')) && $.browser.opera)
                el.css({ position: 'relative', top: 'auto', left: 'auto' }); el.wrap($('<div class="ui-wrapper" style="overflow: hidden;"></div>').css({ position: el.css('position'), width: el.outerWidth(), height: el.outerHeight(), top: el.css('top'), left: el.css('left') })); var oel = this.element; this.element = this.element.parent(); this.element.data('resizable', this); this.element.css({ marginLeft: oel.css("marginLeft"), marginTop: oel.css("marginTop"), marginRight: oel.css("marginRight"), marginBottom: oel.css("marginBottom") }); oel.css({ marginLeft: 0, marginTop: 0, marginRight: 0, marginBottom: 0 }); if ($.browser.safari && o.preventDefault) oel.css('resize', 'none'); o.proportionallyResize = oel.css({ position: 'static', zoom: 1, display: 'block' }); this.element.css({ margin: oel.css('margin') }); this._proportionallyResize();
        }
        if (!o.handles) o.handles = !$('.ui-resizable-handle', this.element).length ? "e,s,se" : { n: '.ui-resizable-n', e: '.ui-resizable-e', s: '.ui-resizable-s', w: '.ui-resizable-w', se: '.ui-resizable-se', sw: '.ui-resizable-sw', ne: '.ui-resizable-ne', nw: '.ui-resizable-nw' }; if (o.handles.constructor == String) {
            o.zIndex = o.zIndex || 1000; if (o.handles == 'all') o.handles = 'n,e,s,w,se,sw,ne,nw'; var n = o.handles.split(","); o.handles = {}; var insertionsDefault = { handle: 'position: absolute; display: none; overflow:hidden;', n: 'top: 0pt; width:100%;', e: 'right: 0pt; height:100%;', s: 'bottom: 0pt; width:100%;', w: 'left: 0pt; height:100%;', se: 'bottom: 0pt; right: 0px;', sw: 'bottom: 0pt; left: 0px;', ne: 'top: 0pt; right: 0px;', nw: 'top: 0pt; left: 0px;' }; for (var i = 0; i < n.length; i++) { var handle = $.trim(n[i]), dt = o.defaultTheme, hname = 'ui-resizable-' + handle, loadDefault = !$.ui.css(hname) && !o.knobHandles, userKnobClass = $.ui.css('ui-resizable-knob-handle'), allDefTheme = $.extend(dt[hname], dt['ui-resizable-handle']), allKnobTheme = $.extend(o.knobTheme[hname], !userKnobClass ? o.knobTheme['ui-resizable-handle'] : {}); var applyZIndex = /sw|se|ne|nw/.test(handle) ? { zIndex: ++o.zIndex} : {}; var defCss = (loadDefault ? insertionsDefault[handle] : ''), axis = $(['<div class="ui-resizable-handle ', hname, '" style="', defCss, insertionsDefault.handle, '"></div>'].join('')).css(applyZIndex); o.handles[handle] = '.ui-resizable-' + handle; this.element.append(axis.css(loadDefault ? allDefTheme : {}).css(o.knobHandles ? allKnobTheme : {}).addClass(o.knobHandles ? 'ui-resizable-knob-handle' : '').addClass(o.knobHandles)); }
            if (o.knobHandles) this.element.addClass('ui-resizable-knob').css(!$.ui.css('ui-resizable-knob') ? {} : {});
        }
        this._renderAxis = function (target) {
            target = target || this.element; for (var i in o.handles) {
                if (o.handles[i].constructor == String)
                    o.handles[i] = $(o.handles[i], this.element).show(); if (o.transparent)
                    o.handles[i].css({ opacity: 0 }); if (this.element.is('.ui-wrapper') && o._nodeName.match(/textarea|input|select|button/i)) {
                    var axis = $(o.handles[i], this.element), padWrapper = 0; padWrapper = /sw|ne|nw|se|n|s/.test(i) ? axis.outerHeight() : axis.outerWidth(); var padPos = ['padding', /ne|nw|n/.test(i) ? 'Top' : /se|sw|s/.test(i) ? 'Bottom' : /^e$/.test(i) ? 'Right' : 'Left'].join(""); if (!o.transparent)
                        target.css(padPos, padWrapper); this._proportionallyResize();
                }
                if (!$(o.handles[i]).length) continue;
            } 
        }; this._renderAxis(this.element); o._handles = $('.ui-resizable-handle', self.element); if (o.disableSelection)
            o._handles.each(function (i, e) { $.ui.disableSelection(e); }); o._handles.mouseover(function () {
                if (!o.resizing) {
                    if (this.className)
                        var axis = this.className.match(/ui-resizable-(se|sw|ne|nw|n|e|s|w)/i); self.axis = o.axis = axis && axis[1] ? axis[1] : 'se';
                } 
            }); if (o.autoHide) { o._handles.hide(); $(self.element).addClass("ui-resizable-autohide").hover(function () { $(this).removeClass("ui-resizable-autohide"); o._handles.show(); }, function () { if (!o.resizing) { $(this).addClass("ui-resizable-autohide"); o._handles.hide(); } }); }
        this.mouseInit();
    }, plugins: {}, ui: function () { return { originalElement: this.originalElement, element: this.element, helper: this.helper, position: this.position, size: this.size, options: this.options, originalSize: this.originalSize, originalPosition: this.originalPosition }; }, propagate: function (n, e) { $.ui.plugin.call(this, n, [e, this.ui()]); if (n != "resize") this.element.triggerHandler(["resize", n].join(""), [e, this.ui()], this.options[n]); }, destroy: function () { var el = this.element, wrapped = el.children(".ui-resizable").get(0); this.mouseDestroy(); var _destroy = function (exp) { $(exp).removeClass("ui-resizable ui-resizable-disabled").removeData("resizable").unbind(".resizable").find('.ui-resizable-handle').remove(); }; _destroy(el); if (el.is('.ui-wrapper') && wrapped) { el.parent().append($(wrapped).css({ position: el.css('position'), width: el.outerWidth(), height: el.outerHeight(), top: el.css('top'), left: el.css('left') })).end().remove(); _destroy(wrapped); } }, mouseStart: function (e) {
        if (this.options.disabled) return false; var handle = false; for (var i in this.options.handles) { if ($(this.options.handles[i])[0] == e.target) handle = true; }
        if (!handle) return false; var o = this.options, iniPos = this.element.position(), el = this.element, num = function (v) { return parseInt(v, 10) || 0; }, ie6 = $.browser.msie && $.browser.version < 7; o.resizing = true; o.documentScroll = { top: $(document).scrollTop(), left: $(document).scrollLeft() }; if (el.is('.ui-draggable') || (/absolute/).test(el.css('position'))) { var sOffset = $.browser.msie && !o.containment && (/absolute/).test(el.css('position')) && !(/relative/).test(el.parent().css('position')); var dscrollt = sOffset ? o.documentScroll.top : 0, dscrolll = sOffset ? o.documentScroll.left : 0; el.css({ position: 'absolute', top: (iniPos.top + dscrollt), left: (iniPos.left + dscrolll) }); }
        if ($.browser.opera && /relative/.test(el.css('position')))
            el.css({ position: 'relative', top: 'auto', left: 'auto' }); this._renderProxy(); var curleft = num(this.helper.css('left')), curtop = num(this.helper.css('top')); if (o.containment) { curleft += $(o.containment).scrollLeft() || 0; curtop += $(o.containment).scrollTop() || 0; }
        this.offset = this.helper.offset(); this.position = { left: curleft, top: curtop }; this.size = o.helper || ie6 ? { width: el.outerWidth(), height: el.outerHeight()} : { width: el.width(), height: el.height() }; this.originalSize = o.helper || ie6 ? { width: el.outerWidth(), height: el.outerHeight()} : { width: el.width(), height: el.height() }; this.originalPosition = { left: curleft, top: curtop }; this.sizeDiff = { width: el.outerWidth() - el.width(), height: el.outerHeight() - el.height() }; this.originalMousePosition = { left: e.pageX, top: e.pageY }; o.aspectRatio = (typeof o.aspectRatio == 'number') ? o.aspectRatio : ((this.originalSize.height / this.originalSize.width) || 1); if (o.preserveCursor)
            $('body').css('cursor', this.axis + '-resize'); this.propagate("start", e); return true;
    }, mouseDrag: function (e) {
        var el = this.helper, o = this.options, props = {}, self = this, smp = this.originalMousePosition, a = this.axis; var dx = (e.pageX - smp.left) || 0, dy = (e.pageY - smp.top) || 0; var trigger = this._change[a]; if (!trigger) return false; var data = trigger.apply(this, [e, dx, dy]), ie6 = $.browser.msie && $.browser.version < 7, csdif = this.sizeDiff; if (o._aspectRatio || e.shiftKey)
            data = this._updateRatio(data, e); data = this._respectSize(data, e); this.propagate("resize", e); el.css({ top: this.position.top + "px", left: this.position.left + "px", width: this.size.width + "px", height: this.size.height + "px" }); if (!o.helper && o.proportionallyResize)
            this._proportionallyResize(); this._updateCache(data); this.element.triggerHandler("resize", [e, this.ui()], this.options["resize"]); return false;
    }, mouseStop: function (e) {
        this.options.resizing = false; var o = this.options, num = function (v) { return parseInt(v, 10) || 0; }, self = this; if (o.helper) {
            var pr = o.proportionallyResize, ista = pr && (/textarea/i).test(pr.get(0).nodeName), soffseth = ista && $.ui.hasScroll(pr.get(0), 'left') ? 0 : self.sizeDiff.height, soffsetw = ista ? 0 : self.sizeDiff.width; var s = { width: (self.size.width - soffsetw), height: (self.size.height - soffseth) }, left = (parseInt(self.element.css('left'), 10) + (self.position.left - self.originalPosition.left)) || null, top = (parseInt(self.element.css('top'), 10) + (self.position.top - self.originalPosition.top)) || null; if (!o.animate)
                this.element.css($.extend(s, { top: top, left: left })); if (o.helper && !o.animate) this._proportionallyResize();
        }
        if (o.preserveCursor)
            $('body').css('cursor', 'auto'); this.propagate("stop", e); if (o.helper) this.helper.remove(); return false;
    }, _updateCache: function (data) { var o = this.options; this.offset = this.helper.offset(); if (data.left) this.position.left = data.left; if (data.top) this.position.top = data.top; if (data.height) this.size.height = data.height; if (data.width) this.size.width = data.width; }, _updateRatio: function (data, e) {
        var o = this.options, cpos = this.position, csize = this.size, a = this.axis; if (data.height) data.width = (csize.height / o.aspectRatio); else if (data.width) data.height = (csize.width * o.aspectRatio); if (a == 'sw') { data.left = cpos.left + (csize.width - data.width); data.top = null; }
        if (a == 'nw') { data.top = cpos.top + (csize.height - data.height); data.left = cpos.left + (csize.width - data.width); }
        return data;
    }, _respectSize: function (data, e) { var el = this.helper, o = this.options, pRatio = o._aspectRatio || e.shiftKey, a = this.axis, ismaxw = data.width && o.maxWidth && o.maxWidth < data.width, ismaxh = data.height && o.maxHeight && o.maxHeight < data.height, isminw = data.width && o.minWidth && o.minWidth > data.width, isminh = data.height && o.minHeight && o.minHeight > data.height; if (isminw) data.width = o.minWidth; if (isminh) data.height = o.minHeight; if (ismaxw) data.width = o.maxWidth; if (ismaxh) data.height = o.maxHeight; var dw = this.originalPosition.left + this.originalSize.width, dh = this.position.top + this.size.height; var cw = /sw|nw|w/.test(a), ch = /nw|ne|n/.test(a); if (isminw && cw) data.left = dw - o.minWidth; if (ismaxw && cw) data.left = dw - o.maxWidth; if (isminh && ch) data.top = dh - o.minHeight; if (ismaxh && ch) data.top = dh - o.maxHeight; var isNotwh = !data.width && !data.height; if (isNotwh && !data.left && data.top) data.top = null; else if (isNotwh && !data.top && data.left) data.left = null; return data; }, _proportionallyResize: function () {
        var o = this.options; if (!o.proportionallyResize) return; var prel = o.proportionallyResize, el = this.helper || this.element; if (!o.borderDif) { var b = [prel.css('borderTopWidth'), prel.css('borderRightWidth'), prel.css('borderBottomWidth'), prel.css('borderLeftWidth')], p = [prel.css('paddingTop'), prel.css('paddingRight'), prel.css('paddingBottom'), prel.css('paddingLeft')]; o.borderDif = $.map(b, function (v, i) { var border = parseInt(v, 10) || 0, padding = parseInt(p[i], 10) || 0; return border + padding; }); }
        prel.css({ height: (el.height() - o.borderDif[0] - o.borderDif[2]) + "px", width: (el.width() - o.borderDif[1] - o.borderDif[3]) + "px" });
    }, _renderProxy: function () {
        var el = this.element, o = this.options; this.elementOffset = el.offset(); if (o.helper) {
            this.helper = this.helper || $('<div style="overflow:hidden;"></div>'); var ie6 = $.browser.msie && $.browser.version < 7, ie6offset = (ie6 ? 1 : 0), pxyoffset = (ie6 ? 2 : -1); this.helper.addClass(o.helper).css({ width: el.outerWidth() + pxyoffset, height: el.outerHeight() + pxyoffset, position: 'absolute', left: this.elementOffset.left - ie6offset + 'px', top: this.elementOffset.top - ie6offset + 'px', zIndex: ++o.zIndex }); this.helper.appendTo("body"); if (o.disableSelection)
                $.ui.disableSelection(this.helper.get(0));
        } else { this.helper = el; } 
    }, _change: { e: function (e, dx, dy) { return { width: this.originalSize.width + dx }; }, w: function (e, dx, dy) { var o = this.options, cs = this.originalSize, sp = this.originalPosition; return { left: sp.left + dx, width: cs.width - dx }; }, n: function (e, dx, dy) { var o = this.options, cs = this.originalSize, sp = this.originalPosition; return { top: sp.top + dy, height: cs.height - dy }; }, s: function (e, dx, dy) { return { height: this.originalSize.height + dy }; }, se: function (e, dx, dy) { return $.extend(this._change.s.apply(this, arguments), this._change.e.apply(this, [e, dx, dy])); }, sw: function (e, dx, dy) { return $.extend(this._change.s.apply(this, arguments), this._change.w.apply(this, [e, dx, dy])); }, ne: function (e, dx, dy) { return $.extend(this._change.n.apply(this, arguments), this._change.e.apply(this, [e, dx, dy])); }, nw: function (e, dx, dy) { return $.extend(this._change.n.apply(this, arguments), this._change.w.apply(this, [e, dx, dy])); } }
    })); $.extend($.ui.resizable, { defaults: { cancel: ":input", distance: 1, delay: 0, preventDefault: true, transparent: false, minWidth: 10, minHeight: 10, aspectRatio: false, disableSelection: true, preserveCursor: true, autoHide: false, knobHandles: false} }); $.ui.plugin.add("resizable", "containment", { start: function (e, ui) {
        var o = ui.options, self = $(this).data("resizable"), el = self.element; var oc = o.containment, ce = (oc instanceof $) ? oc.get(0) : (/parent/.test(oc)) ? el.parent().get(0) : oc; if (!ce) return; self.containerElement = $(ce); if (/document/.test(oc) || oc == document) { self.containerOffset = { left: 0, top: 0 }; self.containerPosition = { left: 0, top: 0 }; self.parentData = { element: $(document), left: 0, top: 0, width: $(document).width(), height: $(document).height() || document.body.parentNode.scrollHeight }; }
        else { self.containerOffset = $(ce).offset(); self.containerPosition = $(ce).position(); self.containerSize = { height: $(ce).innerHeight(), width: $(ce).innerWidth() }; var co = self.containerOffset, ch = self.containerSize.height, cw = self.containerSize.width, width = ($.ui.hasScroll(ce, "left") ? ce.scrollWidth : cw), height = ($.ui.hasScroll(ce) ? ce.scrollHeight : ch); self.parentData = { element: ce, left: co.left, top: co.top, width: width, height: height }; } 
    }, resize: function (e, ui) {
        var o = ui.options, self = $(this).data("resizable"), ps = self.containerSize, co = self.containerOffset, cs = self.size, cp = self.position, pRatio = o._aspectRatio || e.shiftKey, cop = { top: 0, left: 0 }, ce = self.containerElement; if (ce[0] != document && /static/.test(ce.css('position')))
            cop = self.containerPosition; if (cp.left < (o.helper ? co.left : cop.left)) { self.size.width = self.size.width + (o.helper ? (self.position.left - co.left) : (self.position.left - cop.left)); if (pRatio) self.size.height = self.size.width * o.aspectRatio; self.position.left = o.helper ? co.left : cop.left; }
        if (cp.top < (o.helper ? co.top : 0)) { self.size.height = self.size.height + (o.helper ? (self.position.top - co.top) : self.position.top); if (pRatio) self.size.width = self.size.height / o.aspectRatio; self.position.top = o.helper ? co.top : 0; }
        var woset = (o.helper ? self.offset.left - co.left : (self.position.left - cop.left)) + self.sizeDiff.width, hoset = (o.helper ? self.offset.top - co.top : self.position.top) + self.sizeDiff.height; if (woset + self.size.width >= self.parentData.width) { self.size.width = self.parentData.width - woset; if (pRatio) self.size.height = self.size.width * o.aspectRatio; }
        if (hoset + self.size.height >= self.parentData.height) { self.size.height = self.parentData.height - hoset; if (pRatio) self.size.width = self.size.height / o.aspectRatio; } 
    }, stop: function (e, ui) {
        var o = ui.options, self = $(this).data("resizable"), cp = self.position, co = self.containerOffset, cop = self.containerPosition, ce = self.containerElement; var helper = $(self.helper), ho = helper.offset(), w = helper.innerWidth(), h = helper.innerHeight(); if (o.helper && !o.animate && /relative/.test(ce.css('position')))
            $(this).css({ left: (ho.left - co.left), top: (ho.top - co.top), width: w, height: h }); if (o.helper && !o.animate && /static/.test(ce.css('position')))
            $(this).css({ left: cop.left + (ho.left - co.left), top: cop.top + (ho.top - co.top), width: w, height: h });
    } 
    }); $.ui.plugin.add("resizable", "grid", { resize: function (e, ui) {
        var o = ui.options, self = $(this).data("resizable"), cs = self.size, os = self.originalSize, op = self.originalPosition, a = self.axis, ratio = o._aspectRatio || e.shiftKey; o.grid = typeof o.grid == "number" ? [o.grid, o.grid] : o.grid; var ox = Math.round((cs.width - os.width) / (o.grid[0] || 1)) * (o.grid[0] || 1), oy = Math.round((cs.height - os.height) / (o.grid[1] || 1)) * (o.grid[1] || 1); if (/^(se|s|e)$/.test(a)) { self.size.width = os.width + ox; self.size.height = os.height + oy; }
        else if (/^(ne)$/.test(a)) { self.size.width = os.width + ox; self.size.height = os.height + oy; self.position.top = op.top - oy; }
        else if (/^(sw)$/.test(a)) { self.size.width = os.width + ox; self.size.height = os.height + oy; self.position.left = op.left - ox; }
        else { self.size.width = os.width + ox; self.size.height = os.height + oy; self.position.top = op.top - oy; self.position.left = op.left - ox; } 
    } 
    }); $.ui.plugin.add("resizable", "animate", { stop: function (e, ui) { var o = ui.options, self = $(this).data("resizable"); var pr = o.proportionallyResize, ista = pr && (/textarea/i).test(pr.get(0).nodeName), soffseth = ista && $.ui.hasScroll(pr.get(0), 'left') ? 0 : self.sizeDiff.height, soffsetw = ista ? 0 : self.sizeDiff.width; var style = { width: (self.size.width - soffsetw), height: (self.size.height - soffseth) }, left = (parseInt(self.element.css('left'), 10) + (self.position.left - self.originalPosition.left)) || null, top = (parseInt(self.element.css('top'), 10) + (self.position.top - self.originalPosition.top)) || null; self.element.animate($.extend(style, top && left ? { top: top, left: left} : {}), { duration: o.animateDuration || "slow", easing: o.animateEasing || "swing", step: function () { var data = { width: parseInt(self.element.css('width'), 10), height: parseInt(self.element.css('height'), 10), top: parseInt(self.element.css('top'), 10), left: parseInt(self.element.css('left'), 10) }; if (pr) pr.css({ width: data.width, height: data.height }); self._updateCache(data); self.propagate("animate", e); } }); } }); $.ui.plugin.add("resizable", "ghost", { start: function (e, ui) { var o = ui.options, self = $(this).data("resizable"), pr = o.proportionallyResize, cs = self.size; if (!pr) self.ghost = self.element.clone(); else self.ghost = pr.clone(); self.ghost.css({ opacity: .25, display: 'block', position: 'relative', height: cs.height, width: cs.width, margin: 0, left: 0, top: 0 }).addClass('ui-resizable-ghost').addClass(typeof o.ghost == 'string' ? o.ghost : ''); self.ghost.appendTo(self.helper); }, resize: function (e, ui) { var o = ui.options, self = $(this).data("resizable"), pr = o.proportionallyResize; if (self.ghost) self.ghost.css({ position: 'relative', height: self.size.height, width: self.size.width }); }, stop: function (e, ui) { var o = ui.options, self = $(this).data("resizable"), pr = o.proportionallyResize; if (self.ghost && self.helper) self.helper.get(0).removeChild(self.ghost.get(0)); } }); $.ui.plugin.add("resizable", "alsoResize", { start: function (e, ui) {
        var o = ui.options, self = $(this).data("resizable"), _store = function (exp) { $(exp).each(function () { $(this).data("resizable-alsoresize", { width: parseInt($(this).width(), 10), height: parseInt($(this).height(), 10), left: parseInt($(this).css('left'), 10), top: parseInt($(this).css('top'), 10) }); }); }; if (typeof (o.alsoResize) == 'object') {
            if (o.alsoResize.length) { o.alsoResize = o.alsoResize[0]; _store(o.alsoResize); }
            else { $.each(o.alsoResize, function (exp, c) { _store(exp); }); } 
        } else { _store(o.alsoResize); } 
    }, resize: function (e, ui) {
        var o = ui.options, self = $(this).data("resizable"), os = self.originalSize, op = self.originalPosition; var delta = { height: (self.size.height - os.height) || 0, width: (self.size.width - os.width) || 0, top: (self.position.top - op.top) || 0, left: (self.position.left - op.left) || 0 }, _alsoResize = function (exp, c) {
            $(exp).each(function () {
                var start = $(this).data("resizable-alsoresize"), style = {}, css = c && c.length ? c : ['width', 'height', 'top', 'left']; $.each(css || ['width', 'height', 'top', 'left'], function (i, prop) {
                    var sum = (start[prop] || 0) + (delta[prop] || 0); if (sum && sum >= 0)
                        style[prop] = sum || null;
                }); $(this).css(style);
            });
        }; if (typeof (o.alsoResize) == 'object') { $.each(o.alsoResize, function (exp, c) { _alsoResize(exp, c); }); } else { _alsoResize(o.alsoResize); } 
    }, stop: function (e, ui) { $(this).removeData("resizable-alsoresize-start"); } 
    });
})(jQuery); (function ($) {
    $.widget("ui.selectable", $.extend({}, $.ui.mouse, { init: function () { var self = this; this.element.addClass("ui-selectable"); this.dragged = false; var selectees; this.refresh = function () { selectees = $(self.options.filter, self.element[0]); selectees.each(function () { var $this = $(this); var pos = $this.offset(); $.data(this, "selectable-item", { element: this, $element: $this, left: pos.left, top: pos.top, right: pos.left + $this.width(), bottom: pos.top + $this.height(), startselected: false, selected: $this.hasClass('ui-selected'), selecting: $this.hasClass('ui-selecting'), unselecting: $this.hasClass('ui-unselecting') }); }); }; this.refresh(); this.selectees = selectees.addClass("ui-selectee"); this.mouseInit(); this.helper = $(document.createElement('div')).css({ border: '1px dotted black' }); }, toggle: function () { if (this.options.disabled) { this.enable(); } else { this.disable(); } }, destroy: function () { this.element.removeClass("ui-selectable ui-selectable-disabled").removeData("selectable").unbind(".selectable"); this.mouseDestroy(); }, mouseStart: function (e) {
        var self = this; this.opos = [e.pageX, e.pageY]; if (this.options.disabled)
            return; var options = this.options; this.selectees = $(options.filter, this.element[0]); this.element.triggerHandler("selectablestart", [e, { "selectable": this.element[0], "options": options}], options.start); $('body').append(this.helper); this.helper.css({ "z-index": 100, "position": "absolute", "left": e.clientX, "top": e.clientY, "width": 0, "height": 0 }); if (options.autoRefresh) { this.refresh(); }
        this.selectees.filter('.ui-selected').each(function () { var selectee = $.data(this, "selectable-item"); selectee.startselected = true; if (!e.ctrlKey) { selectee.$element.removeClass('ui-selected'); selectee.selected = false; selectee.$element.addClass('ui-unselecting'); selectee.unselecting = true; self.element.triggerHandler("selectableunselecting", [e, { selectable: self.element[0], unselecting: selectee.element, options: options}], options.unselecting); } }); var isSelectee = false; $(e.target).parents().andSelf().each(function () { if ($.data(this, "selectable-item")) isSelectee = true; }); return this.options.keyboard ? !isSelectee : true;
    }, mouseDrag: function (e) {
        var self = this; this.dragged = true; if (this.options.disabled)
            return; var options = this.options; var x1 = this.opos[0], y1 = this.opos[1], x2 = e.pageX, y2 = e.pageY; if (x1 > x2) { var tmp = x2; x2 = x1; x1 = tmp; }
        if (y1 > y2) { var tmp = y2; y2 = y1; y1 = tmp; }
        this.helper.css({ left: x1, top: y1, width: x2 - x1, height: y2 - y1 }); this.selectees.each(function () {
            var selectee = $.data(this, "selectable-item"); if (!selectee || selectee.element == self.element[0])
                return; var hit = false; if (options.tolerance == 'touch') { hit = (!(selectee.left > x2 || selectee.right < x1 || selectee.top > y2 || selectee.bottom < y1)); } else if (options.tolerance == 'fit') { hit = (selectee.left > x1 && selectee.right < x2 && selectee.top > y1 && selectee.bottom < y2); }
            if (hit) {
                if (selectee.selected) { selectee.$element.removeClass('ui-selected'); selectee.selected = false; }
                if (selectee.unselecting) { selectee.$element.removeClass('ui-unselecting'); selectee.unselecting = false; }
                if (!selectee.selecting) { selectee.$element.addClass('ui-selecting'); selectee.selecting = true; self.element.triggerHandler("selectableselecting", [e, { selectable: self.element[0], selecting: selectee.element, options: options}], options.selecting); } 
            } else {
                if (selectee.selecting) {
                    if (e.ctrlKey && selectee.startselected) { selectee.$element.removeClass('ui-selecting'); selectee.selecting = false; selectee.$element.addClass('ui-selected'); selectee.selected = true; } else {
                        selectee.$element.removeClass('ui-selecting'); selectee.selecting = false; if (selectee.startselected) { selectee.$element.addClass('ui-unselecting'); selectee.unselecting = true; }
                        self.element.triggerHandler("selectableunselecting", [e, { selectable: self.element[0], unselecting: selectee.element, options: options}], options.unselecting);
                    } 
                }
                if (selectee.selected) { if (!e.ctrlKey && !selectee.startselected) { selectee.$element.removeClass('ui-selected'); selectee.selected = false; selectee.$element.addClass('ui-unselecting'); selectee.unselecting = true; self.element.triggerHandler("selectableunselecting", [e, { selectable: self.element[0], unselecting: selectee.element, options: options}], options.unselecting); } } 
            } 
        }); return false;
    }, mouseStop: function (e) { var self = this; this.dragged = false; var options = this.options; $('.ui-unselecting', this.element[0]).each(function () { var selectee = $.data(this, "selectable-item"); selectee.$element.removeClass('ui-unselecting'); selectee.unselecting = false; selectee.startselected = false; self.element.triggerHandler("selectableunselected", [e, { selectable: self.element[0], unselected: selectee.element, options: options}], options.unselected); }); $('.ui-selecting', this.element[0]).each(function () { var selectee = $.data(this, "selectable-item"); selectee.$element.removeClass('ui-selecting').addClass('ui-selected'); selectee.selecting = false; selectee.selected = true; selectee.startselected = true; self.element.triggerHandler("selectableselected", [e, { selectable: self.element[0], selected: selectee.element, options: options}], options.selected); }); this.element.triggerHandler("selectablestop", [e, { selectable: self.element[0], options: this.options}], this.options.stop); this.helper.remove(); return false; } 
    })); $.extend($.ui.selectable, { defaults: { distance: 1, delay: 0, cancel: ":input", appendTo: 'body', autoRefresh: true, filter: '*', tolerance: 'touch'} });
})(jQuery); (function ($) {
    function contains(a, b) {
        var safari2 = $.browser.safari && $.browser.version < 522; if (a.contains && !safari2) { return a.contains(b); }
        if (a.compareDocumentPosition)
            return !!(a.compareDocumentPosition(b) & 16); while (b = b.parentNode)
            if (b == a) return true; return false;
    }; $.widget("ui.sortable", $.extend({}, $.ui.mouse, { init: function () { var o = this.options; this.containerCache = {}; this.element.addClass("ui-sortable"); this.refresh(); this.floating = this.items.length ? (/left|right/).test(this.items[0].item.css('float')) : false; if (!(/(relative|absolute|fixed)/).test(this.element.css('position'))) this.element.css('position', 'relative'); this.offset = this.element.offset(); this.mouseInit(); }, plugins: {}, ui: function (inst) { return { helper: (inst || this)["helper"], placeholder: (inst || this)["placeholder"] || $([]), position: (inst || this)["position"], absolutePosition: (inst || this)["positionAbs"], options: this.options, element: this.element, item: (inst || this)["currentItem"], sender: inst ? inst.element : null }; }, propagate: function (n, e, inst, noPropagation) { $.ui.plugin.call(this, n, [e, this.ui(inst)]); if (!noPropagation) this.element.triggerHandler(n == "sort" ? n : "sort" + n, [e, this.ui(inst)], this.options[n]); }, serialize: function (o) { var items = ($.isFunction(this.options.items) ? this.options.items.call(this.element) : $(this.options.items, this.element)).not('.ui-sortable-helper'); var str = []; o = o || {}; items.each(function () { var res = ($(this).attr(o.attribute || 'id') || '').match(o.expression || (/(.+)[-=_](.+)/)); if (res) str.push((o.key || res[1]) + '[]=' + (o.key && o.expression ? res[1] : res[2])); }); return str.join('&'); }, toArray: function (attr) { var items = ($.isFunction(this.options.items) ? this.options.items.call(this.element) : $(this.options.items, this.element)).not('.ui-sortable-helper'); var ret = []; items.each(function () { ret.push($(this).attr(attr || 'id')); }); return ret; }, intersectsWith: function (item) { var x1 = this.positionAbs.left, x2 = x1 + this.helperProportions.width, y1 = this.positionAbs.top, y2 = y1 + this.helperProportions.height; var l = item.left, r = l + item.width, t = item.top, b = t + item.height; if (this.options.tolerance == "pointer" || this.options.forcePointerForContainers || (this.options.tolerance == "guess" && this.helperProportions[this.floating ? 'width' : 'height'] > item[this.floating ? 'width' : 'height'])) { return (y1 + this.offset.click.top > t && y1 + this.offset.click.top < b && x1 + this.offset.click.left > l && x1 + this.offset.click.left < r); } else { return (l < x1 + (this.helperProportions.width / 2) && x2 - (this.helperProportions.width / 2) < r && t < y1 + (this.helperProportions.height / 2) && y2 - (this.helperProportions.height / 2) < b); } }, intersectsWithEdge: function (item) {
        var x1 = this.positionAbs.left, x2 = x1 + this.helperProportions.width, y1 = this.positionAbs.top, y2 = y1 + this.helperProportions.height; var l = item.left, r = l + item.width, t = item.top, b = t + item.height; if (this.options.tolerance == "pointer" || (this.options.tolerance == "guess" && this.helperProportions[this.floating ? 'width' : 'height'] > item[this.floating ? 'width' : 'height'])) { if (!(y1 + this.offset.click.top > t && y1 + this.offset.click.top < b && x1 + this.offset.click.left > l && x1 + this.offset.click.left < r)) return false; if (this.floating) { if (x1 + this.offset.click.left > l && x1 + this.offset.click.left < l + item.width / 2) return 2; if (x1 + this.offset.click.left > l + item.width / 2 && x1 + this.offset.click.left < r) return 1; } else { if (y1 + this.offset.click.top > t && y1 + this.offset.click.top < t + item.height / 2) return 2; if (y1 + this.offset.click.top > t + item.height / 2 && y1 + this.offset.click.top < b) return 1; } } else { if (!(l < x1 + (this.helperProportions.width / 2) && x2 - (this.helperProportions.width / 2) < r && t < y1 + (this.helperProportions.height / 2) && y2 - (this.helperProportions.height / 2) < b)) return false; if (this.floating) { if (x2 > l && x1 < l) return 2; if (x1 < r && x2 > r) return 1; } else { if (y2 > t && y1 < t) return 1; if (y1 < b && y2 > b) return 2; } }
        return false;
    }, refresh: function () { this.refreshItems(); this.refreshPositions(); }, refreshItems: function () {
        this.items = []; this.containers = [this]; var items = this.items; var self = this; var queries = [[$.isFunction(this.options.items) ? this.options.items.call(this.element, null, { options: this.options, item: this.currentItem }) : $(this.options.items, this.element), this]]; if (this.options.connectWith) { for (var i = this.options.connectWith.length - 1; i >= 0; i--) { var cur = $(this.options.connectWith[i]); for (var j = cur.length - 1; j >= 0; j--) { var inst = $.data(cur[j], 'sortable'); if (inst && !inst.options.disabled) { queries.push([$.isFunction(inst.options.items) ? inst.options.items.call(inst.element) : $(inst.options.items, inst.element), inst]); this.containers.push(inst); } }; }; }
        for (var i = queries.length - 1; i >= 0; i--) { queries[i][0].each(function () { $.data(this, 'sortable-item', queries[i][1]); items.push({ item: $(this), instance: queries[i][1], width: 0, height: 0, left: 0, top: 0 }); }); };
    }, refreshPositions: function (fast) {
        if (this.offsetParent) { var po = this.offsetParent.offset(); this.offset.parent = { top: po.top + this.offsetParentBorders.top, left: po.left + this.offsetParentBorders.left }; }
        for (var i = this.items.length - 1; i >= 0; i--) {
            if (this.items[i].instance != this.currentContainer && this.currentContainer && this.items[i].item[0] != this.currentItem[0])
                continue; var t = this.options.toleranceElement ? $(this.options.toleranceElement, this.items[i].item) : this.items[i].item; if (!fast) { this.items[i].width = t[0].offsetWidth; this.items[i].height = t[0].offsetHeight; }
            var p = t.offset(); this.items[i].left = p.left; this.items[i].top = p.top;
        }; if (this.options.custom && this.options.custom.refreshContainers) { this.options.custom.refreshContainers.call(this); } else { for (var i = this.containers.length - 1; i >= 0; i--) { var p = this.containers[i].element.offset(); this.containers[i].containerCache.left = p.left; this.containers[i].containerCache.top = p.top; this.containers[i].containerCache.width = this.containers[i].element.outerWidth(); this.containers[i].containerCache.height = this.containers[i].element.outerHeight(); }; } 
    }, destroy: function () {
        this.element.removeClass("ui-sortable ui-sortable-disabled").removeData("sortable").unbind(".sortable"); this.mouseDestroy(); for (var i = this.items.length - 1; i >= 0; i--)
            this.items[i].item.removeData("sortable-item");
    }, createPlaceholder: function (that) {
        var self = that || this, o = self.options; if (o.placeholder.constructor == String) { var className = o.placeholder; o.placeholder = { element: function () { return $('<div></div>').addClass(className)[0]; }, update: function (i, p) { p.css(i.offset()).css({ width: i.outerWidth(), height: i.outerHeight() }); } }; }
        self.placeholder = $(o.placeholder.element.call(self.element, self.currentItem)).appendTo('body').css({ position: 'absolute' }); o.placeholder.update.call(self.element, self.currentItem, self.placeholder);
    }, contactContainers: function (e) {
        for (var i = this.containers.length - 1; i >= 0; i--) {
            if (this.intersectsWith(this.containers[i].containerCache)) {
                if (!this.containers[i].containerCache.over) {
                    if (this.currentContainer != this.containers[i]) {
                        var dist = 10000; var itemWithLeastDistance = null; var base = this.positionAbs[this.containers[i].floating ? 'left' : 'top']; for (var j = this.items.length - 1; j >= 0; j--) { if (!contains(this.containers[i].element[0], this.items[j].item[0])) continue; var cur = this.items[j][this.containers[i].floating ? 'left' : 'top']; if (Math.abs(cur - base) < dist) { dist = Math.abs(cur - base); itemWithLeastDistance = this.items[j]; } }
                        if (!itemWithLeastDistance && !this.options.dropOnEmpty)
                            continue; if (this.placeholder) this.placeholder.remove(); if (this.containers[i].options.placeholder) { this.containers[i].createPlaceholder(this); } else { this.placeholder = null; ; }
                        this.currentContainer = this.containers[i]; itemWithLeastDistance ? this.rearrange(e, itemWithLeastDistance, null, true) : this.rearrange(e, null, this.containers[i].element, true); this.propagate("change", e); this.containers[i].propagate("change", e, this);
                    }
                    this.containers[i].propagate("over", e, this); this.containers[i].containerCache.over = 1;
                } 
            } else { if (this.containers[i].containerCache.over) { this.containers[i].propagate("out", e, this); this.containers[i].containerCache.over = 0; } } 
        };
    }, mouseCapture: function (e, overrideHandle) {
        if (this.options.disabled || this.options.type == 'static') return false; this.refreshItems(); var currentItem = null, self = this, nodes = $(e.target).parents().each(function () { if ($.data(this, 'sortable-item') == self) { currentItem = $(this); return false; } }); if ($.data(e.target, 'sortable-item') == self) currentItem = $(e.target); if (!currentItem) return false; if (this.options.handle && !overrideHandle) { var validHandle = false; $(this.options.handle, currentItem).find("*").andSelf().each(function () { if (this == e.target) validHandle = true; }); if (!validHandle) return false; }
        this.currentItem = currentItem; return true;
    }, mouseStart: function (e, overrideHandle, noActivation) {
        var o = this.options; this.currentContainer = this; this.refreshPositions(); this.helper = typeof o.helper == 'function' ? $(o.helper.apply(this.element[0], [e, this.currentItem])) : this.currentItem.clone(); if (!this.helper.parents('body').length) $(o.appendTo != 'parent' ? o.appendTo : this.currentItem[0].parentNode)[0].appendChild(this.helper[0]); this.helper.css({ position: 'absolute', clear: 'both' }).addClass('ui-sortable-helper'); this.margins = { left: (parseInt(this.currentItem.css("marginLeft"), 10) || 0), top: (parseInt(this.currentItem.css("marginTop"), 10) || 0) }; this.offset = this.currentItem.offset(); this.offset = { top: this.offset.top - this.margins.top, left: this.offset.left - this.margins.left }; this.offset.click = { left: e.pageX - this.offset.left, top: e.pageY - this.offset.top }; this.offsetParent = this.helper.offsetParent(); var po = this.offsetParent.offset(); this.offsetParentBorders = { top: (parseInt(this.offsetParent.css("borderTopWidth"), 10) || 0), left: (parseInt(this.offsetParent.css("borderLeftWidth"), 10) || 0) }; this.offset.parent = { top: po.top + this.offsetParentBorders.top, left: po.left + this.offsetParentBorders.left }; this.originalPosition = this.generatePosition(e); this.domPosition = { prev: this.currentItem.prev()[0], parent: this.currentItem.parent()[0] }; this.helperProportions = { width: this.helper.outerWidth(), height: this.helper.outerHeight() }; if (o.placeholder) this.createPlaceholder(); this.propagate("start", e); this.helperProportions = { width: this.helper.outerWidth(), height: this.helper.outerHeight() }; if (o.cursorAt) { if (o.cursorAt.left != undefined) this.offset.click.left = o.cursorAt.left; if (o.cursorAt.right != undefined) this.offset.click.left = this.helperProportions.width - o.cursorAt.right; if (o.cursorAt.top != undefined) this.offset.click.top = o.cursorAt.top; if (o.cursorAt.bottom != undefined) this.offset.click.top = this.helperProportions.height - o.cursorAt.bottom; }
        if (o.containment) { if (o.containment == 'parent') o.containment = this.helper[0].parentNode; if (o.containment == 'document' || o.containment == 'window') this.containment = [0 - this.offset.parent.left, 0 - this.offset.parent.top, $(o.containment == 'document' ? document : window).width() - this.offset.parent.left - this.helperProportions.width - this.margins.left - (parseInt(this.element.css("marginRight"), 10) || 0), ($(o.containment == 'document' ? document : window).height() || document.body.parentNode.scrollHeight) - this.offset.parent.top - this.helperProportions.height - this.margins.top - (parseInt(this.element.css("marginBottom"), 10) || 0)]; if (!(/^(document|window|parent)$/).test(o.containment)) { var ce = $(o.containment)[0]; var co = $(o.containment).offset(); this.containment = [co.left + (parseInt($(ce).css("borderLeftWidth"), 10) || 0) - this.offset.parent.left, co.top + (parseInt($(ce).css("borderTopWidth"), 10) || 0) - this.offset.parent.top, co.left + Math.max(ce.scrollWidth, ce.offsetWidth) - (parseInt($(ce).css("borderLeftWidth"), 10) || 0) - this.offset.parent.left - this.helperProportions.width - this.margins.left - (parseInt(this.currentItem.css("marginRight"), 10) || 0), co.top + Math.max(ce.scrollHeight, ce.offsetHeight) - (parseInt($(ce).css("borderTopWidth"), 10) || 0) - this.offset.parent.top - this.helperProportions.height - this.margins.top - (parseInt(this.currentItem.css("marginBottom"), 10) || 0)]; } }
        if (this.options.placeholder != 'clone')
            this.currentItem.css('visibility', 'hidden'); if (!noActivation) { for (var i = this.containers.length - 1; i >= 0; i--) { this.containers[i].propagate("activate", e, this); } }
        if ($.ui.ddmanager) $.ui.ddmanager.current = this; if ($.ui.ddmanager && !o.dropBehaviour) $.ui.ddmanager.prepareOffsets(this, e); this.dragging = true; this.mouseDrag(e); return true;
    }, convertPositionTo: function (d, pos) {
        if (!pos) pos = this.position; var mod = d == "absolute" ? 1 : -1; return { top: (pos.top
+ this.offset.parent.top * mod
- (this.offsetParent[0] == document.body ? 0 : this.offsetParent[0].scrollTop) * mod
+ this.margins.top * mod), left: (pos.left
+ this.offset.parent.left * mod
- (this.offsetParent[0] == document.body ? 0 : this.offsetParent[0].scrollLeft) * mod
+ this.margins.left * mod)
        };
    }, generatePosition: function (e) {
        var o = this.options; var position = { top: (e.pageY
- this.offset.click.top
- this.offset.parent.top
+ (this.offsetParent[0] == document.body ? 0 : this.offsetParent[0].scrollTop)), left: (e.pageX
- this.offset.click.left
- this.offset.parent.left
+ (this.offsetParent[0] == document.body ? 0 : this.offsetParent[0].scrollLeft))
        }; if (!this.originalPosition) return position; if (this.containment) { if (position.left < this.containment[0]) position.left = this.containment[0]; if (position.top < this.containment[1]) position.top = this.containment[1]; if (position.left > this.containment[2]) position.left = this.containment[2]; if (position.top > this.containment[3]) position.top = this.containment[3]; }
        if (o.grid) { var top = this.originalPosition.top + Math.round((position.top - this.originalPosition.top) / o.grid[1]) * o.grid[1]; position.top = this.containment ? (!(top < this.containment[1] || top > this.containment[3]) ? top : (!(top < this.containment[1]) ? top - o.grid[1] : top + o.grid[1])) : top; var left = this.originalPosition.left + Math.round((position.left - this.originalPosition.left) / o.grid[0]) * o.grid[0]; position.left = this.containment ? (!(left < this.containment[0] || left > this.containment[2]) ? left : (!(left < this.containment[0]) ? left - o.grid[0] : left + o.grid[0])) : left; }
        return position;
    }, mouseDrag: function (e) {
        this.position = this.generatePosition(e); this.positionAbs = this.convertPositionTo("absolute"); $.ui.plugin.call(this, "sort", [e, this.ui()]); this.positionAbs = this.convertPositionTo("absolute"); this.helper[0].style.left = this.position.left + 'px'; this.helper[0].style.top = this.position.top + 'px'; for (var i = this.items.length - 1; i >= 0; i--) { var intersection = this.intersectsWithEdge(this.items[i]); if (!intersection) continue; if (this.items[i].item[0] != this.currentItem[0] && this.currentItem[intersection == 1 ? "next" : "prev"]()[0] != this.items[i].item[0] && !contains(this.currentItem[0], this.items[i].item[0]) && (this.options.type == 'semi-dynamic' ? !contains(this.element[0], this.items[i].item[0]) : true)) { this.direction = intersection == 1 ? "down" : "up"; this.rearrange(e, this.items[i]); this.propagate("change", e); break; } }
        this.contactContainers(e); if ($.ui.ddmanager) $.ui.ddmanager.drag(this, e); this.element.triggerHandler("sort", [e, this.ui()], this.options["sort"]); return false;
    }, rearrange: function (e, i, a, hardRefresh) {
        a ? a[0].appendChild(this.currentItem[0]) : i.item[0].parentNode.insertBefore(this.currentItem[0], (this.direction == 'down' ? i.item[0] : i.item[0].nextSibling)); this.counter = this.counter ? ++this.counter : 1; var self = this, counter = this.counter; window.setTimeout(function () { if (counter == self.counter) self.refreshPositions(!hardRefresh); }, 0); if (this.options.placeholder)
            this.options.placeholder.update.call(this.element, this.currentItem, this.placeholder);
    }, mouseStop: function (e, noPropagation) {
        if ($.ui.ddmanager && !this.options.dropBehaviour)
            $.ui.ddmanager.drop(this, e); if (this.options.revert) { var self = this; var cur = self.currentItem.offset(); if (self.placeholder) self.placeholder.animate({ opacity: 'hide' }, (parseInt(this.options.revert, 10) || 500) - 50); $(this.helper).animate({ left: cur.left - this.offset.parent.left - self.margins.left + (this.offsetParent[0] == document.body ? 0 : this.offsetParent[0].scrollLeft), top: cur.top - this.offset.parent.top - self.margins.top + (this.offsetParent[0] == document.body ? 0 : this.offsetParent[0].scrollTop) }, parseInt(this.options.revert, 10) || 500, function () { self.clear(e); }); } else { this.clear(e, noPropagation); }
        return false;
    }, clear: function (e, noPropagation) {
        if (this.domPosition.prev != this.currentItem.prev().not(".ui-sortable-helper")[0] || this.domPosition.parent != this.currentItem.parent()[0]) this.propagate("update", e, null, noPropagation); if (!contains(this.element[0], this.currentItem[0])) { this.propagate("remove", e, null, noPropagation); for (var i = this.containers.length - 1; i >= 0; i--) { if (contains(this.containers[i].element[0], this.currentItem[0])) { this.containers[i].propagate("update", e, this, noPropagation); this.containers[i].propagate("receive", e, this, noPropagation); } }; }; for (var i = this.containers.length - 1; i >= 0; i--) { this.containers[i].propagate("deactivate", e, this, noPropagation); if (this.containers[i].containerCache.over) { this.containers[i].propagate("out", e, this); this.containers[i].containerCache.over = 0; } }
        this.dragging = false; if (this.cancelHelperRemoval) { this.propagate("stop", e, null, noPropagation); return false; }
        $(this.currentItem).css('visibility', ''); if (this.placeholder) this.placeholder.remove(); this.helper.remove(); this.helper = null; this.propagate("stop", e, null, noPropagation); return true;
    } 
    })); $.extend($.ui.sortable, { getter: "serialize toArray", defaults: { helper: "clone", tolerance: "guess", distance: 1, delay: 0, scroll: true, scrollSensitivity: 20, scrollSpeed: 20, cancel: ":input", items: '> *', zIndex: 1000, dropOnEmpty: true, appendTo: "parent"} }); $.ui.plugin.add("sortable", "cursor", { start: function (e, ui) { var t = $('body'); if (t.css("cursor")) ui.options._cursor = t.css("cursor"); t.css("cursor", ui.options.cursor); }, stop: function (e, ui) { if (ui.options._cursor) $('body').css("cursor", ui.options._cursor); } }); $.ui.plugin.add("sortable", "zIndex", { start: function (e, ui) { var t = ui.helper; if (t.css("zIndex")) ui.options._zIndex = t.css("zIndex"); t.css('zIndex', ui.options.zIndex); }, stop: function (e, ui) { if (ui.options._zIndex) $(ui.helper).css('zIndex', ui.options._zIndex); } }); $.ui.plugin.add("sortable", "opacity", { start: function (e, ui) { var t = ui.helper; if (t.css("opacity")) ui.options._opacity = t.css("opacity"); t.css('opacity', ui.options.opacity); }, stop: function (e, ui) { if (ui.options._opacity) $(ui.helper).css('opacity', ui.options._opacity); } }); $.ui.plugin.add("sortable", "scroll", { start: function (e, ui) { var o = ui.options; var i = $(this).data("sortable"); i.overflowY = function (el) { do { if (/auto|scroll/.test(el.css('overflow')) || (/auto|scroll/).test(el.css('overflow-y'))) return el; el = el.parent(); } while (el[0].parentNode); return $(document); } (i.currentItem); i.overflowX = function (el) { do { if (/auto|scroll/.test(el.css('overflow')) || (/auto|scroll/).test(el.css('overflow-x'))) return el; el = el.parent(); } while (el[0].parentNode); return $(document); } (i.currentItem); if (i.overflowY[0] != document && i.overflowY[0].tagName != 'HTML') i.overflowYOffset = i.overflowY.offset(); if (i.overflowX[0] != document && i.overflowX[0].tagName != 'HTML') i.overflowXOffset = i.overflowX.offset(); }, sort: function (e, ui) {
        var o = ui.options; var i = $(this).data("sortable"); if (i.overflowY[0] != document && i.overflowY[0].tagName != 'HTML') {
            if ((i.overflowYOffset.top + i.overflowY[0].offsetHeight) - e.pageY < o.scrollSensitivity)
                i.overflowY[0].scrollTop = i.overflowY[0].scrollTop + o.scrollSpeed; if (e.pageY - i.overflowYOffset.top < o.scrollSensitivity)
                i.overflowY[0].scrollTop = i.overflowY[0].scrollTop - o.scrollSpeed;
        } else {
            if (e.pageY - $(document).scrollTop() < o.scrollSensitivity)
                $(document).scrollTop($(document).scrollTop() - o.scrollSpeed); if ($(window).height() - (e.pageY - $(document).scrollTop()) < o.scrollSensitivity)
                $(document).scrollTop($(document).scrollTop() + o.scrollSpeed);
        }
        if (i.overflowX[0] != document && i.overflowX[0].tagName != 'HTML') {
            if ((i.overflowXOffset.left + i.overflowX[0].offsetWidth) - e.pageX < o.scrollSensitivity)
                i.overflowX[0].scrollLeft = i.overflowX[0].scrollLeft + o.scrollSpeed; if (e.pageX - i.overflowXOffset.left < o.scrollSensitivity)
                i.overflowX[0].scrollLeft = i.overflowX[0].scrollLeft - o.scrollSpeed;
        } else {
            if (e.pageX - $(document).scrollLeft() < o.scrollSensitivity)
                $(document).scrollLeft($(document).scrollLeft() - o.scrollSpeed); if ($(window).width() - (e.pageX - $(document).scrollLeft()) < o.scrollSensitivity)
                $(document).scrollLeft($(document).scrollLeft() + o.scrollSpeed);
        } 
    } 
    }); $.ui.plugin.add("sortable", "axis", { sort: function (e, ui) { var i = $(this).data("sortable"); if (ui.options.axis == "y") i.position.left = i.originalPosition.left; if (ui.options.axis == "x") i.position.top = i.originalPosition.top; } });
})(jQuery); (function ($) {
    $.widget("ui.accordion", { init: function () {
        var options = this.options; if (options.navigation) { var current = this.element.find("a").filter(options.navigationFilter); if (current.length) { if (current.filter(options.header).length) { options.active = current; } else { options.active = current.parent().parent().prev(); current.addClass("current"); } } }
        options.headers = this.element.find(options.header); options.active = findActive(options.headers, options.active); if ($.browser.msie) { this.element.find('a').css('zoom', '1'); }
        if (!this.element.hasClass("ui-accordion")) { this.element.addClass("ui-accordion"); $("<span class='ui-accordion-left'/>").insertBefore(options.headers); $("<span class='ui-accordion-right'/>").appendTo(options.headers); options.headers.addClass("ui-accordion-header").attr("tabindex", "0"); }
        var maxHeight; if (options.fillSpace) { maxHeight = this.element.parent().height(); options.headers.each(function () { maxHeight -= $(this).outerHeight(); }); var maxPadding = 0; options.headers.next().each(function () { maxPadding = Math.max(maxPadding, $(this).innerHeight() - $(this).height()); }).height(maxHeight - maxPadding); } else if (options.autoHeight) { maxHeight = 0; options.headers.next().each(function () { maxHeight = Math.max(maxHeight, $(this).outerHeight()); }).height(maxHeight); }
        options.headers.not(options.active || "").next().hide(); options.active.parent().andSelf().addClass(options.selectedClass); if (options.event) { this.element.bind((options.event) + ".accordion", clickHandler); } 
    }, activate: function (index) { clickHandler.call(this.element[0], { target: findActive(this.options.headers, index)[0] }); }, destroy: function () {
        this.options.headers.next().css("display", ""); if (this.options.fillSpace || this.options.autoHeight) { this.options.headers.next().css("height", ""); }
        $.removeData(this.element[0], "accordion"); this.element.removeClass("ui-accordion").unbind(".accordion");
    } 
    }); function scopeCallback(callback, scope) { return function () { return callback.apply(scope, arguments); }; }; function completed(cancel) {
        if (!$.data(this, "accordion")) { return; }
        var instance = $.data(this, "accordion"); var options = instance.options; options.running = cancel ? 0 : --options.running; if (options.running) { return; }
        if (options.clearStyle) { options.toShow.add(options.toHide).css({ height: "", overflow: "" }); }
        $(this).triggerHandler("accordionchange", [$.event.fix({ type: 'accordionchange', target: instance.element[0] }), options.data], options.change);
    }
    function toggle(toShow, toHide, data, clickedActive, down) {
        var options = $.data(this, "accordion").options; options.toShow = toShow; options.toHide = toHide; options.data = data; var complete = scopeCallback(completed, this); options.running = toHide.size() === 0 ? toShow.size() : toHide.size(); if (options.animated) { if (!options.alwaysOpen && clickedActive) { $.ui.accordion.animations[options.animated]({ toShow: jQuery([]), toHide: toHide, complete: complete, down: down, autoHeight: options.autoHeight }); } else { $.ui.accordion.animations[options.animated]({ toShow: toShow, toHide: toHide, complete: complete, down: down, autoHeight: options.autoHeight }); } } else {
            if (!options.alwaysOpen && clickedActive) { toShow.toggle(); } else { toHide.hide(); toShow.show(); }
            complete(true);
        } 
    }
    function clickHandler(event) {
        var options = $.data(this, "accordion").options; if (options.disabled) { return false; }
        if (!event.target && !options.alwaysOpen) { options.active.parent().andSelf().toggleClass(options.selectedClass); var toHide = options.active.next(), data = { options: options, newHeader: jQuery([]), oldHeader: options.active, newContent: jQuery([]), oldContent: toHide }, toShow = (options.active = $([])); toggle.call(this, toShow, toHide, data); return false; }
        var clicked = $(event.target); clicked = $(clicked.parents(options.header)[0] || clicked); var clickedActive = clicked[0] == options.active[0]; if (options.running || (options.alwaysOpen && clickedActive)) { return false; }
        if (!clicked.is(options.header)) { return; }
        options.active.parent().andSelf().toggleClass(options.selectedClass); if (!clickedActive) { clicked.parent().andSelf().addClass(options.selectedClass); }
        var toShow = clicked.next(), toHide = options.active.next(), data = { options: options, newHeader: clicked, oldHeader: options.active, newContent: toShow, oldContent: toHide }, down = options.headers.index(options.active[0]) > options.headers.index(clicked[0]); options.active = clickedActive ? $([]) : clicked; toggle.call(this, toShow, toHide, data, clickedActive, down); return false;
    }; function findActive(headers, selector) { return selector != undefined ? typeof selector == "number" ? headers.filter(":eq(" + selector + ")") : headers.not(headers.not(selector)) : selector === false ? $([]) : headers.filter(":eq(0)"); }
    $.extend($.ui.accordion, { defaults: { selectedClass: "selected", alwaysOpen: true, animated: 'slide', event: "click", header: "a", autoHeight: true, running: 0, navigationFilter: function () { return this.href.toLowerCase() == location.href.toLowerCase(); } }, animations: { slide: function (options, additions) {
        options = $.extend({ easing: "swing", duration: 300 }, options, additions); if (!options.toHide.size()) { options.toShow.animate({ height: "show" }, options); return; }
        var hideHeight = options.toHide.height(), showHeight = options.toShow.height(), difference = showHeight / hideHeight; options.toShow.css({ height: 0, overflow: 'hidden' }).show(); options.toHide.filter(":hidden").each(options.complete).end().filter(":visible").animate({ height: "hide" }, { step: function (now) {
            var current = (hideHeight - now) * difference; if ($.browser.msie || $.browser.opera) { current = Math.ceil(current); }
            options.toShow.height(current);
        }, duration: options.duration, easing: options.easing, complete: function () {
            if (!options.autoHeight) { options.toShow.css("height", "auto"); }
            options.complete();
        } 
        });
    }, bounceslide: function (options) { this.slide(options, { easing: options.down ? "bounceout" : "swing", duration: options.down ? 1000 : 200 }); }, easeslide: function (options) { this.slide(options, { easing: "easeinout", duration: 700 }); } 
    }
    }); $.fn.activate = function (index) { return this.accordion("activate", index); };
})(jQuery); (function ($) {
    var setDataSwitch = { dragStart: "start.draggable", drag: "drag.draggable", dragStop: "stop.draggable", maxHeight: "maxHeight.resizable", minHeight: "minHeight.resizable", maxWidth: "maxWidth.resizable", minWidth: "minWidth.resizable", resizeStart: "start.resizable", resize: "drag.resizable", resizeStop: "stop.resizable" }; $.widget("ui.dialog", { init: function () {
        var self = this, options = this.options, resizeHandles = typeof options.resizable == 'string' ? options.resizable : 'n,e,s,w,se,sw,ne,nw', uiDialogContent = this.element.addClass('ui-dialog-content').wrap('<div/>').wrap('<div/>'), uiDialogContainer = (this.uiDialogContainer = uiDialogContent.parent().addClass('ui-dialog-container').css({ position: 'relative', width: '100%', height: '100%' })), title = options.title || uiDialogContent.attr('title') || '', uiDialogTitlebar = (this.uiDialogTitlebar = $('<div class="ui-dialog-titlebar"/>')).append('<span class="ui-dialog-title">' + title + '</span>').append('<a href="#" class="ui-dialog-titlebar-close"><span>X</span></a>').prependTo(uiDialogContainer), uiDialog = (this.uiDialog = uiDialogContainer.parent()).appendTo(document.body).hide().addClass('ui-dialog').addClass(options.dialogClass).addClass(uiDialogContent.attr('className')).removeClass('ui-dialog-content').css({ position: 'absolute', width: options.width, height: options.height, overflow: 'hidden', zIndex: options.zIndex }).attr('tabIndex', -1).css('outline', 0).keydown(function (ev) { if (options.closeOnEscape) { var ESC = 27; (ev.keyCode && ev.keyCode == ESC && self.close()); } }).mousedown(function () { self.moveToTop(); }), uiDialogButtonPane = (this.uiDialogButtonPane = $('<div/>')).addClass('ui-dialog-buttonpane').css({ position: 'absolute', bottom: 0 }).appendTo(uiDialog); this.uiDialogTitlebarClose = $('.ui-dialog-titlebar-close', uiDialogTitlebar).hover(function () { $(this).addClass('ui-dialog-titlebar-close-hover'); }, function () { $(this).removeClass('ui-dialog-titlebar-close-hover'); }).mousedown(function (ev) { ev.stopPropagation(); }).click(function () { self.close(); return false; }); this.uiDialogTitlebar.find("*").add(this.uiDialogTitlebar).each(function () { $.ui.disableSelection(this); }); if ($.fn.draggable) { uiDialog.draggable({ cancel: '.ui-dialog-content', helper: options.dragHelper, handle: '.ui-dialog-titlebar', start: function (e, ui) { self.moveToTop(); (options.dragStart && options.dragStart.apply(self.element[0], arguments)); }, drag: function (e, ui) { (options.drag && options.drag.apply(self.element[0], arguments)); }, stop: function (e, ui) { (options.dragStop && options.dragStop.apply(self.element[0], arguments)); $.ui.dialog.overlay.resize(); } }); (options.draggable || uiDialog.draggable('disable')); }
        if ($.fn.resizable) { uiDialog.resizable({ cancel: '.ui-dialog-content', helper: options.resizeHelper, maxWidth: options.maxWidth, maxHeight: options.maxHeight, minWidth: options.minWidth, minHeight: options.minHeight, start: function () { (options.resizeStart && options.resizeStart.apply(self.element[0], arguments)); }, resize: function (e, ui) { (options.autoResize && self.size.apply(self)); (options.resize && options.resize.apply(self.element[0], arguments)); }, handles: resizeHandles, stop: function (e, ui) { (options.autoResize && self.size.apply(self)); (options.resizeStop && options.resizeStop.apply(self.element[0], arguments)); $.ui.dialog.overlay.resize(); } }); (options.resizable || uiDialog.resizable('disable')); }
        this.createButtons(options.buttons); this.isOpen = false; (options.bgiframe && $.fn.bgiframe && uiDialog.bgiframe()); (options.autoOpen && this.open());
    }, setData: function (key, value) {
        (setDataSwitch[key] && this.uiDialog.data(setDataSwitch[key], value)); switch (key) { case "buttons": this.createButtons(value); break; case "draggable": this.uiDialog.draggable(value ? 'enable' : 'disable'); break; case "height": this.uiDialog.height(value); break; case "position": this.position(value); break; case "resizable": (typeof value == 'string' && this.uiDialog.data('handles.resizable', value)); this.uiDialog.resizable(value ? 'enable' : 'disable'); break; case "title": $(".ui-dialog-title", this.uiDialogTitlebar).text(value); break; case "width": this.uiDialog.width(value); break; }
        $.widget.prototype.setData.apply(this, arguments);
    }, position: function (pos) {
        var wnd = $(window), doc = $(document), pTop = doc.scrollTop(), pLeft = doc.scrollLeft(), minTop = pTop; if ($.inArray(pos, ['center', 'top', 'right', 'bottom', 'left']) >= 0) { pos = [pos == 'right' || pos == 'left' ? pos : 'center', pos == 'top' || pos == 'bottom' ? pos : 'middle']; }
        if (pos.constructor != Array) { pos = ['center', 'middle']; }
        if (pos[0].constructor == Number) { pLeft += pos[0]; } else { switch (pos[0]) { case 'left': pLeft += 0; break; case 'right': pLeft += wnd.width() - this.uiDialog.width(); break; default: case 'center': pLeft += (wnd.width() - this.uiDialog.width()) / 2; } }
        if (pos[1].constructor == Number) { pTop += pos[1]; } else { switch (pos[1]) { case 'top': pTop += 0; break; case 'bottom': pTop += wnd.height() - this.uiDialog.height(); break; default: case 'middle': pTop += (wnd.height() - this.uiDialog.height()) / 2; } }
        pTop = Math.max(pTop, minTop); this.uiDialog.css({ top: pTop, left: pLeft });
    }, size: function () { var container = this.uiDialogContainer, titlebar = this.uiDialogTitlebar, content = this.element, tbMargin = parseInt(content.css('margin-top'), 10) + parseInt(content.css('margin-bottom'), 10), lrMargin = parseInt(content.css('margin-left'), 10) + parseInt(content.css('margin-right'), 10); content.height(container.height() - titlebar.outerHeight() - tbMargin); content.width(container.width() - lrMargin); }, open: function () {
        if (this.isOpen) { return; }
        this.overlay = this.options.modal ? new $.ui.dialog.overlay(this) : null; (this.uiDialog.next().length > 0) && this.uiDialog.appendTo('body'); this.position(this.options.position); this.uiDialog.show(this.options.show); this.options.autoResize && this.size(); this.moveToTop(true); var openEV = null; var openUI = { options: this.options }; this.uiDialogTitlebarClose.focus(); this.element.triggerHandler("dialogopen", [openEV, openUI], this.options.open); this.isOpen = true;
    }, moveToTop: function (force) {
        if ((this.options.modal && !force) || (!this.options.stack && !this.options.modal)) { return this.element.triggerHandler("dialogfocus", [null, { options: this.options}], this.options.focus); }
        var maxZ = this.options.zIndex, options = this.options; $('.ui-dialog:visible').each(function () { maxZ = Math.max(maxZ, parseInt($(this).css('z-index'), 10) || options.zIndex); }); (this.overlay && this.overlay.$el.css('z-index', ++maxZ)); this.uiDialog.css('z-index', ++maxZ); this.element.triggerHandler("dialogfocus", [null, { options: this.options}], this.options.focus);
    }, close: function () { (this.overlay && this.overlay.destroy()); this.uiDialog.hide(this.options.hide); var closeEV = null; var closeUI = { options: this.options }; this.element.triggerHandler("dialogclose", [closeEV, closeUI], this.options.close); $.ui.dialog.overlay.resize(); this.isOpen = false; }, destroy: function () { (this.overlay && this.overlay.destroy()); this.uiDialog.hide(); this.element.unbind('.dialog').removeData('dialog').removeClass('ui-dialog-content').hide().appendTo('body'); this.uiDialog.remove(); }, createButtons: function (buttons) { var self = this, hasButtons = false, uiDialogButtonPane = this.uiDialogButtonPane; uiDialogButtonPane.empty().hide(); $.each(buttons, function () { return !(hasButtons = true); }); if (hasButtons) { uiDialogButtonPane.show(); $.each(buttons, function (name, fn) { $('<button/>').text(name).click(function () { fn.apply(self.element[0], arguments); }).appendTo(uiDialogButtonPane); }); } } 
    }); $.extend($.ui.dialog, { defaults: { autoOpen: true, autoResize: true, bgiframe: false, buttons: {}, closeOnEscape: true, draggable: true, height: 200, minHeight: 100, minWidth: 150, modal: false, overlay: {}, position: 'center', resizable: true, stack: true, width: 300, zIndex: 1000 }, overlay: function (dialog) { this.$el = $.ui.dialog.overlay.create(dialog); } }); $.extend($.ui.dialog.overlay, { instances: [], events: $.map('focus,mousedown,mouseup,keydown,keypress,click'.split(','), function (e) { return e + '.dialog-overlay'; }).join(' '), create: function (dialog) {
        if (this.instances.length === 0) {
            setTimeout(function () {
                $('a, :input').bind($.ui.dialog.overlay.events, function () {
                    var allow = false; var $dialog = $(this).parents('.ui-dialog'); if ($dialog.length) { var $overlays = $('.ui-dialog-overlay'); if ($overlays.length) { var maxZ = parseInt($overlays.css('z-index'), 10); $overlays.each(function () { maxZ = Math.max(maxZ, parseInt($(this).css('z-index'), 10)); }); allow = parseInt($dialog.css('z-index'), 10) > maxZ; } else { allow = true; } }
                    return allow;
                });
            }, 1); $(document).bind('keydown.dialog-overlay', function (e) { var ESC = 27; (e.keyCode && e.keyCode == ESC && dialog.close()); }); $(window).bind('resize.dialog-overlay', $.ui.dialog.overlay.resize);
        }
        var $el = $('<div/>').appendTo(document.body).addClass('ui-dialog-overlay').css($.extend({ borderWidth: 0, margin: 0, padding: 0, position: 'absolute', top: 0, left: 0, width: this.width(), height: this.height() }, dialog.options.overlay)); (dialog.options.bgiframe && $.fn.bgiframe && $el.bgiframe()); this.instances.push($el); return $el;
    }, destroy: function ($el) {
        this.instances.splice($.inArray(this.instances, $el), 1); if (this.instances.length === 0) { $('a, :input').add([document, window]).unbind('.dialog-overlay'); }
        $el.remove();
    }, height: function () { if ($.browser.msie && $.browser.version < 7) { var scrollHeight = Math.max(document.documentElement.scrollHeight, document.body.scrollHeight); var offsetHeight = Math.max(document.documentElement.offsetHeight, document.body.offsetHeight); if (scrollHeight < offsetHeight) { return $(window).height() + 'px'; } else { return scrollHeight + 'px'; } } else { return $(document).height() + 'px'; } }, width: function () { if ($.browser.msie && $.browser.version < 7) { var scrollWidth = Math.max(document.documentElement.scrollWidth, document.body.scrollWidth); var offsetWidth = Math.max(document.documentElement.offsetWidth, document.body.offsetWidth); if (scrollWidth < offsetWidth) { return $(window).width() + 'px'; } else { return scrollWidth + 'px'; } } else { return $(document).width() + 'px'; } }, resize: function () { var $overlays = $([]); $.each($.ui.dialog.overlay.instances, function () { $overlays = $overlays.add(this); }); $overlays.css({ width: 0, height: 0 }).css({ width: $.ui.dialog.overlay.width(), height: $.ui.dialog.overlay.height() }); } 
    }); $.extend($.ui.dialog.overlay.prototype, { destroy: function () { $.ui.dialog.overlay.destroy(this.$el); } });
})(jQuery); (function ($) {
    $.fn.unwrap = $.fn.unwrap || function (expr) { return this.each(function () { $(this).parents(expr).eq(0).after(this).remove(); }); }; $.widget("ui.slider", { plugins: {}, ui: function (e) { return { options: this.options, handle: this.currentHandle, value: this.options.axis != "both" || !this.options.axis ? Math.round(this.value(null, this.options.axis == "vertical" ? "y" : "x")) : { x: Math.round(this.value(null, "x")), y: Math.round(this.value(null, "y")) }, range: this.getRange() }; }, propagate: function (n, e) { $.ui.plugin.call(this, n, [e, this.ui()]); this.element.triggerHandler(n == "slide" ? n : "slide" + n, [e, this.ui()], this.options[n]); }, destroy: function () {
        this.element.removeClass("ui-slider ui-slider-disabled").removeData("slider").unbind(".slider"); if (this.handle && this.handle.length) { this.handle.unwrap("a"); this.handle.each(function () { $(this).data("mouse").mouseDestroy(); }); }
        this.generated && this.generated.remove();
    }, setData: function (key, value) {
        $.widget.prototype.setData.apply(this, arguments); if (/min|max|steps/.test(key)) { this.initBoundaries(); }
        if (key == "range") { value ? this.handle.length == 2 && this.createRange() : this.removeRange(); } 
    }, init: function () {
        var self = this; this.element.addClass("ui-slider"); this.initBoundaries(); this.handle = $(this.options.handle, this.element); if (!this.handle.length) {
            self.handle = self.generated = $(self.options.handles || [0]).map(function () {
                var handle = $("<div/>").addClass("ui-slider-handle").appendTo(self.element); if (this.id)
                    handle.attr("id", this.id); return handle[0];
            });
        }
        var handleclass = function (el) { this.element = $(el); this.element.data("mouse", this); this.options = self.options; this.element.bind("mousedown", function () { if (self.currentHandle) this.blur(self.currentHandle); self.focus(this, 1); }); this.mouseInit(); }; $.extend(handleclass.prototype, $.ui.mouse, { mouseStart: function (e) { return self.start.call(self, e, this.element[0]); }, mouseStop: function (e) { return self.stop.call(self, e, this.element[0]); }, mouseDrag: function (e) { return self.drag.call(self, e, this.element[0]); }, mouseCapture: function () { return true; }, trigger: function (e) { this.mouseDown(e); } }); $(this.handle).each(function () { new handleclass(this); }).wrap('<a href="javascript:void(0)" style="outline:none;border:none;"></a>').parent().bind('focus', function (e) { self.focus(this.firstChild); }).bind('blur', function (e) { self.blur(this.firstChild); }).bind('keydown', function (e) { if (!self.options.noKeyboard) self.keydown(e.keyCode, this.firstChild); }); this.element.bind('mousedown.slider', function (e) { self.click.apply(self, [e]); self.currentHandle.data("mouse").trigger(e); self.firstValue = self.firstValue + 1; }); $.each(this.options.handles || [], function (index, handle) { self.moveTo(handle.start, index, true); }); if (!isNaN(this.options.startValue))
            this.moveTo(this.options.startValue, 0, true); this.previousHandle = $(this.handle[0]); if (this.handle.length == 2 && this.options.range) this.createRange();
    }, initBoundaries: function () { var element = this.element[0], o = this.options; this.actualSize = { width: this.element.outerWidth(), height: this.element.outerHeight() }; $.extend(o, { axis: o.axis || (element.offsetWidth < element.offsetHeight ? 'vertical' : 'horizontal'), max: !isNaN(parseInt(o.max, 10)) ? { x: parseInt(o.max, 10), y: parseInt(o.max, 10)} : ({ x: o.max && o.max.x || 100, y: o.max && o.max.y || 100 }), min: !isNaN(parseInt(o.min, 10)) ? { x: parseInt(o.min, 10), y: parseInt(o.min, 10)} : ({ x: o.min && o.min.x || 0, y: o.min && o.min.y || 0 }) }); o.realMax = { x: o.max.x - o.min.x, y: o.max.y - o.min.y }; o.stepping = { x: o.stepping && o.stepping.x || parseInt(o.stepping, 10) || (o.steps ? o.realMax.x / (o.steps.x || parseInt(o.steps, 10) || o.realMax.x) : 0), y: o.stepping && o.stepping.y || parseInt(o.stepping, 10) || (o.steps ? o.realMax.y / (o.steps.y || parseInt(o.steps, 10) || o.realMax.y) : 0) }; }, keydown: function (keyCode, handle) { if (/(37|38|39|40)/.test(keyCode)) { this.moveTo({ x: /(37|39)/.test(keyCode) ? (keyCode == 37 ? '-' : '+') + '=' + this.oneStep("x") : 0, y: /(38|40)/.test(keyCode) ? (keyCode == 38 ? '-' : '+') + '=' + this.oneStep("y") : 0 }, handle); } }, focus: function (handle, hard) {
        this.currentHandle = $(handle).addClass('ui-slider-handle-active'); if (hard)
            this.currentHandle.parent()[0].focus();
    }, blur: function (handle) { $(handle).removeClass('ui-slider-handle-active'); if (this.currentHandle && this.currentHandle[0] == handle) { this.previousHandle = this.currentHandle; this.currentHandle = null; }; }, click: function (e) {
        var pointer = [e.pageX, e.pageY]; var clickedHandle = false; this.handle.each(function () {
            if (this == e.target)
                clickedHandle = true;
        }); if (clickedHandle || this.options.disabled || !(this.currentHandle || this.previousHandle))
            return; if (!this.currentHandle && this.previousHandle)
            this.focus(this.previousHandle, true); this.offset = this.element.offset(); this.moveTo({ y: this.convertValue(e.pageY - this.offset.top - this.currentHandle[0].offsetHeight / 2, "y"), x: this.convertValue(e.pageX - this.offset.left - this.currentHandle[0].offsetWidth / 2, "x") }, null, !this.options.distance);
    }, createRange: function () { if (this.rangeElement) return; this.rangeElement = $('<div></div>').addClass('ui-slider-range').css({ position: 'absolute' }).appendTo(this.element); this.updateRange(); }, removeRange: function () { this.rangeElement.remove(); this.rangeElement = null; }, updateRange: function () { var prop = this.options.axis == "vertical" ? "top" : "left"; var size = this.options.axis == "vertical" ? "height" : "width"; this.rangeElement.css(prop, (parseInt($(this.handle[0]).css(prop), 10) || 0) + this.handleSize(0, this.options.axis == "vertical" ? "y" : "x") / 2); this.rangeElement.css(size, (parseInt($(this.handle[1]).css(prop), 10) || 0) - (parseInt($(this.handle[0]).css(prop), 10) || 0)); }, getRange: function () { return this.rangeElement ? this.convertValue(parseInt(this.rangeElement.css(this.options.axis == "vertical" ? "height" : "width"), 10), this.options.axis == "vertical" ? "y" : "x") : null; }, handleIndex: function () { return this.handle.index(this.currentHandle[0]); }, value: function (handle, axis) { if (this.handle.length == 1) this.currentHandle = this.handle; if (!axis) axis = this.options.axis == "vertical" ? "y" : "x"; var curHandle = $(handle != undefined && handle !== null ? this.handle[handle] || handle : this.currentHandle); if (curHandle.data("mouse").sliderValue) { return parseInt(curHandle.data("mouse").sliderValue[axis], 10); } else { return parseInt(((parseInt(curHandle.css(axis == "x" ? "left" : "top"), 10) / (this.actualSize[axis == "x" ? "width" : "height"] - this.handleSize(handle, axis))) * this.options.realMax[axis]) + this.options.min[axis], 10); } }, convertValue: function (value, axis) { return this.options.min[axis] + (value / (this.actualSize[axis == "x" ? "width" : "height"] - this.handleSize(null, axis))) * this.options.realMax[axis]; }, translateValue: function (value, axis) { return ((value - this.options.min[axis]) / this.options.realMax[axis]) * (this.actualSize[axis == "x" ? "width" : "height"] - this.handleSize(null, axis)); }, translateRange: function (value, axis) {
        if (this.rangeElement) {
            if (this.currentHandle[0] == this.handle[0] && value >= this.translateValue(this.value(1), axis))
                value = this.translateValue(this.value(1, axis) - this.oneStep(axis), axis); if (this.currentHandle[0] == this.handle[1] && value <= this.translateValue(this.value(0), axis))
                value = this.translateValue(this.value(0, axis) + this.oneStep(axis), axis);
        }
        if (this.options.handles) { var handle = this.options.handles[this.handleIndex()]; if (value < this.translateValue(handle.min, axis)) { value = this.translateValue(handle.min, axis); } else if (value > this.translateValue(handle.max, axis)) { value = this.translateValue(handle.max, axis); } }
        return value;
    }, translateLimits: function (value, axis) {
        if (value >= this.actualSize[axis == "x" ? "width" : "height"] - this.handleSize(null, axis))
            value = this.actualSize[axis == "x" ? "width" : "height"] - this.handleSize(null, axis); if (value <= 0)
            value = 0; return value;
    }, handleSize: function (handle, axis) { return $(handle != undefined && handle !== null ? this.handle[handle] : this.currentHandle)[0]["offset" + (axis == "x" ? "Width" : "Height")]; }, oneStep: function (axis) { return this.options.stepping[axis] || 1; }, start: function (e, handle) {
        var o = this.options; if (o.disabled) return false; this.actualSize = { width: this.element.outerWidth(), height: this.element.outerHeight() }; if (!this.currentHandle)
            this.focus(this.previousHandle, true); this.offset = this.element.offset(); this.handleOffset = this.currentHandle.offset(); this.clickOffset = { top: e.pageY - this.handleOffset.top, left: e.pageX - this.handleOffset.left }; this.firstValue = this.value(); this.propagate('start', e); this.drag(e, handle); return true;
    }, stop: function (e) {
        this.propagate('stop', e); if (this.firstValue != this.value())
            this.propagate('change', e); this.focus(this.currentHandle, true); return false;
    }, drag: function (e, handle) {
        var o = this.options; var position = { top: e.pageY - this.offset.top - this.clickOffset.top, left: e.pageX - this.offset.left - this.clickOffset.left }; if (!this.currentHandle) this.focus(this.previousHandle, true); position.left = this.translateLimits(position.left, "x"); position.top = this.translateLimits(position.top, "y"); if (o.stepping.x) { var value = this.convertValue(position.left, "x"); value = Math.round(value / o.stepping.x) * o.stepping.x; position.left = this.translateValue(value, "x"); }
        if (o.stepping.y) { var value = this.convertValue(position.top, "y"); value = Math.round(value / o.stepping.y) * o.stepping.y; position.top = this.translateValue(value, "y"); }
        position.left = this.translateRange(position.left, "x"); position.top = this.translateRange(position.top, "y"); if (o.axis != "vertical") this.currentHandle.css({ left: position.left }); if (o.axis != "horizontal") this.currentHandle.css({ top: position.top }); this.currentHandle.data("mouse").sliderValue = { x: Math.round(this.convertValue(position.left, "x")) || 0, y: Math.round(this.convertValue(position.top, "y")) || 0 }; if (this.rangeElement)
            this.updateRange(); this.propagate('slide', e); return false;
    }, moveTo: function (value, handle, noPropagation) {
        var o = this.options; this.actualSize = { width: this.element.outerWidth(), height: this.element.outerHeight() }; if (handle == undefined && !this.currentHandle && this.handle.length != 1)
            return false; if (handle == undefined && !this.currentHandle)
            handle = 0; if (handle != undefined)
            this.currentHandle = this.previousHandle = $(this.handle[handle] || handle); if (value.x !== undefined && value.y !== undefined) { var x = value.x, y = value.y; } else { var x = value, y = value; }
        if (x !== undefined && x.constructor != Number) { var me = /^\-\=/.test(x), pe = /^\+\=/.test(x); if (me || pe) { x = this.value(null, "x") + parseInt(x.replace(me ? '=' : '+=', ''), 10); } else { x = isNaN(parseInt(x, 10)) ? undefined : parseInt(x, 10); } }
        if (y !== undefined && y.constructor != Number) { var me = /^\-\=/.test(y), pe = /^\+\=/.test(y); if (me || pe) { y = this.value(null, "y") + parseInt(y.replace(me ? '=' : '+=', ''), 10); } else { y = isNaN(parseInt(y, 10)) ? undefined : parseInt(y, 10); } }
        if (o.axis != "vertical" && x !== undefined) { if (o.stepping.x) x = Math.round(x / o.stepping.x) * o.stepping.x; x = this.translateValue(x, "x"); x = this.translateLimits(x, "x"); x = this.translateRange(x, "x"); o.animate ? this.currentHandle.stop().animate({ left: x }, (Math.abs(parseInt(this.currentHandle.css("left")) - x)) * (!isNaN(parseInt(o.animate)) ? o.animate : 5)) : this.currentHandle.css({ left: x }); }
        if (o.axis != "horizontal" && y !== undefined) { if (o.stepping.y) y = Math.round(y / o.stepping.y) * o.stepping.y; y = this.translateValue(y, "y"); y = this.translateLimits(y, "y"); y = this.translateRange(y, "y"); o.animate ? this.currentHandle.stop().animate({ top: y }, (Math.abs(parseInt(this.currentHandle.css("top")) - y)) * (!isNaN(parseInt(o.animate)) ? o.animate : 5)) : this.currentHandle.css({ top: y }); }
        if (this.rangeElement)
            this.updateRange(); this.currentHandle.data("mouse").sliderValue = { x: Math.round(this.convertValue(x, "x")) || 0, y: Math.round(this.convertValue(y, "y")) || 0 }; if (!noPropagation) { this.propagate('start', null); this.propagate('stop', null); this.propagate('change', null); this.propagate("slide", null); } 
    } 
    }); $.ui.slider.getter = "value"; $.ui.slider.defaults = { handle: ".ui-slider-handle", distance: 1, animate: false };
})(jQuery); (function ($) {
    $.widget("ui.tabs", { init: function () { this.options.event += '.tabs'; this.tabify(true); }, setData: function (key, value) {
        if ((/^selected/).test(key))
            this.select(value); else { this.options[key] = value; this.tabify(); } 
    }, length: function () { return this.$tabs.length; }, tabId: function (a) { return a.title && a.title.replace(/\s/g, '_').replace(/[^A-Za-z0-9\-_:\.]/g, '') || this.options.idPrefix + $.data(a); }, ui: function (tab, panel) { return { options: this.options, tab: tab, panel: panel, index: this.$tabs.index(tab) }; }, tabify: function (init) {
        this.$lis = $('li:has(a[href])', this.element); this.$tabs = this.$lis.map(function () { return $('a', this)[0]; }); this.$panels = $([]); var self = this, o = this.options; this.$tabs.each(function (i, a) {
            if (a.hash && a.hash.replace('#', ''))
                self.$panels = self.$panels.add(a.hash); else if ($(a).attr('href') != '#') {
                $.data(a, 'href.tabs', a.href); $.data(a, 'load.tabs', a.href); var id = self.tabId(a); a.href = '#' + id; var $panel = $('#' + id); if (!$panel.length) { $panel = $(o.panelTemplate).attr('id', id).addClass(o.panelClass).insertAfter(self.$panels[i - 1] || self.element); $panel.data('destroy.tabs', true); }
                self.$panels = self.$panels.add($panel);
            }
            else
                o.disabled.push(i + 1);
        }); if (init) {
            this.element.addClass(o.navClass); this.$panels.each(function () { var $this = $(this); $this.addClass(o.panelClass); }); if (o.selected === undefined) {
                if (location.hash) {
                    this.$tabs.each(function (i, a) {
                        if (a.hash == location.hash) {
                            o.selected = i; if ($.browser.msie || $.browser.opera) { var $toShow = $(location.hash), toShowId = $toShow.attr('id'); $toShow.attr('id', ''); setTimeout(function () { $toShow.attr('id', toShowId); }, 500); }
                            scrollTo(0, 0); return false;
                        } 
                    });
                }
                else if (o.cookie) {
                    var index = parseInt($.cookie('ui-tabs' + $.data(self.element)), 10); if (index && self.$tabs[index])
                        o.selected = index;
                }
                else if (self.$lis.filter('.' + o.selectedClass).length)
                    o.selected = self.$lis.index(self.$lis.filter('.' + o.selectedClass)[0]);
            }
            o.selected = o.selected === null || o.selected !== undefined ? o.selected : 0; o.disabled = $.unique(o.disabled.concat($.map(this.$lis.filter('.' + o.disabledClass), function (n, i) { return self.$lis.index(n); }))).sort(); if ($.inArray(o.selected, o.disabled) != -1)
                o.disabled.splice($.inArray(o.selected, o.disabled), 1); this.$panels.addClass(o.hideClass); this.$lis.removeClass(o.selectedClass); if (o.selected !== null) {
                this.$panels.eq(o.selected).show().removeClass(o.hideClass); this.$lis.eq(o.selected).addClass(o.selectedClass); var onShow = function () { $(self.element).triggerHandler('tabsshow', [self.fakeEvent('tabsshow'), self.ui(self.$tabs[o.selected], self.$panels[o.selected])], o.show); }; if ($.data(this.$tabs[o.selected], 'load.tabs'))
                    this.load(o.selected, onShow); else
                    onShow();
            }
            $(window).bind('unload', function () { self.$tabs.unbind('.tabs'); self.$lis = self.$tabs = self.$panels = null; });
        }
        for (var i = 0, li; li = this.$lis[i]; i++)
            $(li)[$.inArray(i, o.disabled) != -1 && !$(li).hasClass(o.selectedClass) ? 'addClass' : 'removeClass'](o.disabledClass); if (o.cache === false)
            this.$tabs.removeData('cache.tabs'); var hideFx, showFx, baseFx = { 'min-width': 0, duration: 1 }, baseDuration = 'normal'; if (o.fx && o.fx.constructor == Array)
            hideFx = o.fx[0] || baseFx, showFx = o.fx[1] || baseFx; else
            hideFx = showFx = o.fx || baseFx; var resetCSS = { display: '', overflow: '', height: '' }; if (!$.browser.msie)
            resetCSS.opacity = ''; function hideTab(clicked, $hide, $show) {
                $hide.animate(hideFx, hideFx.duration || baseDuration, function () {
                    $hide.addClass(o.hideClass).css(resetCSS); if ($.browser.msie && hideFx.opacity)
                        $hide[0].style.filter = ''; if ($show)
                        showTab(clicked, $show, $hide);
                });
            }
        function showTab(clicked, $show, $hide) {
            if (showFx === baseFx)
                $show.css('display', 'block'); $show.animate(showFx, showFx.duration || baseDuration, function () {
                    $show.removeClass(o.hideClass).css(resetCSS); if ($.browser.msie && showFx.opacity)
                        $show[0].style.filter = ''; $(self.element).triggerHandler('tabsshow', [self.fakeEvent('tabsshow'), self.ui(clicked, $show[0])], o.show);
                });
        }
        function switchTab(clicked, $li, $hide, $show) { $li.addClass(o.selectedClass).siblings().removeClass(o.selectedClass); hideTab(clicked, $hide, $show); }
        this.$tabs.unbind('.tabs').bind(o.event, function () {
            var $li = $(this).parents('li:eq(0)'), $hide = self.$panels.filter(':visible'), $show = $(this.hash); if (($li.hasClass(o.selectedClass) && !o.unselect) || $li.hasClass(o.disabledClass) || $(this).hasClass(o.loadingClass) || $(self.element).triggerHandler('tabsselect', [self.fakeEvent('tabsselect'), self.ui(this, $show[0])], o.select) === false) { this.blur(); return false; }
            self.options.selected = self.$tabs.index(this); if (o.unselect) { if ($li.hasClass(o.selectedClass)) { self.options.selected = null; $li.removeClass(o.selectedClass); self.$panels.stop(); hideTab(this, $hide); this.blur(); return false; } else if (!$hide.length) { self.$panels.stop(); var a = this; self.load(self.$tabs.index(this), function () { $li.addClass(o.selectedClass).addClass(o.unselectClass); showTab(a, $show); }); this.blur(); return false; } }
            if (o.cookie)
                $.cookie('ui-tabs' + $.data(self.element), self.options.selected, o.cookie); self.$panels.stop(); if ($show.length) { var a = this; self.load(self.$tabs.index(this), $hide.length ? function () { switchTab(a, $li, $hide, $show); } : function () { $li.addClass(o.selectedClass); showTab(a, $show); }); } else
                throw 'jQuery UI Tabs: Mismatching fragment identifier.'; if ($.browser.msie)
                this.blur(); return false;
        }); if (!(/^click/).test(o.event))
            this.$tabs.bind('click.tabs', function () { return false; });
    }, add: function (url, label, index) {
        if (index == undefined)
            index = this.$tabs.length; var o = this.options; var $li = $(o.tabTemplate.replace(/#\{href\}/g, url).replace(/#\{label\}/g, label)); $li.data('destroy.tabs', true); var id = url.indexOf('#') == 0 ? url.replace('#', '') : this.tabId($('a:first-child', $li)[0]); var $panel = $('#' + id); if (!$panel.length) { $panel = $(o.panelTemplate).attr('id', id).addClass(o.hideClass).data('destroy.tabs', true); }
        $panel.addClass(o.panelClass); if (index >= this.$lis.length) { $li.appendTo(this.element); $panel.appendTo(this.element[0].parentNode); } else { $li.insertBefore(this.$lis[index]); $panel.insertBefore(this.$panels[index]); }
        o.disabled = $.map(o.disabled, function (n, i) { return n >= index ? ++n : n }); this.tabify(); if (this.$tabs.length == 1) {
            $li.addClass(o.selectedClass); $panel.removeClass(o.hideClass); var href = $.data(this.$tabs[0], 'load.tabs'); if (href)
                this.load(index, href);
        }
        this.element.triggerHandler('tabsadd', [this.fakeEvent('tabsadd'), this.ui(this.$tabs[index], this.$panels[index])], o.add);
    }, remove: function (index) {
        var o = this.options, $li = this.$lis.eq(index).remove(), $panel = this.$panels.eq(index).remove(); if ($li.hasClass(o.selectedClass) && this.$tabs.length > 1)
            this.select(index + (index + 1 < this.$tabs.length ? 1 : -1)); o.disabled = $.map($.grep(o.disabled, function (n, i) { return n != index; }), function (n, i) { return n >= index ? --n : n }); this.tabify(); this.element.triggerHandler('tabsremove', [this.fakeEvent('tabsremove'), this.ui($li.find('a')[0], $panel[0])], o.remove);
    }, enable: function (index) {
        var o = this.options; if ($.inArray(index, o.disabled) == -1)
            return; var $li = this.$lis.eq(index).removeClass(o.disabledClass); if ($.browser.safari) { $li.css('display', 'inline-block'); setTimeout(function () { $li.css('display', 'block'); }, 0); }
        o.disabled = $.grep(o.disabled, function (n, i) { return n != index; }); this.element.triggerHandler('tabsenable', [this.fakeEvent('tabsenable'), this.ui(this.$tabs[index], this.$panels[index])], o.enable);
    }, disable: function (index) { var self = this, o = this.options; if (index != o.selected) { this.$lis.eq(index).addClass(o.disabledClass); o.disabled.push(index); o.disabled.sort(); this.element.triggerHandler('tabsdisable', [this.fakeEvent('tabsdisable'), this.ui(this.$tabs[index], this.$panels[index])], o.disable); } }, select: function (index) {
        if (typeof index == 'string')
            index = this.$tabs.index(this.$tabs.filter('[href$=' + index + ']')[0]); this.$tabs.eq(index).trigger(this.options.event);
    }, load: function (index, callback) {
        var self = this, o = this.options, $a = this.$tabs.eq(index), a = $a[0], bypassCache = callback == undefined || callback === false, url = $a.data('load.tabs'); callback = callback || function () { }; if (!url || !bypassCache && $.data(a, 'cache.tabs')) { callback(); return; }
        var inner = function (parent) { var $parent = $(parent), $inner = $parent.find('*:last'); return $inner.length && $inner.is(':not(img)') && $inner || $parent; }; var cleanup = function () {
            self.$tabs.filter('.' + o.loadingClass).removeClass(o.loadingClass).each(function () {
                if (o.spinner)
                    inner(this).parent().html(inner(this).data('label.tabs'));
            }); self.xhr = null;
        }; if (o.spinner) { var label = inner(a).html(); inner(a).wrapInner('<em></em>').find('em').data('label.tabs', label).html(o.spinner); }
        var ajaxOptions = $.extend({}, o.ajaxOptions, { url: url, success: function (r, s) {
            $(a.hash).html(r); cleanup(); if (o.cache)
                $.data(a, 'cache.tabs', true); $(self.element).triggerHandler('tabsload', [self.fakeEvent('tabsload'), self.ui(self.$tabs[index], self.$panels[index])], o.load); o.ajaxOptions.success && o.ajaxOptions.success(r, s); callback();
        } 
        }); if (this.xhr) { this.xhr.abort(); cleanup(); }
        $a.addClass(o.loadingClass); setTimeout(function () { self.xhr = $.ajax(ajaxOptions); }, 0);
    }, url: function (index, url) { this.$tabs.eq(index).removeData('cache.tabs').data('load.tabs', url); }, destroy: function () {
        var o = this.options; this.element.unbind('.tabs').removeClass(o.navClass).removeData('tabs'); this.$tabs.each(function () {
            var href = $.data(this, 'href.tabs'); if (href)
                this.href = href; var $this = $(this).unbind('.tabs'); $.each(['href', 'load', 'cache'], function (i, prefix) { $this.removeData(prefix + '.tabs'); });
        }); this.$lis.add(this.$panels).each(function () {
            if ($.data(this, 'destroy.tabs'))
                $(this).remove(); else
                $(this).removeClass([o.selectedClass, o.unselectClass, o.disabledClass, o.panelClass, o.hideClass].join(' '));
        });
    }, fakeEvent: function (type) { return $.event.fix({ type: type, target: this.element[0] }); } 
    }); $.ui.tabs.defaults = { unselect: false, event: 'click', disabled: [], cookie: null, spinner: 'Loading&#8230;', cache: false, idPrefix: 'ui-tabs-', ajaxOptions: {}, fx: null, tabTemplate: '<li><a href="#{href}"><span>#{label}</span></a></li>', panelTemplate: '<div></div>', navClass: 'ui-tabs-nav', selectedClass: 'ui-tabs-selected', unselectClass: 'ui-tabs-unselect', disabledClass: 'ui-tabs-disabled', panelClass: 'ui-tabs-panel', hideClass: 'ui-tabs-hide', loadingClass: 'ui-tabs-loading' }; $.ui.tabs.getter = "length"; $.extend($.ui.tabs.prototype, { rotation: null, rotate: function (ms, continuing) {
        continuing = continuing || false; var self = this, t = this.options.selected; function start() { self.rotation = setInterval(function () { t = ++t < self.$tabs.length ? t : 0; self.select(t); }, ms); }
        function stop(e) { if (!e || e.clientX) { clearInterval(self.rotation); } }
        if (ms) {
            start(); if (!continuing)
                this.$tabs.bind(this.options.event, stop); else
                this.$tabs.bind(this.options.event, function () { stop(); t = self.options.selected; start(); });
        }
        else { stop(); this.$tabs.unbind(this.options.event, stop); } 
    } 
    });
})(jQuery); (function ($) {
    var PROP_NAME = 'datepicker'; function Datepicker() { this.debug = false; this._curInst = null; this._disabledInputs = []; this._datepickerShowing = false; this._inDialog = false; this._mainDivId = 'ui-datepicker-div'; this._appendClass = 'ui-datepicker-append'; this._triggerClass = 'ui-datepicker-trigger'; this._dialogClass = 'ui-datepicker-dialog'; this._promptClass = 'ui-datepicker-prompt'; this._unselectableClass = 'ui-datepicker-unselectable'; this._currentClass = 'ui-datepicker-current-day'; this.regional = []; this.regional[''] = { clearText: 'Clear', clearStatus: 'Erase the current date', closeText: 'Close', closeStatus: 'Close without change', prevText: '&#x3c;Prev', prevStatus: 'Show the previous month', nextText: 'Next&#x3e;', nextStatus: 'Show the next month', currentText: 'Today', currentStatus: 'Show the current month', monthNames: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'], monthNamesShort: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'], monthStatus: 'Show a different month', yearStatus: 'Show a different year', weekHeader: 'Wk', weekStatus: 'Week of the year', dayNames: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'], dayNamesShort: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'], dayNamesMin: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'], dayStatus: 'Set DD as first week day', dateStatus: 'Select DD, M d', dateFormat: 'mm/dd/yy', firstDay: 0, initStatus: 'Select a date', isRTL: false }; this._defaults = { showOn: 'focus', showAnim: 'show', showOptions: {}, defaultDate: null, appendText: '', buttonText: '...', buttonImage: '', buttonImageOnly: false, closeAtTop: true, mandatory: false, hideIfNoPrevNext: false, navigationAsDateFormat: false, gotoCurrent: false, changeMonth: true, changeYear: true, yearRange: '-10:+10', changeFirstDay: true, highlightWeek: false, showOtherMonths: false, showWeeks: false, calculateWeek: this.iso8601Week, shortYearCutoff: '+10', showStatus: false, statusForDate: this.dateStatus, minDate: null, maxDate: null, duration: 'normal', beforeShowDay: null, beforeShow: null, onSelect: null, onChangeMonthYear: null, onClose: null, numberOfMonths: 1, stepMonths: 1, rangeSelect: false, rangeSeparator: ' - ', altField: '', altFormat: '' }; $.extend(this._defaults, this.regional['']); this.dpDiv = $('<div id="' + this._mainDivId + '" style="display: none;"></div>'); }
    $.extend(Datepicker.prototype, { markerClassName: 'hasDatepicker', log: function () {
        if (this.debug)
            console.log.apply('', arguments);
    }, setDefaults: function (settings) { extendRemove(this._defaults, settings || {}); return this; }, _attachDatepicker: function (target, settings) {
        var inlineSettings = null; for (attrName in this._defaults) { var attrValue = target.getAttribute('date:' + attrName); if (attrValue) { inlineSettings = inlineSettings || {}; try { inlineSettings[attrName] = eval(attrValue); } catch (err) { inlineSettings[attrName] = attrValue; } } }
        var nodeName = target.nodeName.toLowerCase(); var inline = (nodeName == 'div' || nodeName == 'span'); if (!target.id)
            target.id = 'dp' + new Date().getTime(); var inst = this._newInst($(target), inline); inst.settings = $.extend({}, settings || {}, inlineSettings || {}); if (nodeName == 'input') { this._connectDatepicker(target, inst); } else if (inline) { this._inlineDatepicker(target, inst); } 
    }, _newInst: function (target, inline) { return { id: target[0].id, input: target, selectedDay: 0, selectedMonth: 0, selectedYear: 0, drawMonth: 0, drawYear: 0, inline: inline, dpDiv: (!inline ? this.dpDiv : $('<div class="ui-datepicker-inline"></div>')) }; }, _connectDatepicker: function (target, inst) {
        var input = $(target); if (input.hasClass(this.markerClassName))
            return; var appendText = this._get(inst, 'appendText'); var isRTL = this._get(inst, 'isRTL'); if (appendText)
            input[isRTL ? 'before' : 'after']('<span class="' + this._appendClass + '">' + appendText + '</span>'); var showOn = this._get(inst, 'showOn'); if (showOn == 'focus' || showOn == 'both')
            input.focus(this._showDatepicker); if (showOn == 'button' || showOn == 'both') {
            var buttonText = this._get(inst, 'buttonText'); var buttonImage = this._get(inst, 'buttonImage'); var trigger = $(this._get(inst, 'buttonImageOnly') ? $('<img/>').addClass(this._triggerClass).attr({ src: buttonImage, alt: buttonText, title: buttonText }) : $('<button type="button"></button>').addClass(this._triggerClass).html(buttonImage == '' ? buttonText : $('<img/>').attr({ src: buttonImage, alt: buttonText, title: buttonText }))); input[isRTL ? 'before' : 'after'](trigger); trigger.click(function () {
                if ($.datepicker._datepickerShowing && $.datepicker._lastInput == target)
                    $.datepicker._hideDatepicker(); else
                    $.datepicker._showDatepicker(target); return false;
            });
        }
        input.addClass(this.markerClassName).keydown(this._doKeyDown).keypress(this._doKeyPress).bind("setData.datepicker", function (event, key, value) { inst.settings[key] = value; }).bind("getData.datepicker", function (event, key) { return this._get(inst, key); }); $.data(target, PROP_NAME, inst);
    }, _inlineDatepicker: function (target, inst) {
        var input = $(target); if (input.hasClass(this.markerClassName))
            return; input.addClass(this.markerClassName).append(inst.dpDiv).bind("setData.datepicker", function (event, key, value) { inst.settings[key] = value; }).bind("getData.datepicker", function (event, key) { return this._get(inst, key); }); $.data(target, PROP_NAME, inst); this._setDate(inst, this._getDefaultDate(inst)); this._updateDatepicker(inst);
    }, _dialogDatepicker: function (input, dateText, onSelect, settings, pos) {
        var inst = this._dialogInst; if (!inst) { var id = 'dp' + new Date().getTime(); this._dialogInput = $('<input type="text" id="' + id + '" size="1" style="position: absolute; top: -100px;"/>'); this._dialogInput.keydown(this._doKeyDown); $('body').append(this._dialogInput); inst = this._dialogInst = this._newInst(this._dialogInput, false); inst.settings = {}; $.data(this._dialogInput[0], PROP_NAME, inst); }
        extendRemove(inst.settings, settings || {}); this._dialogInput.val(dateText); this._pos = (pos ? (pos.length ? pos : [pos.pageX, pos.pageY]) : null); if (!this._pos) { var browserWidth = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth; var browserHeight = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight; var scrollX = document.documentElement.scrollLeft || document.body.scrollLeft; var scrollY = document.documentElement.scrollTop || document.body.scrollTop; this._pos = [(browserWidth / 2) - 100 + scrollX, (browserHeight / 2) - 150 + scrollY]; }
        this._dialogInput.css('left', this._pos[0] + 'px').css('top', this._pos[1] + 'px'); inst.settings.onSelect = onSelect; this._inDialog = true; this.dpDiv.addClass(this._dialogClass); this._showDatepicker(this._dialogInput[0]); if ($.blockUI)
            $.blockUI(this.dpDiv); $.data(this._dialogInput[0], PROP_NAME, inst); return this;
    }, _destroyDatepicker: function (target) {
        var nodeName = target.nodeName.toLowerCase(); var $target = $(target); $.removeData(target, PROP_NAME); if (nodeName == 'input') { $target.siblings('.' + this._appendClass).remove().end().siblings('.' + this._triggerClass).remove().end().removeClass(this.markerClassName).unbind('focus', this._showDatepicker).unbind('keydown', this._doKeyDown).unbind('keypress', this._doKeyPress); } else if (nodeName == 'div' || nodeName == 'span')
            $target.removeClass(this.markerClassName).empty();
    }, _enableDatepicker: function (target) { target.disabled = false; $(target).siblings('button.' + this._triggerClass).each(function () { this.disabled = false; }).end().siblings('img.' + this._triggerClass).css({ opacity: '1.0', cursor: '' }); this._disabledInputs = $.map(this._disabledInputs, function (value) { return (value == target ? null : value); }); }, _disableDatepicker: function (target) { target.disabled = true; $(target).siblings('button.' + this._triggerClass).each(function () { this.disabled = true; }).end().siblings('img.' + this._triggerClass).css({ opacity: '0.5', cursor: 'default' }); this._disabledInputs = $.map(this._disabledInputs, function (value) { return (value == target ? null : value); }); this._disabledInputs[this._disabledInputs.length] = target; }, _isDisabledDatepicker: function (target) {
        if (!target)
            return false; for (var i = 0; i < this._disabledInputs.length; i++) {
            if (this._disabledInputs[i] == target)
                return true;
        }
        return false;
    }, _changeDatepicker: function (target, name, value) {
        var settings = name || {}; if (typeof name == 'string') { settings = {}; settings[name] = value; }
        if (inst = $.data(target, PROP_NAME)) { extendRemove(inst.settings, settings); this._updateDatepicker(inst); } 
    }, _setDateDatepicker: function (target, date, endDate) { var inst = $.data(target, PROP_NAME); if (inst) { this._setDate(inst, date, endDate); this._updateDatepicker(inst); } }, _getDateDatepicker: function (target) {
        var inst = $.data(target, PROP_NAME); if (inst)
            this._setDateFromField(inst); return (inst ? this._getDate(inst) : null);
    }, _doKeyDown: function (e) {
        var inst = $.data(e.target, PROP_NAME); var handled = true; if ($.datepicker._datepickerShowing)
            switch (e.keyCode) { case 9: $.datepicker._hideDatepicker(null, ''); break; case 13: $.datepicker._selectDay(e.target, inst.selectedMonth, inst.selectedYear, $('td.ui-datepicker-days-cell-over', inst.dpDiv)[0]); return false; break; case 27: $.datepicker._hideDatepicker(null, $.datepicker._get(inst, 'duration')); break; case 33: $.datepicker._adjustDate(e.target, (e.ctrlKey ? -1 : -$.datepicker._get(inst, 'stepMonths')), (e.ctrlKey ? 'Y' : 'M')); break; case 34: $.datepicker._adjustDate(e.target, (e.ctrlKey ? +1 : +$.datepicker._get(inst, 'stepMonths')), (e.ctrlKey ? 'Y' : 'M')); break; case 35: if (e.ctrlKey) $.datepicker._clearDate(e.target); break; case 36: if (e.ctrlKey) $.datepicker._gotoToday(e.target); break; case 37: if (e.ctrlKey) $.datepicker._adjustDate(e.target, -1, 'D'); break; case 38: if (e.ctrlKey) $.datepicker._adjustDate(e.target, -7, 'D'); break; case 39: if (e.ctrlKey) $.datepicker._adjustDate(e.target, +1, 'D'); break; case 40: if (e.ctrlKey) $.datepicker._adjustDate(e.target, +7, 'D'); break; default: handled = false; }
        else if (e.keyCode == 36 && e.ctrlKey)
            $.datepicker._showDatepicker(this); else
            handled = false; if (handled) { e.preventDefault(); e.stopPropagation(); } 
    }, _doKeyPress: function (e) { var inst = $.data(e.target, PROP_NAME); var chars = $.datepicker._possibleChars($.datepicker._get(inst, 'dateFormat')); var chr = String.fromCharCode(e.charCode == undefined ? e.keyCode : e.charCode); return e.ctrlKey || (chr < ' ' || !chars || chars.indexOf(chr) > -1); }, _showDatepicker: function (input) {
        input = input.target || input; if (input.nodeName.toLowerCase() != 'input')
            input = $('input', input.parentNode)[0]; if ($.datepicker._isDisabledDatepicker(input) || $.datepicker._lastInput == input)
            return; var inst = $.data(input, PROP_NAME); var beforeShow = $.datepicker._get(inst, 'beforeShow'); extendRemove(inst.settings, (beforeShow ? beforeShow.apply(input, [input, inst]) : {})); $.datepicker._hideDatepicker(null, ''); $.datepicker._lastInput = input; $.datepicker._setDateFromField(inst); if ($.datepicker._inDialog)
            input.value = ''; if (!$.datepicker._pos) { $.datepicker._pos = $.datepicker._findPos(input); $.datepicker._pos[1] += input.offsetHeight; }
        var isFixed = false; $(input).parents().each(function () { isFixed |= $(this).css('position') == 'fixed'; return !isFixed; }); if (isFixed && $.browser.opera) { $.datepicker._pos[0] -= document.documentElement.scrollLeft; $.datepicker._pos[1] -= document.documentElement.scrollTop; }
        var offset = { left: $.datepicker._pos[0], top: $.datepicker._pos[1] }; $.datepicker._pos = null; inst.rangeStart = null; inst.dpDiv.css({ position: 'absolute', display: 'block', top: '-1000px' }); $.datepicker._updateDatepicker(inst); inst.dpDiv.width($.datepicker._getNumberOfMonths(inst)[1] * $('.ui-datepicker', inst.dpDiv[0])[0].offsetWidth); offset = $.datepicker._checkOffset(inst, offset, isFixed); inst.dpDiv.css({ position: ($.datepicker._inDialog && $.blockUI ? 'static' : (isFixed ? 'fixed' : 'absolute')), display: 'none', left: offset.left + 'px', top: offset.top + 'px' }); if (!inst.inline) {
            var showAnim = $.datepicker._get(inst, 'showAnim') || 'show'; var duration = $.datepicker._get(inst, 'duration'); var postProcess = function () {
                $.datepicker._datepickerShowing = true; if ($.browser.msie && parseInt($.browser.version) < 7)
                    $('iframe.ui-datepicker-cover').css({ width: inst.dpDiv.width() + 4, height: inst.dpDiv.height() + 4 });
            }; if ($.effects && $.effects[showAnim])
                inst.dpDiv.show(showAnim, $.datepicker._get(inst, 'showOptions'), duration, postProcess); else
                inst.dpDiv[showAnim](duration, postProcess); if (duration == '')
                postProcess(); if (inst.input[0].type != 'hidden')
                inst.input[0].focus(); $.datepicker._curInst = inst;
        } 
    }, _updateDatepicker: function (inst) {
        var dims = { width: inst.dpDiv.width() + 4, height: inst.dpDiv.height() + 4 }; inst.dpDiv.empty().append(this._generateDatepicker(inst)).find('iframe.ui-datepicker-cover').css({ width: dims.width, height: dims.height }); var numMonths = this._getNumberOfMonths(inst); inst.dpDiv[(numMonths[0] != 1 || numMonths[1] != 1 ? 'add' : 'remove') + 'Class']('ui-datepicker-multi'); inst.dpDiv[(this._get(inst, 'isRTL') ? 'add' : 'remove') + 'Class']('ui-datepicker-rtl'); if (inst.input && inst.input[0].type != 'hidden')
            $(inst.input[0]).focus();
    }, _checkOffset: function (inst, offset, isFixed) {
        var pos = inst.input ? this._findPos(inst.input[0]) : null; var browserWidth = window.innerWidth || document.documentElement.clientWidth; var browserHeight = window.innerHeight || document.documentElement.clientHeight; var scrollX = document.documentElement.scrollLeft || document.body.scrollLeft; var scrollY = document.documentElement.scrollTop || document.body.scrollTop; if (this._get(inst, 'isRTL') || (offset.left + inst.dpDiv.width() - scrollX) > browserWidth)
            offset.left = Math.max((isFixed ? 0 : scrollX), pos[0] + (inst.input ? inst.input.width() : 0) - (isFixed ? scrollX : 0) - inst.dpDiv.width() -
(isFixed && $.browser.opera ? document.documentElement.scrollLeft : 0)); else
            offset.left -= (isFixed ? scrollX : 0); if ((offset.top + inst.dpDiv.height() - scrollY) > browserHeight)
            offset.top = Math.max((isFixed ? 0 : scrollY), pos[1] - (isFixed ? scrollY : 0) - (this._inDialog ? 0 : inst.dpDiv.height()) -
(isFixed && $.browser.opera ? document.documentElement.scrollTop : 0)); else
            offset.top -= (isFixed ? scrollY : 0); return offset;
    }, _findPos: function (obj) {
        while (obj && (obj.type == 'hidden' || obj.nodeType != 1)) { obj = obj.nextSibling; }
        var position = $(obj).offset(); return [position.left, position.top];
    }, _hideDatepicker: function (input, duration) {
        var inst = this._curInst; if (!inst)
            return; var rangeSelect = this._get(inst, 'rangeSelect'); if (rangeSelect && this._stayOpen)
            this._selectDate('#' + inst.id, this._formatDate(inst, inst.currentDay, inst.currentMonth, inst.currentYear)); this._stayOpen = false; if (this._datepickerShowing) {
            duration = (duration != null ? duration : this._get(inst, 'duration')); var showAnim = this._get(inst, 'showAnim'); var postProcess = function () { $.datepicker._tidyDialog(inst); }; if (duration != '' && $.effects && $.effects[showAnim])
                inst.dpDiv.hide(showAnim, $.datepicker._get(inst, 'showOptions'), duration, postProcess); else
                inst.dpDiv[(duration == '' ? 'hide' : (showAnim == 'slideDown' ? 'slideUp' : (showAnim == 'fadeIn' ? 'fadeOut' : 'hide')))](duration, postProcess); if (duration == '')
                this._tidyDialog(inst); var onClose = this._get(inst, 'onClose'); if (onClose)
                onClose.apply((inst.input ? inst.input[0] : null), [this._getDate(inst), inst]); this._datepickerShowing = false; this._lastInput = null; inst.settings.prompt = null; if (this._inDialog) { this._dialogInput.css({ position: 'absolute', left: '0', top: '-100px' }); if ($.blockUI) { $.unblockUI(); $('body').append(this.dpDiv); } }
            this._inDialog = false;
        }
        this._curInst = null;
    }, _tidyDialog: function (inst) { inst.dpDiv.removeClass(this._dialogClass).unbind('.ui-datepicker'); $('.' + this._promptClass, inst.dpDiv).remove(); }, _checkExternalClick: function (event) {
        if (!$.datepicker._curInst)
            return; var $target = $(event.target); if (($target.parents('#' + $.datepicker._mainDivId).length == 0) && !$target.hasClass($.datepicker.markerClassName) && !$target.hasClass($.datepicker._triggerClass) && $.datepicker._datepickerShowing && !($.datepicker._inDialog && $.blockUI))
            $.datepicker._hideDatepicker(null, '');
    }, _adjustDate: function (id, offset, period) { var target = $(id); var inst = $.data(target[0], PROP_NAME); this._adjustInstDate(inst, offset, period); this._updateDatepicker(inst); }, _gotoToday: function (id) {
        var target = $(id); var inst = $.data(target[0], PROP_NAME); if (this._get(inst, 'gotoCurrent') && inst.currentDay) { inst.selectedDay = inst.currentDay; inst.drawMonth = inst.selectedMonth = inst.currentMonth; inst.drawYear = inst.selectedYear = inst.currentYear; }
        else { var date = new Date(); inst.selectedDay = date.getDate(); inst.drawMonth = inst.selectedMonth = date.getMonth(); inst.drawYear = inst.selectedYear = date.getFullYear(); }
        this._adjustDate(target); this._notifyChange(inst);
    }, _selectMonthYear: function (id, select, period) { var target = $(id); var inst = $.data(target[0], PROP_NAME); inst._selectingMonthYear = false; inst[period == 'M' ? 'drawMonth' : 'drawYear'] = select.options[select.selectedIndex].value - 0; this._adjustDate(target); this._notifyChange(inst); }, _clickMonthYear: function (id) {
        var target = $(id); var inst = $.data(target[0], PROP_NAME); if (inst.input && inst._selectingMonthYear && !$.browser.msie)
            inst.input[0].focus(); inst._selectingMonthYear = !inst._selectingMonthYear;
    }, _changeFirstDay: function (id, day) { var target = $(id); var inst = $.data(target[0], PROP_NAME); inst.settings.firstDay = day; this._updateDatepicker(inst); }, _selectDay: function (id, month, year, td) {
        if ($(td).hasClass(this._unselectableClass))
            return; var target = $(id); var inst = $.data(target[0], PROP_NAME); var rangeSelect = this._get(inst, 'rangeSelect'); if (rangeSelect) { this._stayOpen = !this._stayOpen; if (this._stayOpen) { $('.ui-datepicker td').removeClass(this._currentClass); $(td).addClass(this._currentClass); } }
        inst.selectedDay = inst.currentDay = $('a', td).html(); inst.selectedMonth = inst.currentMonth = month; inst.selectedYear = inst.currentYear = year; if (this._stayOpen) { inst.endDay = inst.endMonth = inst.endYear = null; }
        else if (rangeSelect) { inst.endDay = inst.currentDay; inst.endMonth = inst.currentMonth; inst.endYear = inst.currentYear; }
        this._selectDate(id, this._formatDate(inst, inst.currentDay, inst.currentMonth, inst.currentYear)); if (this._stayOpen) { inst.rangeStart = this._daylightSavingAdjust(new Date(inst.currentYear, inst.currentMonth, inst.currentDay)); this._updateDatepicker(inst); }
        else if (rangeSelect) {
            inst.selectedDay = inst.currentDay = inst.rangeStart.getDate(); inst.selectedMonth = inst.currentMonth = inst.rangeStart.getMonth(); inst.selectedYear = inst.currentYear = inst.rangeStart.getFullYear(); inst.rangeStart = null; if (inst.inline)
                this._updateDatepicker(inst);
        } 
    }, _clearDate: function (id) {
        var target = $(id); var inst = $.data(target[0], PROP_NAME); if (this._get(inst, 'mandatory'))
            return; this._stayOpen = false; inst.endDay = inst.endMonth = inst.endYear = inst.rangeStart = null; this._selectDate(target, '');
    }, _selectDate: function (id, dateStr) {
        var target = $(id); var inst = $.data(target[0], PROP_NAME); dateStr = (dateStr != null ? dateStr : this._formatDate(inst)); if (this._get(inst, 'rangeSelect') && dateStr)
            dateStr = (inst.rangeStart ? this._formatDate(inst, inst.rangeStart) : dateStr) + this._get(inst, 'rangeSeparator') + dateStr; if (inst.input)
            inst.input.val(dateStr); this._updateAlternate(inst); var onSelect = this._get(inst, 'onSelect'); if (onSelect)
            onSelect.apply((inst.input ? inst.input[0] : null), [dateStr, inst]); else if (inst.input)
            inst.input.trigger('change'); if (inst.inline)
            this._updateDatepicker(inst); else if (!this._stayOpen) {
            this._hideDatepicker(null, this._get(inst, 'duration')); this._lastInput = inst.input[0]; if (typeof (inst.input[0]) != 'object')
                inst.input[0].focus(); this._lastInput = null;
        } 
    }, _updateAlternate: function (inst) {
        var altField = this._get(inst, 'altField'); if (altField) {
            var altFormat = this._get(inst, 'altFormat'); var date = this._getDate(inst); dateStr = (isArray(date) ? (!date[0] && !date[1] ? '' : this.formatDate(altFormat, date[0], this._getFormatConfig(inst)) +
this._get(inst, 'rangeSeparator') + this.formatDate(altFormat, date[1] || date[0], this._getFormatConfig(inst))) : this.formatDate(altFormat, date, this._getFormatConfig(inst))); $(altField).each(function () { $(this).val(dateStr); });
        } 
    }, noWeekends: function (date) { var day = date.getDay(); return [(day > 0 && day < 6), '']; }, iso8601Week: function (date) {
        var checkDate = new Date(date.getFullYear(), date.getMonth(), date.getDate()); var firstMon = new Date(checkDate.getFullYear(), 1 - 1, 4); var firstDay = firstMon.getDay() || 7; firstMon.setDate(firstMon.getDate() + 1 - firstDay); if (firstDay < 4 && checkDate < firstMon) { checkDate.setDate(checkDate.getDate() - 3); return $.datepicker.iso8601Week(checkDate); } else if (checkDate > new Date(checkDate.getFullYear(), 12 - 1, 28)) { firstDay = new Date(checkDate.getFullYear() + 1, 1 - 1, 4).getDay() || 7; if (firstDay > 4 && (checkDate.getDay() || 7) < firstDay - 3) { checkDate.setDate(checkDate.getDate() + 3); return $.datepicker.iso8601Week(checkDate); } }
        return Math.floor(((checkDate - firstMon) / 86400000) / 7) + 1;
    }, dateStatus: function (date, inst) { return $.datepicker.formatDate($.datepicker._get(inst, 'dateStatus'), date, $.datepicker._getFormatConfig(inst)); }, parseDate: function (format, value, settings) {
        if (format == null || value == null)
            throw 'Invalid arguments'; value = (typeof value == 'object' ? value.toString() : value + ''); if (value == '')
            return null; var shortYearCutoff = (settings ? settings.shortYearCutoff : null) || this._defaults.shortYearCutoff; var dayNamesShort = (settings ? settings.dayNamesShort : null) || this._defaults.dayNamesShort; var dayNames = (settings ? settings.dayNames : null) || this._defaults.dayNames; var monthNamesShort = (settings ? settings.monthNamesShort : null) || this._defaults.monthNamesShort; var monthNames = (settings ? settings.monthNames : null) || this._defaults.monthNames; var year = -1; var month = -1; var day = -1; var literal = false; var lookAhead = function (match) {
                var matches = (iFormat + 1 < format.length && format.charAt(iFormat + 1) == match); if (matches)
                    iFormat++; return matches;
            }; var getNumber = function (match) {
                lookAhead(match); var origSize = (match == '@' ? 14 : (match == 'y' ? 4 : 2)); var size = origSize; var num = 0; while (size > 0 && iValue < value.length && value.charAt(iValue) >= '0' && value.charAt(iValue) <= '9') { num = num * 10 + (value.charAt(iValue++) - 0); size--; }
                if (size == origSize)
                    throw 'Missing number at position ' + iValue; return num;
            }; var getName = function (match, shortNames, longNames) {
                var names = (lookAhead(match) ? longNames : shortNames); var size = 0; for (var j = 0; j < names.length; j++)
                    size = Math.max(size, names[j].length); var name = ''; var iInit = iValue; while (size > 0 && iValue < value.length) {
                    name += value.charAt(iValue++); for (var i = 0; i < names.length; i++)
                        if (name == names[i])
                            return i + 1; size--;
                }
                throw 'Unknown name at position ' + iInit;
            }; var checkLiteral = function () {
                if (value.charAt(iValue) != format.charAt(iFormat))
                    throw 'Unexpected literal at position ' + iValue; iValue++;
            }; var iValue = 0; for (var iFormat = 0; iFormat < format.length; iFormat++) {
            if (literal)
                if (format.charAt(iFormat) == "'" && !lookAhead("'"))
                    literal = false; else
                    checkLiteral(); else
                switch (format.charAt(iFormat)) {
                case 'd': day = getNumber('d'); break; case 'D': getName('D', dayNamesShort, dayNames); break; case 'm': month = getNumber('m'); break; case 'M': month = getName('M', monthNamesShort, monthNames); break; case 'y': year = getNumber('y'); break; case '@': var date = new Date(getNumber('@')); year = date.getFullYear(); month = date.getMonth() + 1; day = date.getDate(); break; case "'": if (lookAhead("'"))
                        checkLiteral(); else
                        literal = true; break; default: checkLiteral();
            } 
        }
        if (year < 100)
            year += new Date().getFullYear() - new Date().getFullYear() % 100 +
(year <= shortYearCutoff ? 0 : -100); var date = this._daylightSavingAdjust(new Date(year, month - 1, day)); if (date.getFullYear() != year || date.getMonth() + 1 != month || date.getDate() != day)
            throw 'Invalid date'; return date;
    }, ATOM: 'yy-mm-dd', COOKIE: 'D, dd M yy', ISO_8601: 'yy-mm-dd', RFC_822: 'D, d M y', RFC_850: 'DD, dd-M-y', RFC_1036: 'D, d M y', RFC_1123: 'D, d M yy', RFC_2822: 'D, d M yy', RSS: 'D, d M y', TIMESTAMP: '@', W3C: 'yy-mm-dd', formatDate: function (format, date, settings) {
        if (!date)
            return ''; var dayNamesShort = (settings ? settings.dayNamesShort : null) || this._defaults.dayNamesShort; var dayNames = (settings ? settings.dayNames : null) || this._defaults.dayNames; var monthNamesShort = (settings ? settings.monthNamesShort : null) || this._defaults.monthNamesShort; var monthNames = (settings ? settings.monthNames : null) || this._defaults.monthNames; var lookAhead = function (match) {
                var matches = (iFormat + 1 < format.length && format.charAt(iFormat + 1) == match); if (matches)
                    iFormat++; return matches;
            }; var formatNumber = function (match, value) { return (lookAhead(match) && value < 10 ? '0' : '') + value; }; var formatName = function (match, value, shortNames, longNames) { return (lookAhead(match) ? longNames[value] : shortNames[value]); }; var output = ''; var literal = false; if (date)
            for (var iFormat = 0; iFormat < format.length; iFormat++) {
                if (literal)
                    if (format.charAt(iFormat) == "'" && !lookAhead("'"))
                        literal = false; else
                        output += format.charAt(iFormat); else
                    switch (format.charAt(iFormat)) {
                    case 'd': output += formatNumber('d', date.getDate()); break; case 'D': output += formatName('D', date.getDay(), dayNamesShort, dayNames); break; case 'm': output += formatNumber('m', date.getMonth() + 1); break; case 'M': output += formatName('M', date.getMonth(), monthNamesShort, monthNames); break; case 'y': output += (lookAhead('y') ? date.getFullYear() : (date.getYear() % 100 < 10 ? '0' : '') + date.getYear() % 100); break; case '@': output += date.getTime(); break; case "'": if (lookAhead("'"))
                            output += "'"; else
                            literal = true; break; default: output += format.charAt(iFormat);
                } 
            }
        return output;
    }, _possibleChars: function (format) {
        var chars = ''; var literal = false; for (var iFormat = 0; iFormat < format.length; iFormat++)
            if (literal)
                if (format.charAt(iFormat) == "'" && !lookAhead("'"))
                    literal = false; else
                    chars += format.charAt(iFormat); else
                switch (format.charAt(iFormat)) {
                case 'd': case 'm': case 'y': case '@': chars += '0123456789'; break; case 'D': case 'M': return null; case "'": if (lookAhead("'"))
                        chars += "'"; else
                        literal = true; break; default: chars += format.charAt(iFormat);
            }
        return chars;
    }, _get: function (inst, name) { return inst.settings[name] !== undefined ? inst.settings[name] : this._defaults[name]; }, _setDateFromField: function (inst) {
        var dateFormat = this._get(inst, 'dateFormat'); var dates = inst.input ? inst.input.val().split(this._get(inst, 'rangeSeparator')) : null; inst.endDay = inst.endMonth = inst.endYear = null; var date = defaultDate = this._getDefaultDate(inst); if (dates.length > 0) {
            var settings = this._getFormatConfig(inst); if (dates.length > 1) { date = this.parseDate(dateFormat, dates[1], settings) || defaultDate; inst.endDay = date.getDate(); inst.endMonth = date.getMonth(); inst.endYear = date.getFullYear(); }
            try { date = this.parseDate(dateFormat, dates[0], settings) || defaultDate; } catch (e) { this.log(e); date = defaultDate; } 
        }
        inst.selectedDay = date.getDate(); inst.drawMonth = inst.selectedMonth = date.getMonth(); inst.drawYear = inst.selectedYear = date.getFullYear(); inst.currentDay = (dates[0] ? date.getDate() : 0); inst.currentMonth = (dates[0] ? date.getMonth() : 0); inst.currentYear = (dates[0] ? date.getFullYear() : 0); this._adjustInstDate(inst);
    }, _getDefaultDate: function (inst) { var date = this._determineDate(this._get(inst, 'defaultDate'), new Date()); var minDate = this._getMinMaxDate(inst, 'min', true); var maxDate = this._getMinMaxDate(inst, 'max'); date = (minDate && date < minDate ? minDate : date); date = (maxDate && date > maxDate ? maxDate : date); return date; }, _determineDate: function (date, defaultDate) {
        var offsetNumeric = function (offset) { var date = new Date(); date.setDate(date.getDate() + offset); return date; }; var offsetString = function (offset, getDaysInMonth) {
            var date = new Date(); var year = date.getFullYear(); var month = date.getMonth(); var day = date.getDate(); var pattern = /([+-]?[0-9]+)\s*(d|D|w|W|m|M|y|Y)?/g; var matches = pattern.exec(offset); while (matches) {
                switch (matches[2] || 'd') { case 'd': case 'D': day += (matches[1] - 0); break; case 'w': case 'W': day += (matches[1] * 7); break; case 'm': case 'M': month += (matches[1] - 0); day = Math.min(day, getDaysInMonth(year, month)); break; case 'y': case 'Y': year += (matches[1] - 0); day = Math.min(day, getDaysInMonth(year, month)); break; }
                matches = pattern.exec(offset);
            }
            return new Date(year, month, day);
        }; date = (date == null ? defaultDate : (typeof date == 'string' ? offsetString(date, this._getDaysInMonth) : (typeof date == 'number' ? (isNaN(date) ? defaultDate : offsetNumeric(date)) : date))); date = (date && date.toString() == 'Invalid Date' ? defaultDate : date); if (date) { date.setHours(0); date.setMinutes(0); date.setSeconds(0); date.setMilliseconds(0); }
        return this._daylightSavingAdjust(date);
    }, _daylightSavingAdjust: function (date) { if (!date) return null; date.setHours(date.getHours() > 12 ? date.getHours() + 2 : 0); return date; }, _setDate: function (inst, date, endDate) {
        var clear = !(date); date = this._determineDate(date, new Date()); inst.selectedDay = inst.currentDay = date.getDate(); inst.drawMonth = inst.selectedMonth = inst.currentMonth = date.getMonth(); inst.drawYear = inst.selectedYear = inst.currentYear = date.getFullYear(); if (this._get(inst, 'rangeSelect')) { if (endDate) { endDate = this._determineDate(endDate, null); inst.endDay = endDate.getDate(); inst.endMonth = endDate.getMonth(); inst.endYear = endDate.getFullYear(); } else { inst.endDay = inst.currentDay; inst.endMonth = inst.currentMonth; inst.endYear = inst.currentYear; } }
        this._adjustInstDate(inst); if (inst.input)
            inst.input.val(clear ? '' : this._formatDate(inst) +
(!this._get(inst, 'rangeSelect') ? '' : this._get(inst, 'rangeSeparator') +
this._formatDate(inst, inst.endDay, inst.endMonth, inst.endYear)));
    }, _getDate: function (inst) {
        var startDate = (!inst.currentYear || (inst.input && inst.input.val() == '') ? null : this._daylightSavingAdjust(new Date(inst.currentYear, inst.currentMonth, inst.currentDay))); if (this._get(inst, 'rangeSelect')) { return [inst.rangeStart || startDate, (!inst.endYear ? inst.rangeStart || startDate : this._daylightSavingAdjust(new Date(inst.endYear, inst.endMonth, inst.endDay)))]; } else
            return startDate;
    }, _generateDatepicker: function (inst) {
        var today = new Date(); today = this._daylightSavingAdjust(new Date(today.getFullYear(), today.getMonth(), today.getDate())); var showStatus = this._get(inst, 'showStatus'); var isRTL = this._get(inst, 'isRTL'); var clear = (this._get(inst, 'mandatory') ? '' : '<div class="ui-datepicker-clear"><a onclick="jQuery.datepicker._clearDate(\'#' + inst.id + '\');"' +
(showStatus ? this._addStatus(inst, this._get(inst, 'clearStatus') || '&#xa0;') : '') + '>' +
this._get(inst, 'clearText') + '</a></div>'); var controls = '<div class="ui-datepicker-control">' + (isRTL ? '' : clear) + '<div class="ui-datepicker-close"><a onclick="jQuery.datepicker._hideDatepicker();"' +
(showStatus ? this._addStatus(inst, this._get(inst, 'closeStatus') || '&#xa0;') : '') + '>' +
this._get(inst, 'closeText') + '</a></div>' + (isRTL ? clear : '') + '</div>'; var prompt = this._get(inst, 'prompt'); var closeAtTop = this._get(inst, 'closeAtTop'); var hideIfNoPrevNext = this._get(inst, 'hideIfNoPrevNext'); var navigationAsDateFormat = this._get(inst, 'navigationAsDateFormat'); var numMonths = this._getNumberOfMonths(inst); var stepMonths = this._get(inst, 'stepMonths'); var isMultiMonth = (numMonths[0] != 1 || numMonths[1] != 1); var currentDate = this._daylightSavingAdjust((!inst.currentDay ? new Date(9999, 9, 9) : new Date(inst.currentYear, inst.currentMonth, inst.currentDay))); var minDate = this._getMinMaxDate(inst, 'min', true); var maxDate = this._getMinMaxDate(inst, 'max'); var drawMonth = inst.drawMonth; var drawYear = inst.drawYear; if (maxDate) { var maxDraw = this._daylightSavingAdjust(new Date(maxDate.getFullYear(), maxDate.getMonth() - numMonths[1] + 1, maxDate.getDate())); maxDraw = (minDate && maxDraw < minDate ? minDate : maxDraw); while (this._daylightSavingAdjust(new Date(drawYear, drawMonth, 1)) > maxDraw) { drawMonth--; if (drawMonth < 0) { drawMonth = 11; drawYear--; } } }
        var prevText = this._get(inst, 'prevText'); prevText = (!navigationAsDateFormat ? prevText : this.formatDate(prevText, this._daylightSavingAdjust(new Date(drawYear, drawMonth - stepMonths, 1)), this._getFormatConfig(inst))); var prev = '<div class="ui-datepicker-prev">' + (this._canAdjustMonth(inst, -1, drawYear, drawMonth) ? '<a onclick="jQuery.datepicker._adjustDate(\'#' + inst.id + '\', -' + stepMonths + ', \'M\');"' +
(showStatus ? this._addStatus(inst, this._get(inst, 'prevStatus') || '&#xa0;') : '') + '>' + prevText + '</a>' : (hideIfNoPrevNext ? '' : '<label>' + prevText + '</label>')) + '</div>'; var nextText = this._get(inst, 'nextText'); nextText = (!navigationAsDateFormat ? nextText : this.formatDate(nextText, this._daylightSavingAdjust(new Date(drawYear, drawMonth + stepMonths, 1)), this._getFormatConfig(inst))); var next = '<div class="ui-datepicker-next">' + (this._canAdjustMonth(inst, +1, drawYear, drawMonth) ? '<a onclick="jQuery.datepicker._adjustDate(\'#' + inst.id + '\', +' + stepMonths + ', \'M\');"' +
(showStatus ? this._addStatus(inst, this._get(inst, 'nextStatus') || '&#xa0;') : '') + '>' + nextText + '</a>' : (hideIfNoPrevNext ? '' : '<label>' + nextText + '</label>')) + '</div>'; var currentText = this._get(inst, 'currentText'); currentText = (!navigationAsDateFormat ? currentText : this.formatDate(currentText, today, this._getFormatConfig(inst))); var html = (prompt ? '<div class="' + this._promptClass + '">' + prompt + '</div>' : '') +
(closeAtTop && !inst.inline ? controls : '') + '<div class="ui-datepicker-links">' + (isRTL ? next : prev) +
(this._isInRange(inst, (this._get(inst, 'gotoCurrent') && inst.currentDay ? currentDate : today)) ? '<div class="ui-datepicker-current">' + '<a onclick="jQuery.datepicker._gotoToday(\'#' + inst.id + '\');"' +
(showStatus ? this._addStatus(inst, this._get(inst, 'currentStatus') || '&#xa0;') : '') + '>' +
currentText + '</a></div>' : '') + (isRTL ? prev : next) + '</div>'; var firstDay = this._get(inst, 'firstDay'); var changeFirstDay = this._get(inst, 'changeFirstDay'); var dayNames = this._get(inst, 'dayNames'); var dayNamesShort = this._get(inst, 'dayNamesShort'); var dayNamesMin = this._get(inst, 'dayNamesMin'); var monthNames = this._get(inst, 'monthNames'); var beforeShowDay = this._get(inst, 'beforeShowDay'); var highlightWeek = this._get(inst, 'highlightWeek'); var showOtherMonths = this._get(inst, 'showOtherMonths'); var showWeeks = this._get(inst, 'showWeeks'); var calculateWeek = this._get(inst, 'calculateWeek') || this.iso8601Week; var status = (showStatus ? this._get(inst, 'dayStatus') || '&#xa0;' : ''); var dateStatus = this._get(inst, 'statusForDate') || this.dateStatus; var endDate = inst.endDay ? this._daylightSavingAdjust(new Date(inst.endYear, inst.endMonth, inst.endDay)) : currentDate; for (var row = 0; row < numMonths[0]; row++)
            for (var col = 0; col < numMonths[1]; col++) {
                var selectedDate = this._daylightSavingAdjust(new Date(drawYear, drawMonth, inst.selectedDay)); html += '<div class="ui-datepicker-one-month' + (col == 0 ? ' ui-datepicker-new-row' : '') + '">' +
this._generateMonthYearHeader(inst, drawMonth, drawYear, minDate, maxDate, selectedDate, row > 0 || col > 0, showStatus, monthNames) + '<table class="ui-datepicker" cellpadding="0" cellspacing="0"><thead>' + '<tr class="ui-datepicker-title-row">' +
(showWeeks ? '<td>' + this._get(inst, 'weekHeader') + '</td>' : ''); for (var dow = 0; dow < 7; dow++) {
                    var day = (dow + firstDay) % 7; var dayStatus = (status.indexOf('DD') > -1 ? status.replace(/DD/, dayNames[day]) : status.replace(/D/, dayNamesShort[day])); html += '<td' + ((dow + firstDay + 6) % 7 >= 5 ? ' class="ui-datepicker-week-end-cell"' : '') + '>' +
(!changeFirstDay ? '<span' : '<a onclick="jQuery.datepicker._changeFirstDay(\'#' + inst.id + '\', ' + day + ');"') +
(showStatus ? this._addStatus(inst, dayStatus) : '') + ' title="' + dayNames[day] + '">' +
dayNamesMin[day] + (changeFirstDay ? '</a>' : '</span>') + '</td>';
                }
                html += '</tr></thead><tbody>'; var daysInMonth = this._getDaysInMonth(drawYear, drawMonth); if (drawYear == inst.selectedYear && drawMonth == inst.selectedMonth)
                    inst.selectedDay = Math.min(inst.selectedDay, daysInMonth); var leadDays = (this._getFirstDayOfMonth(drawYear, drawMonth) - firstDay + 7) % 7; var numRows = (isMultiMonth ? 6 : Math.ceil((leadDays + daysInMonth) / 7)); var printDate = this._daylightSavingAdjust(new Date(drawYear, drawMonth, 1 - leadDays)); for (var dRow = 0; dRow < numRows; dRow++) {
                    html += '<tr class="ui-datepicker-days-row">' +
(showWeeks ? '<td class="ui-datepicker-week-col">' + calculateWeek(printDate) + '</td>' : ''); for (var dow = 0; dow < 7; dow++) {
                        var daySettings = (beforeShowDay ? beforeShowDay.apply((inst.input ? inst.input[0] : null), [printDate]) : [true, '']); var otherMonth = (printDate.getMonth() != drawMonth); var unselectable = otherMonth || !daySettings[0] || (minDate && printDate < minDate) || (maxDate && printDate > maxDate); html += '<td class="ui-datepicker-days-cell' +
((dow + firstDay + 6) % 7 >= 5 ? ' ui-datepicker-week-end-cell' : '') +
(otherMonth ? ' ui-datepicker-otherMonth' : '') +
(printDate.getTime() == selectedDate.getTime() && drawMonth == inst.selectedMonth ? ' ui-datepicker-days-cell-over' : '') +
(unselectable ? ' ' + this._unselectableClass : '') +
(otherMonth && !showOtherMonths ? '' : ' ' + daySettings[1] +
(printDate.getTime() >= currentDate.getTime() && printDate.getTime() <= endDate.getTime() ? ' ' + this._currentClass : '') +
(printDate.getTime() == today.getTime() ? ' ui-datepicker-today' : '')) + '"' +
((!otherMonth || showOtherMonths) && daySettings[2] ? ' title="' + daySettings[2] + '"' : '') +
(unselectable ? (highlightWeek ? ' onmouseover="jQuery(this).parent().addClass(\'ui-datepicker-week-over\');"' + ' onmouseout="jQuery(this).parent().removeClass(\'ui-datepicker-week-over\');"' : '') : ' onmouseover="jQuery(this).addClass(\'ui-datepicker-days-cell-over\')' +
(highlightWeek ? '.parent().addClass(\'ui-datepicker-week-over\')' : '') + ';' +
(!showStatus || (otherMonth && !showOtherMonths) ? '' : 'jQuery(\'#ui-datepicker-status-' +
inst.id + '\').html(\'' + (dateStatus.apply((inst.input ? inst.input[0] : null), [printDate, inst]) || '&#xa0;') + '\');') + '"' + ' onmouseout="jQuery(this).removeClass(\'ui-datepicker-days-cell-over\')' +
(highlightWeek ? '.parent().removeClass(\'ui-datepicker-week-over\')' : '') + ';' +
(!showStatus || (otherMonth && !showOtherMonths) ? '' : 'jQuery(\'#ui-datepicker-status-' +
inst.id + '\').html(\'&#xa0;\');') + '" onclick="jQuery.datepicker._selectDay(\'#' +
inst.id + '\',' + drawMonth + ',' + drawYear + ', this);"') + '>' +
(otherMonth ? (showOtherMonths ? printDate.getDate() : '&#xa0;') : (unselectable ? printDate.getDate() : '<a>' + printDate.getDate() + '</a>')) + '</td>'; printDate.setDate(printDate.getDate() + 1); printDate = this._daylightSavingAdjust(printDate);
                    }
                    html += '</tr>';
                }
                drawMonth++; if (drawMonth > 11) { drawMonth = 0; drawYear++; }
                html += '</tbody></table></div>';
            }
        html += (showStatus ? '<div style="clear: both;"></div><div id="ui-datepicker-status-' + inst.id + '" class="ui-datepicker-status">' + (this._get(inst, 'initStatus') || '&#xa0;') + '</div>' : '') +
(!closeAtTop && !inst.inline ? controls : '') + '<div style="clear: both;"></div>' +
($.browser.msie && parseInt($.browser.version) < 7 && !inst.inline ? '<iframe src="javascript:false;" class="ui-datepicker-cover"></iframe>' : ''); return html;
    }, _generateMonthYearHeader: function (inst, drawMonth, drawYear, minDate, maxDate, selectedDate, secondary, showStatus, monthNames) {
        minDate = (inst.rangeStart && minDate && selectedDate < minDate ? selectedDate : minDate); var html = '<div class="ui-datepicker-header">'; if (secondary || !this._get(inst, 'changeMonth'))
            html += monthNames[drawMonth] + '&#xa0;'; else {
            var inMinYear = (minDate && minDate.getFullYear() == drawYear); var inMaxYear = (maxDate && maxDate.getFullYear() == drawYear); html += '<select class="ui-datepicker-new-month" ' + 'onchange="jQuery.datepicker._selectMonthYear(\'#' + inst.id + '\', this, \'M\');" ' + 'onclick="jQuery.datepicker._clickMonthYear(\'#' + inst.id + '\');"' +
(showStatus ? this._addStatus(inst, this._get(inst, 'monthStatus') || '&#xa0;') : '') + '>'; for (var month = 0; month < 12; month++) {
                if ((!inMinYear || month >= minDate.getMonth()) && (!inMaxYear || month <= maxDate.getMonth()))
                    html += '<option value="' + month + '"' +
(month == drawMonth ? ' selected="selected"' : '') + '>' + monthNames[month] + '</option>';
            }
            html += '</select>';
        }
        if (secondary || !this._get(inst, 'changeYear'))
            html += drawYear; else {
            var years = this._get(inst, 'yearRange').split(':'); var year = 0; var endYear = 0; if (years.length != 2) { year = drawYear - 10; endYear = drawYear + 10; } else if (years[0].charAt(0) == '+' || years[0].charAt(0) == '-') { year = endYear = new Date().getFullYear(); year += parseInt(years[0], 10); endYear += parseInt(years[1], 10); } else { year = parseInt(years[0], 10); endYear = parseInt(years[1], 10); }
            year = (minDate ? Math.max(year, minDate.getFullYear()) : year); endYear = (maxDate ? Math.min(endYear, maxDate.getFullYear()) : endYear); html += '<select class="ui-datepicker-new-year" ' + 'onchange="jQuery.datepicker._selectMonthYear(\'#' + inst.id + '\', this, \'Y\');" ' + 'onclick="jQuery.datepicker._clickMonthYear(\'#' + inst.id + '\');"' +
(showStatus ? this._addStatus(inst, this._get(inst, 'yearStatus') || '&#xa0;') : '') + '>'; for (; year <= endYear; year++) {
                html += '<option value="' + year + '"' +
(year == drawYear ? ' selected="selected"' : '') + '>' + year + '</option>';
            }
            html += '</select>';
        }
        html += '</div>'; return html;
    }, _addStatus: function (inst, text) { return ' onmouseover="jQuery(\'#ui-datepicker-status-' + inst.id + '\').html(\'' + text + '\');" ' + 'onmouseout="jQuery(\'#ui-datepicker-status-' + inst.id + '\').html(\'&#xa0;\');"'; }, _adjustInstDate: function (inst, offset, period) {
        var year = inst.drawYear + (period == 'Y' ? offset : 0); var month = inst.drawMonth + (period == 'M' ? offset : 0); var day = Math.min(inst.selectedDay, this._getDaysInMonth(year, month)) +
(period == 'D' ? offset : 0); var date = this._daylightSavingAdjust(new Date(year, month, day)); var minDate = this._getMinMaxDate(inst, 'min', true); var maxDate = this._getMinMaxDate(inst, 'max'); date = (minDate && date < minDate ? minDate : date); date = (maxDate && date > maxDate ? maxDate : date); inst.selectedDay = date.getDate(); inst.drawMonth = inst.selectedMonth = date.getMonth(); inst.drawYear = inst.selectedYear = date.getFullYear(); if (period == 'M' || period == 'Y')
            this._notifyChange(inst);
    }, _notifyChange: function (inst) {
        var onChange = this._get(inst, 'onChangeMonthYear'); if (onChange)
            onChange.apply((inst.input ? inst.input[0] : null), [new Date(inst.selectedYear, inst.selectedMonth, 1), inst]);
    }, _getNumberOfMonths: function (inst) { var numMonths = this._get(inst, 'numberOfMonths'); return (numMonths == null ? [1, 1] : (typeof numMonths == 'number' ? [1, numMonths] : numMonths)); }, _getMinMaxDate: function (inst, minMax, checkRange) { var date = this._determineDate(this._get(inst, minMax + 'Date'), null); return (!checkRange || !inst.rangeStart ? date : (!date || inst.rangeStart > date ? inst.rangeStart : date)); }, _getDaysInMonth: function (year, month) { return 32 - new Date(year, month, 32).getDate(); }, _getFirstDayOfMonth: function (year, month) { return new Date(year, month, 1).getDay(); }, _canAdjustMonth: function (inst, offset, curYear, curMonth) {
        var numMonths = this._getNumberOfMonths(inst); var date = this._daylightSavingAdjust(new Date(curYear, curMonth + (offset < 0 ? offset : numMonths[1]), 1)); if (offset < 0)
            date.setDate(this._getDaysInMonth(date.getFullYear(), date.getMonth())); return this._isInRange(inst, date);
    }, _isInRange: function (inst, date) { var newMinDate = (!inst.rangeStart ? null : this._daylightSavingAdjust(new Date(inst.selectedYear, inst.selectedMonth, inst.selectedDay))); newMinDate = (newMinDate && inst.rangeStart < newMinDate ? inst.rangeStart : newMinDate); var minDate = newMinDate || this._getMinMaxDate(inst, 'min'); var maxDate = this._getMinMaxDate(inst, 'max'); return ((!minDate || date >= minDate) && (!maxDate || date <= maxDate)); }, _getFormatConfig: function (inst) { var shortYearCutoff = this._get(inst, 'shortYearCutoff'); shortYearCutoff = (typeof shortYearCutoff != 'string' ? shortYearCutoff : new Date().getFullYear() % 100 + parseInt(shortYearCutoff, 10)); return { shortYearCutoff: shortYearCutoff, dayNamesShort: this._get(inst, 'dayNamesShort'), dayNames: this._get(inst, 'dayNames'), monthNamesShort: this._get(inst, 'monthNamesShort'), monthNames: this._get(inst, 'monthNames') }; }, _formatDate: function (inst, day, month, year) {
        if (!day) { inst.currentDay = inst.selectedDay; inst.currentMonth = inst.selectedMonth; inst.currentYear = inst.selectedYear; }
        var date = (day ? (typeof day == 'object' ? day : this._daylightSavingAdjust(new Date(year, month, day))) : this._daylightSavingAdjust(new Date(inst.currentYear, inst.currentMonth, inst.currentDay))); return this.formatDate(this._get(inst, 'dateFormat'), date, this._getFormatConfig(inst));
    } 
    }); function extendRemove(target, props) {
        $.extend(target, props); for (var name in props)
            if (props[name] == null || props[name] == undefined)
                target[name] = props[name]; return target;
    }; function isArray(a) { return (a && (($.browser.safari && typeof a == 'object' && a.length) || (a.constructor && a.constructor.toString().match(/\Array\(\)/)))); }; $.fn.datepicker = function (options) {
        var otherArgs = Array.prototype.slice.call(arguments, 1); if (typeof options == 'string' && (options == 'isDisabled' || options == 'getDate'))
            return $.datepicker['_' + options + 'Datepicker'].apply($.datepicker, [this[0]].concat(otherArgs)); return this.each(function () { typeof options == 'string' ? $.datepicker['_' + options + 'Datepicker'].apply($.datepicker, [this].concat(otherArgs)) : $.datepicker._attachDatepicker(this, options); });
    }; $.datepicker = new Datepicker(); $(document).ready(function () { $(document.body).append($.datepicker.dpDiv).mousedown($.datepicker._checkExternalClick); });
})(jQuery); ; (function ($) {
    $.effects = $.effects || {}; $.extend($.effects, { save: function (el, set) { for (var i = 0; i < set.length; i++) { if (set[i] !== null) $.data(el[0], "ec.storage." + set[i], el[0].style[set[i]]); } }, restore: function (el, set) { for (var i = 0; i < set.length; i++) { if (set[i] !== null) el.css(set[i], $.data(el[0], "ec.storage." + set[i])); } }, setMode: function (el, mode) { if (mode == 'toggle') mode = el.is(':hidden') ? 'show' : 'hide'; return mode; }, getBaseline: function (origin, original) { var y, x; switch (origin[0]) { case 'top': y = 0; break; case 'middle': y = 0.5; break; case 'bottom': y = 1; break; default: y = origin[0] / original.height; }; switch (origin[1]) { case 'left': x = 0; break; case 'center': x = 0.5; break; case 'right': x = 1; break; default: x = origin[1] / original.width; }; return { x: x, y: y }; }, createWrapper: function (el) {
        if (el.parent().attr('id') == 'fxWrapper')
            return el; var props = { width: el.outerWidth({ margin: true }), height: el.outerHeight({ margin: true }), 'float': el.css('float') }; el.wrap('<div id="fxWrapper" style="font-size:100%;background:transparent;border:none;margin:0;padding:0"></div>'); var wrapper = el.parent(); if (el.css('position') == 'static') { wrapper.css({ position: 'relative' }); el.css({ position: 'relative' }); } else { var top = el.css('top'); if (isNaN(parseInt(top))) top = 'auto'; var left = el.css('left'); if (isNaN(parseInt(left))) left = 'auto'; wrapper.css({ position: el.css('position'), top: top, left: left, zIndex: el.css('z-index') }).show(); el.css({ position: 'relative', top: 0, left: 0 }); }
        wrapper.css(props); return wrapper;
    }, removeWrapper: function (el) {
        if (el.parent().attr('id') == 'fxWrapper')
            return el.parent().replaceWith(el); return el;
    }, setTransition: function (el, list, factor, val) { val = val || {}; $.each(list, function (i, x) { unit = el.cssUnit(x); if (unit[0] > 0) val[x] = unit[0] * factor + unit[1]; }); return val; }, animateClass: function (value, duration, easing, callback) {
        var cb = (typeof easing == "function" ? easing : (callback ? callback : null)); var ea = (typeof easing == "object" ? easing : null); return this.each(function () {
            var offset = {}; var that = $(this); var oldStyleAttr = that.attr("style") || ''; if (typeof oldStyleAttr == 'object') oldStyleAttr = oldStyleAttr["cssText"]; if (value.toggle) { that.hasClass(value.toggle) ? value.remove = value.toggle : value.add = value.toggle; }
            var oldStyle = $.extend({}, (document.defaultView ? document.defaultView.getComputedStyle(this, null) : this.currentStyle)); if (value.add) that.addClass(value.add); if (value.remove) that.removeClass(value.remove); var newStyle = $.extend({}, (document.defaultView ? document.defaultView.getComputedStyle(this, null) : this.currentStyle)); if (value.add) that.removeClass(value.add); if (value.remove) that.addClass(value.remove); for (var n in newStyle) { if (typeof newStyle[n] != "function" && newStyle[n] && n.indexOf("Moz") == -1 && n.indexOf("length") == -1 && newStyle[n] != oldStyle[n] && (n.match(/color/i) || (!n.match(/color/i) && !isNaN(parseInt(newStyle[n], 10)))) && (oldStyle.position != "static" || (oldStyle.position == "static" && !n.match(/left|top|bottom|right/)))) offset[n] = newStyle[n]; }
            that.animate(offset, duration, ea, function () { if (typeof $(this).attr("style") == 'object') { $(this).attr("style")["cssText"] = ""; $(this).attr("style")["cssText"] = oldStyleAttr; } else $(this).attr("style", oldStyleAttr); if (value.add) $(this).addClass(value.add); if (value.remove) $(this).removeClass(value.remove); if (cb) cb.apply(this, arguments); });
        });
    } 
    }); $.fn.extend({ _show: $.fn.show, _hide: $.fn.hide, __toggle: $.fn.toggle, _addClass: $.fn.addClass, _removeClass: $.fn.removeClass, _toggleClass: $.fn.toggleClass, effect: function (fx, o, speed, callback) { return $.effects[fx] ? $.effects[fx].call(this, { method: fx, options: o || {}, duration: speed, callback: callback }) : null; }, show: function () {
        if (!arguments[0] || (arguments[0].constructor == Number || /(slow|normal|fast)/.test(arguments[0])))
            return this._show.apply(this, arguments); else { var o = arguments[1] || {}; o['mode'] = 'show'; return this.effect.apply(this, [arguments[0], o, arguments[2] || o.duration, arguments[3] || o.callback]); } 
    }, hide: function () {
        if (!arguments[0] || (arguments[0].constructor == Number || /(slow|normal|fast)/.test(arguments[0])))
            return this._hide.apply(this, arguments); else { var o = arguments[1] || {}; o['mode'] = 'hide'; return this.effect.apply(this, [arguments[0], o, arguments[2] || o.duration, arguments[3] || o.callback]); } 
    }, toggle: function () {
        if (!arguments[0] || (arguments[0].constructor == Number || /(slow|normal|fast)/.test(arguments[0])) || (arguments[0].constructor == Function))
            return this.__toggle.apply(this, arguments); else { var o = arguments[1] || {}; o['mode'] = 'toggle'; return this.effect.apply(this, [arguments[0], o, arguments[2] || o.duration, arguments[3] || o.callback]); } 
    }, addClass: function (classNames, speed, easing, callback) { return speed ? $.effects.animateClass.apply(this, [{ add: classNames }, speed, easing, callback]) : this._addClass(classNames); }, removeClass: function (classNames, speed, easing, callback) { return speed ? $.effects.animateClass.apply(this, [{ remove: classNames }, speed, easing, callback]) : this._removeClass(classNames); }, toggleClass: function (classNames, speed, easing, callback) { return speed ? $.effects.animateClass.apply(this, [{ toggle: classNames }, speed, easing, callback]) : this._toggleClass(classNames); }, morph: function (remove, add, speed, easing, callback) { return $.effects.animateClass.apply(this, [{ add: add, remove: remove }, speed, easing, callback]); }, switchClass: function () { return this.morph.apply(this, arguments); }, cssUnit: function (key) {
        var style = this.css(key), val = []; $.each(['em', 'px', '%', 'pt'], function (i, unit) {
            if (style.indexOf(unit) > 0)
                val = [parseFloat(style), unit];
        }); return val;
    } 
    }); jQuery.each(['backgroundColor', 'borderBottomColor', 'borderLeftColor', 'borderRightColor', 'borderTopColor', 'color', 'outlineColor'], function (i, attr) {
        jQuery.fx.step[attr] = function (fx) {
            if (fx.state == 0) { fx.start = getColor(fx.elem, attr); fx.end = getRGB(fx.end); }
            fx.elem.style[attr] = "rgb(" + [Math.max(Math.min(parseInt((fx.pos * (fx.end[0] - fx.start[0])) + fx.start[0]), 255), 0), Math.max(Math.min(parseInt((fx.pos * (fx.end[1] - fx.start[1])) + fx.start[1]), 255), 0), Math.max(Math.min(parseInt((fx.pos * (fx.end[2] - fx.start[2])) + fx.start[2]), 255), 0)].join(",") + ")";
        } 
    }); function getRGB(color) {
        var result; if (color && color.constructor == Array && color.length == 3)
            return color; if (result = /rgb\(\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*\)/.exec(color))
            return [parseInt(result[1]), parseInt(result[2]), parseInt(result[3])]; if (result = /rgb\(\s*([0-9]+(?:\.[0-9]+)?)\%\s*,\s*([0-9]+(?:\.[0-9]+)?)\%\s*,\s*([0-9]+(?:\.[0-9]+)?)\%\s*\)/.exec(color))
            return [parseFloat(result[1]) * 2.55, parseFloat(result[2]) * 2.55, parseFloat(result[3]) * 2.55]; if (result = /#([a-fA-F0-9]{2})([a-fA-F0-9]{2})([a-fA-F0-9]{2})/.exec(color))
            return [parseInt(result[1], 16), parseInt(result[2], 16), parseInt(result[3], 16)]; if (result = /#([a-fA-F0-9])([a-fA-F0-9])([a-fA-F0-9])/.exec(color))
            return [parseInt(result[1] + result[1], 16), parseInt(result[2] + result[2], 16), parseInt(result[3] + result[3], 16)]; if (result = /rgba\(0, 0, 0, 0\)/.exec(color))
            return colors['transparent']
        return colors[jQuery.trim(color).toLowerCase()];
    }
    function getColor(elem, attr) {
        var color; do {
            color = jQuery.curCSS(elem, attr); if (color != '' && color != 'transparent' || jQuery.nodeName(elem, "body"))
                break; attr = "backgroundColor";
        } while (elem = elem.parentNode); return getRGB(color);
    }; var colors = { aqua: [0, 255, 255], azure: [240, 255, 255], beige: [245, 245, 220], black: [0, 0, 0], blue: [0, 0, 255], brown: [165, 42, 42], cyan: [0, 255, 255], darkblue: [0, 0, 139], darkcyan: [0, 139, 139], darkgrey: [169, 169, 169], darkgreen: [0, 100, 0], darkkhaki: [189, 183, 107], darkmagenta: [139, 0, 139], darkolivegreen: [85, 107, 47], darkorange: [255, 140, 0], darkorchid: [153, 50, 204], darkred: [139, 0, 0], darksalmon: [233, 150, 122], darkviolet: [148, 0, 211], fuchsia: [255, 0, 255], gold: [255, 215, 0], green: [0, 128, 0], indigo: [75, 0, 130], khaki: [240, 230, 140], lightblue: [173, 216, 230], lightcyan: [224, 255, 255], lightgreen: [144, 238, 144], lightgrey: [211, 211, 211], lightpink: [255, 182, 193], lightyellow: [255, 255, 224], lime: [0, 255, 0], magenta: [255, 0, 255], maroon: [128, 0, 0], navy: [0, 0, 128], olive: [128, 128, 0], orange: [255, 165, 0], pink: [255, 192, 203], purple: [128, 0, 128], violet: [128, 0, 128], red: [255, 0, 0], silver: [192, 192, 192], white: [255, 255, 255], yellow: [255, 255, 0], transparent: [255, 255, 255] }; jQuery.easing['jswing'] = jQuery.easing['swing']; jQuery.extend(jQuery.easing, { def: 'easeOutQuad', swing: function (x, t, b, c, d) { return jQuery.easing[jQuery.easing.def](x, t, b, c, d); }, easeInQuad: function (x, t, b, c, d) { return c * (t /= d) * t + b; }, easeOutQuad: function (x, t, b, c, d) { return -c * (t /= d) * (t - 2) + b; }, easeInOutQuad: function (x, t, b, c, d) { if ((t /= d / 2) < 1) return c / 2 * t * t + b; return -c / 2 * ((--t) * (t - 2) - 1) + b; }, easeInCubic: function (x, t, b, c, d) { return c * (t /= d) * t * t + b; }, easeOutCubic: function (x, t, b, c, d) { return c * ((t = t / d - 1) * t * t + 1) + b; }, easeInOutCubic: function (x, t, b, c, d) { if ((t /= d / 2) < 1) return c / 2 * t * t * t + b; return c / 2 * ((t -= 2) * t * t + 2) + b; }, easeInQuart: function (x, t, b, c, d) { return c * (t /= d) * t * t * t + b; }, easeOutQuart: function (x, t, b, c, d) { return -c * ((t = t / d - 1) * t * t * t - 1) + b; }, easeInOutQuart: function (x, t, b, c, d) { if ((t /= d / 2) < 1) return c / 2 * t * t * t * t + b; return -c / 2 * ((t -= 2) * t * t * t - 2) + b; }, easeInQuint: function (x, t, b, c, d) { return c * (t /= d) * t * t * t * t + b; }, easeOutQuint: function (x, t, b, c, d) { return c * ((t = t / d - 1) * t * t * t * t + 1) + b; }, easeInOutQuint: function (x, t, b, c, d) { if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b; return c / 2 * ((t -= 2) * t * t * t * t + 2) + b; }, easeInSine: function (x, t, b, c, d) { return -c * Math.cos(t / d * (Math.PI / 2)) + c + b; }, easeOutSine: function (x, t, b, c, d) { return c * Math.sin(t / d * (Math.PI / 2)) + b; }, easeInOutSine: function (x, t, b, c, d) { return -c / 2 * (Math.cos(Math.PI * t / d) - 1) + b; }, easeInExpo: function (x, t, b, c, d) { return (t == 0) ? b : c * Math.pow(2, 10 * (t / d - 1)) + b; }, easeOutExpo: function (x, t, b, c, d) { return (t == d) ? b + c : c * (-Math.pow(2, -10 * t / d) + 1) + b; }, easeInOutExpo: function (x, t, b, c, d) { if (t == 0) return b; if (t == d) return b + c; if ((t /= d / 2) < 1) return c / 2 * Math.pow(2, 10 * (t - 1)) + b; return c / 2 * (-Math.pow(2, -10 * --t) + 2) + b; }, easeInCirc: function (x, t, b, c, d) { return -c * (Math.sqrt(1 - (t /= d) * t) - 1) + b; }, easeOutCirc: function (x, t, b, c, d) { return c * Math.sqrt(1 - (t = t / d - 1) * t) + b; }, easeInOutCirc: function (x, t, b, c, d) { if ((t /= d / 2) < 1) return -c / 2 * (Math.sqrt(1 - t * t) - 1) + b; return c / 2 * (Math.sqrt(1 - (t -= 2) * t) + 1) + b; }, easeInElastic: function (x, t, b, c, d) {
        var s = 1.70158; var p = 0; var a = c; if (t == 0) return b; if ((t /= d) == 1) return b + c; if (!p) p = d * .3; if (a < Math.abs(c)) { a = c; var s = p / 4; }
        else var s = p / (2 * Math.PI) * Math.asin(c / a); return -(a * Math.pow(2, 10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p)) + b;
    }, easeOutElastic: function (x, t, b, c, d) {
        var s = 1.70158; var p = 0; var a = c; if (t == 0) return b; if ((t /= d) == 1) return b + c; if (!p) p = d * .3; if (a < Math.abs(c)) { a = c; var s = p / 4; }
        else var s = p / (2 * Math.PI) * Math.asin(c / a); return a * Math.pow(2, -10 * t) * Math.sin((t * d - s) * (2 * Math.PI) / p) + c + b;
    }, easeInOutElastic: function (x, t, b, c, d) {
        var s = 1.70158; var p = 0; var a = c; if (t == 0) return b; if ((t /= d / 2) == 2) return b + c; if (!p) p = d * (.3 * 1.5); if (a < Math.abs(c)) { a = c; var s = p / 4; }
        else var s = p / (2 * Math.PI) * Math.asin(c / a); if (t < 1) return -.5 * (a * Math.pow(2, 10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p)) + b; return a * Math.pow(2, -10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p) * .5 + c + b;
    }, easeInBack: function (x, t, b, c, d, s) { if (s == undefined) s = 1.70158; return c * (t /= d) * t * ((s + 1) * t - s) + b; }, easeOutBack: function (x, t, b, c, d, s) { if (s == undefined) s = 1.70158; return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b; }, easeInOutBack: function (x, t, b, c, d, s) { if (s == undefined) s = 1.70158; if ((t /= d / 2) < 1) return c / 2 * (t * t * (((s *= (1.525)) + 1) * t - s)) + b; return c / 2 * ((t -= 2) * t * (((s *= (1.525)) + 1) * t + s) + 2) + b; }, easeInBounce: function (x, t, b, c, d) { return c - jQuery.easing.easeOutBounce(x, d - t, 0, c, d) + b; }, easeOutBounce: function (x, t, b, c, d) { if ((t /= d) < (1 / 2.75)) { return c * (7.5625 * t * t) + b; } else if (t < (2 / 2.75)) { return c * (7.5625 * (t -= (1.5 / 2.75)) * t + .75) + b; } else if (t < (2.5 / 2.75)) { return c * (7.5625 * (t -= (2.25 / 2.75)) * t + .9375) + b; } else { return c * (7.5625 * (t -= (2.625 / 2.75)) * t + .984375) + b; } }, easeInOutBounce: function (x, t, b, c, d) { if (t < d / 2) return jQuery.easing.easeInBounce(x, t * 2, 0, c, d) * .5 + b; return jQuery.easing.easeOutBounce(x, t * 2 - d, 0, c, d) * .5 + c * .5 + b; } 
    });
})(jQuery); (function ($) { $.effects.blind = function (o) { return this.queue(function () { var el = $(this), props = ['position', 'top', 'left']; var mode = $.effects.setMode(el, o.options.mode || 'hide'); var direction = o.options.direction || 'vertical'; $.effects.save(el, props); el.show(); var wrapper = $.effects.createWrapper(el).css({ overflow: 'hidden' }); var ref = (direction == 'vertical') ? 'height' : 'width'; var distance = (direction == 'vertical') ? wrapper.height() : wrapper.width(); if (mode == 'show') wrapper.css(ref, 0); var animation = {}; animation[ref] = mode == 'show' ? distance : 0; wrapper.animate(animation, o.duration, o.options.easing, function () { if (mode == 'hide') el.hide(); $.effects.restore(el, props); $.effects.removeWrapper(el); if (o.callback) o.callback.apply(el[0], arguments); el.dequeue(); }); }); }; })(jQuery); (function ($) { $.effects.bounce = function (o) { return this.queue(function () { var el = $(this), props = ['position', 'top', 'left']; var mode = $.effects.setMode(el, o.options.mode || 'effect'); var direction = o.options.direction || 'up'; var distance = o.options.distance || 20; var times = o.options.times || 5; var speed = o.duration || 250; if (/show|hide/.test(mode)) props.push('opacity'); $.effects.save(el, props); el.show(); $.effects.createWrapper(el); var ref = (direction == 'up' || direction == 'down') ? 'top' : 'left'; var motion = (direction == 'up' || direction == 'left') ? 'pos' : 'neg'; var distance = o.options.distance || (ref == 'top' ? el.outerHeight({ margin: true }) / 3 : el.outerWidth({ margin: true }) / 3); if (mode == 'show') el.css('opacity', 0).css(ref, motion == 'pos' ? -distance : distance); if (mode == 'hide') distance = distance / (times * 2); if (mode != 'hide') times--; if (mode == 'show') { var animation = { opacity: 1 }; animation[ref] = (motion == 'pos' ? '+=' : '-=') + distance; el.animate(animation, speed / 2, o.options.easing); distance = distance / 2; times--; }; for (var i = 0; i < times; i++) { var animation1 = {}, animation2 = {}; animation1[ref] = (motion == 'pos' ? '-=' : '+=') + distance; animation2[ref] = (motion == 'pos' ? '+=' : '-=') + distance; el.animate(animation1, speed / 2, o.options.easing).animate(animation2, speed / 2, o.options.easing); distance = (mode == 'hide') ? distance * 2 : distance / 2; }; if (mode == 'hide') { var animation = { opacity: 0 }; animation[ref] = (motion == 'pos' ? '-=' : '+=') + distance; el.animate(animation, speed / 2, o.options.easing, function () { el.hide(); $.effects.restore(el, props); $.effects.removeWrapper(el); if (o.callback) o.callback.apply(this, arguments); }); } else { var animation1 = {}, animation2 = {}; animation1[ref] = (motion == 'pos' ? '-=' : '+=') + distance; animation2[ref] = (motion == 'pos' ? '+=' : '-=') + distance; el.animate(animation1, speed / 2, o.options.easing).animate(animation2, speed / 2, o.options.easing, function () { $.effects.restore(el, props); $.effects.removeWrapper(el); if (o.callback) o.callback.apply(this, arguments); }); }; el.queue('fx', function () { el.dequeue(); }); el.dequeue(); }); }; })(jQuery); (function ($) {
    $.effects.clip = function (o) {
        return this.queue(function () {
            var el = $(this), props = ['position', 'top', 'left', 'height', 'width']; var mode = $.effects.setMode(el, o.options.mode || 'hide'); var direction = o.options.direction || 'vertical'; $.effects.save(el, props); el.show(); var wrapper = $.effects.createWrapper(el).css({ overflow: 'hidden' }); var animate = el[0].tagName == 'IMG' ? wrapper : el; var ref = { size: (direction == 'vertical') ? 'height' : 'width', position: (direction == 'vertical') ? 'top' : 'left' }; var distance = (direction == 'vertical') ? animate.height() : animate.width(); if (mode == 'show') { animate.css(ref.size, 0); animate.css(ref.position, distance / 2); }
            var animation = {}; animation[ref.size] = mode == 'show' ? distance : 0; animation[ref.position] = mode == 'show' ? 0 : distance / 2; animate.animate(animation, { queue: false, duration: o.duration, easing: o.options.easing, complete: function () { if (mode == 'hide') el.hide(); $.effects.restore(el, props); $.effects.removeWrapper(el); if (o.callback) o.callback.apply(el[0], arguments); el.dequeue(); } });
        });
    };
})(jQuery); (function ($) { $.effects.drop = function (o) { return this.queue(function () { var el = $(this), props = ['position', 'top', 'left', 'opacity']; var mode = $.effects.setMode(el, o.options.mode || 'hide'); var direction = o.options.direction || 'left'; $.effects.save(el, props); el.show(); $.effects.createWrapper(el); var ref = (direction == 'up' || direction == 'down') ? 'top' : 'left'; var motion = (direction == 'up' || direction == 'left') ? 'pos' : 'neg'; var distance = o.options.distance || (ref == 'top' ? el.outerHeight({ margin: true }) / 2 : el.outerWidth({ margin: true }) / 2); if (mode == 'show') el.css('opacity', 0).css(ref, motion == 'pos' ? -distance : distance); var animation = { opacity: mode == 'show' ? 1 : 0 }; animation[ref] = (mode == 'show' ? (motion == 'pos' ? '+=' : '-=') : (motion == 'pos' ? '-=' : '+=')) + distance; el.animate(animation, { queue: false, duration: o.duration, easing: o.options.easing, complete: function () { if (mode == 'hide') el.hide(); $.effects.restore(el, props); $.effects.removeWrapper(el); if (o.callback) o.callback.apply(this, arguments); el.dequeue(); } }); }); }; })(jQuery); (function ($) {
    $.effects.explode = function (o) {
        return this.queue(function () {
            var rows = o.options.pieces ? Math.round(Math.sqrt(o.options.pieces)) : 3; var cells = o.options.pieces ? Math.round(Math.sqrt(o.options.pieces)) : 3; o.options.mode = o.options.mode == 'toggle' ? ($(this).is(':visible') ? 'hide' : 'show') : o.options.mode; var el = $(this).show().css('visibility', 'hidden'); var offset = el.offset(); offset.top -= parseInt(el.css("marginTop")) || 0; offset.left -= parseInt(el.css("marginLeft")) || 0; var width = el.outerWidth(true); var height = el.outerHeight(true); for (var i = 0; i < rows; i++) { for (var j = 0; j < cells; j++) { el.clone().appendTo('body').wrap('<div></div>').css({ position: 'absolute', visibility: 'visible', left: -j * (width / cells), top: -i * (height / rows) }).parent().addClass('effects-explode').css({ position: 'absolute', overflow: 'hidden', width: width / cells, height: height / rows, left: offset.left + j * (width / cells) + (o.options.mode == 'show' ? (j - Math.floor(cells / 2)) * (width / cells) : 0), top: offset.top + i * (height / rows) + (o.options.mode == 'show' ? (i - Math.floor(rows / 2)) * (height / rows) : 0), opacity: o.options.mode == 'show' ? 0 : 1 }).animate({ left: offset.left + j * (width / cells) + (o.options.mode == 'show' ? 0 : (j - Math.floor(cells / 2)) * (width / cells)), top: offset.top + i * (height / rows) + (o.options.mode == 'show' ? 0 : (i - Math.floor(rows / 2)) * (height / rows)), opacity: o.options.mode == 'show' ? 1 : 0 }, o.duration || 500); } }
            setTimeout(function () { o.options.mode == 'show' ? el.css({ visibility: 'visible' }) : el.css({ visibility: 'visible' }).hide(); if (o.callback) o.callback.apply(el[0]); el.dequeue(); $('.effects-explode').remove(); }, o.duration || 500);
        });
    };
})(jQuery); (function ($) { $.effects.fold = function (o) { return this.queue(function () { var el = $(this), props = ['position', 'top', 'left']; var mode = $.effects.setMode(el, o.options.mode || 'hide'); var size = o.options.size || 15; var horizFirst = !(!o.options.horizFirst); $.effects.save(el, props); el.show(); var wrapper = $.effects.createWrapper(el).css({ overflow: 'hidden' }); var widthFirst = ((mode == 'show') != horizFirst); var ref = widthFirst ? ['width', 'height'] : ['height', 'width']; var distance = widthFirst ? [wrapper.width(), wrapper.height()] : [wrapper.height(), wrapper.width()]; var percent = /([0-9]+)%/.exec(size); if (percent) size = parseInt(percent[1]) / 100 * distance[mode == 'hide' ? 0 : 1]; if (mode == 'show') wrapper.css(horizFirst ? { height: 0, width: size} : { height: size, width: 0 }); var animation1 = {}, animation2 = {}; animation1[ref[0]] = mode == 'show' ? distance[0] : size; animation2[ref[1]] = mode == 'show' ? distance[1] : 0; wrapper.animate(animation1, o.duration / 2, o.options.easing).animate(animation2, o.duration / 2, o.options.easing, function () { if (mode == 'hide') el.hide(); $.effects.restore(el, props); $.effects.removeWrapper(el); if (o.callback) o.callback.apply(el[0], arguments); el.dequeue(); }); }); }; })(jQuery); ; (function ($) { $.effects.highlight = function (o) { return this.queue(function () { var el = $(this), props = ['backgroundImage', 'backgroundColor', 'opacity']; var mode = $.effects.setMode(el, o.options.mode || 'show'); var color = o.options.color || "#ffff99"; var oldColor = el.css("backgroundColor"); $.effects.save(el, props); el.show(); el.css({ backgroundImage: 'none', backgroundColor: color }); var animation = { backgroundColor: oldColor }; if (mode == "hide") animation['opacity'] = 0; el.animate(animation, { queue: false, duration: o.duration, easing: o.options.easing, complete: function () { if (mode == "hide") el.hide(); $.effects.restore(el, props); if (mode == "show" && jQuery.browser.msie) this.style.removeAttribute('filter'); if (o.callback) o.callback.apply(this, arguments); el.dequeue(); } }); }); }; })(jQuery); (function ($) {
    $.effects.pulsate = function (o) {
        return this.queue(function () {
            var el = $(this); var mode = $.effects.setMode(el, o.options.mode || 'show'); var times = o.options.times || 5; if (mode == 'hide') times--; if (el.is(':hidden')) { el.css('opacity', 0); el.show(); el.animate({ opacity: 1 }, o.duration / 2, o.options.easing); times = times - 2; }
            for (var i = 0; i < times; i++) { el.animate({ opacity: 0 }, o.duration / 2, o.options.easing).animate({ opacity: 1 }, o.duration / 2, o.options.easing); }; if (mode == 'hide') { el.animate({ opacity: 0 }, o.duration / 2, o.options.easing, function () { el.hide(); if (o.callback) o.callback.apply(this, arguments); }); } else { el.animate({ opacity: 0 }, o.duration / 2, o.options.easing).animate({ opacity: 1 }, o.duration / 2, o.options.easing, function () { if (o.callback) o.callback.apply(this, arguments); }); }; el.queue('fx', function () { el.dequeue(); }); el.dequeue();
        });
    };
})(jQuery); (function ($) {
    $.effects.puff = function (o) { return this.queue(function () { var el = $(this); var options = $.extend(true, {}, o.options); var mode = $.effects.setMode(el, o.options.mode || 'hide'); var percent = parseInt(o.options.percent) || 150; options.fade = true; var original = { height: el.height(), width: el.width() }; var factor = percent / 100; el.from = (mode == 'hide') ? original : { height: original.height * factor, width: original.width * factor }; options.from = el.from; options.percent = (mode == 'hide') ? percent : 100; options.mode = mode; el.effect('scale', options, o.duration, o.callback); el.dequeue(); }); }; $.effects.scale = function (o) {
        return this.queue(function () {
            var el = $(this); var options = $.extend(true, {}, o.options); var mode = $.effects.setMode(el, o.options.mode || 'effect'); var percent = parseInt(o.options.percent) || (parseInt(o.options.percent) == 0 ? 0 : (mode == 'hide' ? 0 : 100)); var direction = o.options.direction || 'both'; var origin = o.options.origin; if (mode != 'effect') { options.origin = origin || ['middle', 'center']; options.restore = true; }
            var original = { height: el.height(), width: el.width() }; el.from = o.options.from || (mode == 'show' ? { height: 0, width: 0} : original); var factor = { y: direction != 'horizontal' ? (percent / 100) : 1, x: direction != 'vertical' ? (percent / 100) : 1 }; el.to = { height: original.height * factor.y, width: original.width * factor.x }; if (o.options.fade) { if (mode == 'show') { el.from.opacity = 0; el.to.opacity = 1; }; if (mode == 'hide') { el.from.opacity = 1; el.to.opacity = 0; }; }; options.from = el.from; options.to = el.to; options.mode = mode; el.effect('size', options, o.duration, o.callback); el.dequeue();
        });
    }; $.effects.size = function (o) { return this.queue(function () { var el = $(this), props = ['position', 'top', 'left', 'width', 'height', 'overflow', 'opacity']; var props1 = ['position', 'top', 'left', 'overflow', 'opacity']; var props2 = ['width', 'height', 'overflow']; var cProps = ['fontSize']; var vProps = ['borderTopWidth', 'borderBottomWidth', 'paddingTop', 'paddingBottom']; var hProps = ['borderLeftWidth', 'borderRightWidth', 'paddingLeft', 'paddingRight']; var mode = $.effects.setMode(el, o.options.mode || 'effect'); var restore = o.options.restore || false; var scale = o.options.scale || 'both'; var origin = o.options.origin; var original = { height: el.height(), width: el.width() }; el.from = o.options.from || original; el.to = o.options.to || original; if (origin) { var baseline = $.effects.getBaseline(origin, original); el.from.top = (original.height - el.from.height) * baseline.y; el.from.left = (original.width - el.from.width) * baseline.x; el.to.top = (original.height - el.to.height) * baseline.y; el.to.left = (original.width - el.to.width) * baseline.x; }; var factor = { from: { y: el.from.height / original.height, x: el.from.width / original.width }, to: { y: el.to.height / original.height, x: el.to.width / original.width} }; if (scale == 'box' || scale == 'both') { if (factor.from.y != factor.to.y) { props = props.concat(vProps); el.from = $.effects.setTransition(el, vProps, factor.from.y, el.from); el.to = $.effects.setTransition(el, vProps, factor.to.y, el.to); }; if (factor.from.x != factor.to.x) { props = props.concat(hProps); el.from = $.effects.setTransition(el, hProps, factor.from.x, el.from); el.to = $.effects.setTransition(el, hProps, factor.to.x, el.to); }; }; if (scale == 'content' || scale == 'both') { if (factor.from.y != factor.to.y) { props = props.concat(cProps); el.from = $.effects.setTransition(el, cProps, factor.from.y, el.from); el.to = $.effects.setTransition(el, cProps, factor.to.y, el.to); }; }; $.effects.save(el, restore ? props : props1); el.show(); $.effects.createWrapper(el); el.css('overflow', 'hidden').css(el.from); if (scale == 'content' || scale == 'both') { vProps = vProps.concat(['marginTop', 'marginBottom']).concat(cProps); hProps = hProps.concat(['marginLeft', 'marginRight']); props2 = props.concat(vProps).concat(hProps); el.find("*[width]").each(function () { child = $(this); if (restore) $.effects.save(child, props2); var c_original = { height: child.height(), width: child.width() }; child.from = { height: c_original.height * factor.from.y, width: c_original.width * factor.from.x }; child.to = { height: c_original.height * factor.to.y, width: c_original.width * factor.to.x }; if (factor.from.y != factor.to.y) { child.from = $.effects.setTransition(child, vProps, factor.from.y, child.from); child.to = $.effects.setTransition(child, vProps, factor.to.y, child.to); }; if (factor.from.x != factor.to.x) { child.from = $.effects.setTransition(child, hProps, factor.from.x, child.from); child.to = $.effects.setTransition(child, hProps, factor.to.x, child.to); }; child.css(child.from); child.animate(child.to, o.duration, o.options.easing, function () { if (restore) $.effects.restore(child, props2); }); }); }; el.animate(el.to, { queue: false, duration: o.duration, easing: o.options.easing, complete: function () { if (mode == 'hide') el.hide(); $.effects.restore(el, restore ? props : props1); $.effects.removeWrapper(el); if (o.callback) o.callback.apply(this, arguments); el.dequeue(); } }); }); };
})(jQuery); (function ($) { $.effects.shake = function (o) { return this.queue(function () { var el = $(this), props = ['position', 'top', 'left']; var mode = $.effects.setMode(el, o.options.mode || 'effect'); var direction = o.options.direction || 'left'; var distance = o.options.distance || 20; var times = o.options.times || 3; var speed = o.duration || o.options.duration || 140; $.effects.save(el, props); el.show(); $.effects.createWrapper(el); var ref = (direction == 'up' || direction == 'down') ? 'top' : 'left'; var motion = (direction == 'up' || direction == 'left') ? 'pos' : 'neg'; var animation = {}, animation1 = {}, animation2 = {}; animation[ref] = (motion == 'pos' ? '-=' : '+=') + distance; animation1[ref] = (motion == 'pos' ? '+=' : '-=') + distance * 2; animation2[ref] = (motion == 'pos' ? '-=' : '+=') + distance * 2; el.animate(animation, speed, o.options.easing); for (var i = 1; i < times; i++) { el.animate(animation1, speed, o.options.easing).animate(animation2, speed, o.options.easing); }; el.animate(animation1, speed, o.options.easing).animate(animation, speed / 2, o.options.easing, function () { $.effects.restore(el, props); $.effects.removeWrapper(el); if (o.callback) o.callback.apply(this, arguments); }); el.queue('fx', function () { el.dequeue(); }); el.dequeue(); }); }; })(jQuery); (function ($) { $.effects.slide = function (o) { return this.queue(function () { var el = $(this), props = ['position', 'top', 'left']; var mode = $.effects.setMode(el, o.options.mode || 'show'); var direction = o.options.direction || 'left'; $.effects.save(el, props); el.show(); $.effects.createWrapper(el).css({ overflow: 'hidden' }); var ref = (direction == 'up' || direction == 'down') ? 'top' : 'left'; var motion = (direction == 'up' || direction == 'left') ? 'pos' : 'neg'; var distance = o.options.distance || (ref == 'top' ? el.outerHeight({ margin: true }) : el.outerWidth({ margin: true })); if (mode == 'show') el.css(ref, motion == 'pos' ? -distance : distance); var animation = {}; animation[ref] = (mode == 'show' ? (motion == 'pos' ? '+=' : '-=') : (motion == 'pos' ? '-=' : '+=')) + distance; el.animate(animation, { queue: false, duration: o.duration, easing: o.options.easing, complete: function () { if (mode == 'hide') el.hide(); $.effects.restore(el, props); $.effects.removeWrapper(el); if (o.callback) o.callback.apply(this, arguments); el.dequeue(); } }); }); }; })(jQuery); (function ($) { $.effects.transfer = function (o) { return this.queue(function () { var el = $(this); var mode = $.effects.setMode(el, o.options.mode || 'effect'); var target = $(o.options.to); var position = el.offset(); var transfer = $('<div class="ui-effects-transfer"></div>').appendTo(document.body); if (o.options.className) transfer.addClass(o.options.className); transfer.addClass(o.options.className); transfer.css({ top: position.top, left: position.left, height: el.outerHeight() - parseInt(transfer.css('borderTopWidth')) - parseInt(transfer.css('borderBottomWidth')), width: el.outerWidth() - parseInt(transfer.css('borderLeftWidth')) - parseInt(transfer.css('borderRightWidth')), position: 'absolute' }); position = target.offset(); animation = { top: position.top, left: position.left, height: target.outerHeight() - parseInt(transfer.css('borderTopWidth')) - parseInt(transfer.css('borderBottomWidth')), width: target.outerWidth() - parseInt(transfer.css('borderLeftWidth')) - parseInt(transfer.css('borderRightWidth')) }; transfer.animate(animation, o.duration, o.options.easing, function () { transfer.remove(); if (o.callback) o.callback.apply(el[0], arguments); el.dequeue(); }); }); }; })(jQuery);