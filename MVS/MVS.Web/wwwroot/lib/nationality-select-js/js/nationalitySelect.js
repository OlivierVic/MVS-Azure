// wrap in UMD - see https://github.com/umdjs/umd/blob/master/jqueryPlugin.js
(function (factory) {
    if (typeof define === "function" && define.amd) {
        define(["jquery"], function ($) {
            factory($, window, document);
        });
    } else if (typeof module === "object" && module.exports) {
        module.exports = factory(require("jquery"), window, document);
    } else {
        factory(jQuery, window, document);
    }
})(function ($, window, document, undefined) {
    "use strict";
    var pluginName = "nationalitySelect", id = 1, // give each instance its own ID for namespaced event handling
        defaults = {
            // Default country
            defaultCountry: "",
            // Position the selected flag inside or outside of the input
            defaultStyling: "inside",
            // don't display these countries
            excludeCountries: [],
            // Display only these countries
            onlyCountries: [],
            // The countries at the top of the list. Defaults to United States and United Kingdom
            preferredCountries: [],
            // localized country names e.g. { 'de': 'Deutschland' }
            localizedCountries: null,
            // Set the dropdown's width to be the same as the input. This is automatically enabled for small screens.
            responsiveDropdown: ($(window).width() < 768 ? true : false),
        }, keys = {
            UP: 38,
            DOWN: 40,
            ENTER: 13,
            ESC: 27,
            BACKSPACE: 8,
            PLUS: 43,
            SPACE: 32,
            A: 65,
            Z: 90
        }, windowLoaded = false;
    // keep track of if the window.load event has fired as impossible to check after the fact
    $(window).on('load', function () {
        windowLoaded = true;
    });
    function Plugin(element, options) {
        this.element = element;
        this.options = $.extend({}, defaults, options);
        this._defaults = defaults;
        // event namespace
        this.ns = "." + pluginName + id++;
        this._name = pluginName;
        this.init();
    }
    Plugin.prototype = {
        init: function () {
            // Process all the data: onlyCountries, excludeCountries, preferredCountries, defaultCountry etc
            this._processCountryData();
            // Generate the markup
            this._generateMarkup();
            // Set the initial state of the input value and the selected flag
            this._setInitialState();
            // Start all of the event listeners: input keyup, selectedFlag click
            this._initListeners();
            // Return this when the auto country is resolved.
            this.autoCountryDeferred = new $.Deferred();
            // Get auto country.
            this._initAutoCountry();
            // Keep track as the user types
            this.typedLetters = "";

            return this.autoCountryDeferred;
        },
        /********************
         *  PRIVATE METHODS
         ********************/
        // prepare all of the country data, including onlyCountries, excludeCountries, preferredCountries and
        // defaultCountry options
        _processCountryData: function () {
            // set the instances country data objects
            this._setInstanceCountryData();
            // set the preferredCountries property
            this._setPreferredCountries();
            // translate countries according to localizedCountries option
            if (this.options.localizedCountries) this._translateCountriesByLocale();
            // sort countries by name
            if (this.options.onlyCountries.length || this.options.localizedCountries) {
                this.countries.sort(this._countryNameSort);
            }
        },
        // process onlyCountries array if present
        _setInstanceCountryData: function () {
            var that = this;
            if (this.options.onlyCountries.length) {
                var newCountries = [];
                $.each(this.options.onlyCountries, function (i, countryCode) {
                    var countryData = that._getCountryData(countryCode, true);
                    if (countryData) {
                        newCountries.push(countryData);
                    }
                });
                this.countries = newCountries;
            } else if (this.options.excludeCountries.length) {
                var lowerCaseExcludeCountries = this.options.excludeCountries.map(function (country) {
                    return country.toLowerCase();
                });
                this.countries = allCountries.filter(function (country) {
                    return lowerCaseExcludeCountries.indexOf(country.iso2) === -1;
                });
            } else {
                this.countries = allCountries;
            }
        },
        // Process preferred countries - iterate through the preferences,
        // fetching the country data for each one
        _setPreferredCountries: function () {
            var that = this;
            this.preferredCountries = [];
            $.each(this.options.preferredCountries, function (i, countryCode) {
                var countryData = that._getCountryData(countryCode, false);
                if (countryData) {
                    that.preferredCountries.push(countryData);
                }
            });
        },
        // Translate Countries by object literal provided on config
        _translateCountriesByLocale() {
            for (let i = 0; i < this.countries.length; i++) {
                const iso = this.countries[i].iso2.toLowerCase();
                if (this.options.localizedCountries.hasOwnProperty(iso)) {
                    this.countries[i].name = this.options.localizedCountries[iso];
                }
            }
        },
        // sort by country name
        _countryNameSort(a, b) {
            return a.name.localeCompare(b.name);
        },
        // generate all of the markup for the plugin: the selected flag overlay, and the dropdown
        _generateMarkup: function () {
            // Country input
            this.countryInput = $(this.element);
            // containers (mostly for positioning)
            var mainClass = "country-select";
            if (this.options.defaultStyling) {
                mainClass += " " + this.options.defaultStyling;
            }
            this.countryInput.wrap($("<div>", {
                "class": mainClass
            }));
            var flagsContainer = $("<div>", {
                "class": "flag-dropdown"
            }).insertAfter(this.countryInput);
            // currently selected flag (displayed to left of input)
            var selectedFlag = $("<div>", {
                "class": "selected-flag"
            }).appendTo(flagsContainer);
            this.selectedFlagInner = $("<div>", {
                "class": "flag"
            }).appendTo(selectedFlag);
            // CSS triangle
            $("<div>", {
                "class": "arrow"
            }).appendTo(selectedFlag);
            // country list contains: preferred countries, then divider, then all countries
            this.countryList = $("<ul>", {
                "class": "country-list v-hide"
            }).appendTo(flagsContainer);
            if (this.preferredCountries.length) {
                this._appendListItems(this.preferredCountries, "preferred");
                $("<li>", {
                    "class": "divider"
                }).appendTo(this.countryList);
            }
            this._appendListItems(this.countries, "");
            // Add the hidden input for the country code
            this.countryCodeInput = $("#" + this.countryInput.attr("id") + "_code");
            if (!this.countryCodeInput) {
                this.countryCodeInput = $('<input type="hidden" id="' + this.countryInput.attr("id") + '_code" name="' + this.countryInput.attr("name") + '_code" value="" />');
                this.countryCodeInput.insertAfter(this.countryInput);
            }
            // now we can grab the dropdown height, and hide it properly
            this.dropdownHeight = this.countryList.outerHeight();
            // set the dropdown width according to the input if responsiveDropdown option is present or if it's a small screen
            if (this.options.responsiveDropdown) {
                $(window).resize(function () {
                    $('.country-select').each(function () {
                        var dropdownWidth = this.offsetWidth;
                        $(this).find('.country-list').css("width", dropdownWidth + "px");
                    });
                }).resize();
            }
            this.countryList.removeClass("v-hide").addClass("hide");
            // this is useful in lots of places
            this.countryListItems = this.countryList.children(".country");
        },
        // add a country <li> to the countryList <ul> container
        _appendListItems: function (countries, className) {
            // Generate DOM elements as a large temp string, so that there is only
            // one DOM insert event
            var tmp = "";
            // for each country
            $.each(countries, function (i, c) {
                // open the list item
                tmp += '<li class="country ' + className + '" data-country-code="' + c.iso2 + '">';
                // add the flag
                tmp += '<div class="flag ' + c.iso2 + '"></div>';
                // and the country name
                tmp += '<span class="country-name">' + c.name + '</span>';
                // close the list item
                tmp += '</li>';
            });
            this.countryList.append(tmp);
        },
        // set the initial state of the input value and the selected flag
        _setInitialState: function () {
            var flagIsSet = false;
            // If the input is pre-populated, then just update the selected flag
            if (this.countryInput.val()) {
                flagIsSet = this._updateFlagFromInputVal();
            }
            // If the country code input is pre-populated, update the name and the selected flag
            var selectedCode = this.countryCodeInput.val();
            if (selectedCode) {
                this.selectCountry(selectedCode);
            }
            if (!flagIsSet) {
                // flag is not set, so set to the default country
                var defaultCountry;
                // check the defaultCountry option, else fall back to the first in the list
                if (this.options.defaultCountry) {
                    defaultCountry = this._getCountryData(this.options.defaultCountry, false);
                    // Did we not find the requested default country?
                    if (!defaultCountry) {
                        defaultCountry = this.preferredCountries.length ? this.preferredCountries[0] : this.countries[0];
                    }
                } else {
                    defaultCountry = this.preferredCountries.length ? this.preferredCountries[0] : this.countries[0];
                }
                this.defaultCountry = defaultCountry.iso2;
            }
        },
        // initialise the main event listeners: input keyup, and click selected flag
        _initListeners: function () {
            var that = this;
            // Update flag on keyup.
            // Use keyup instead of keypress because we want to update on backspace
            // and instead of keydown because the value hasn't updated when that
            // event is fired.
            // NOTE: better to have this one listener all the time instead of
            // starting it on focus and stopping it on blur, because then you've
            // got two listeners (focus and blur)
            this.countryInput.on("keyup" + this.ns, function () {
                that._updateFlagFromInputVal();
            });
            // toggle country dropdown on click
            var selectedFlag = this.selectedFlagInner.parent();
            selectedFlag.on("click" + this.ns, function (e) {
                // only intercept this event if we're opening the dropdown
                // else let it bubble up to the top ("click-off-to-close" listener)
                // we cannot just stopPropagation as it may be needed to close another instance
                if (that.countryList.hasClass("hide") && !that.countryInput.prop("disabled")) {
                    that._showDropdown();
                }
            });
            // Despite above note, added blur to ensure partially spelled country
            // with correctly chosen flag is spelled out on blur. Also, correctly
            // selects flag when field is autofilled
            this.countryInput.on("blur" + this.ns, function () {
                if (that.countryInput.val() != that.getSelectedCountryData().name) {
                    that.setCountry(that.countryInput.val());
                }
                that.countryInput.val(that.getSelectedCountryData().name);
            });
        },
        _initAutoCountry: function () {
            if (this.options.initialCountry === "auto") {
                this._loadAutoCountry();
            } else {
                if (this.defaultCountry) {
                    this.selectCountry(this.defaultCountry);
                }
                this.autoCountryDeferred.resolve();
            }
        },
        // perform the geo ip lookup
        _loadAutoCountry: function () {
            var that = this;

            // 3 options:
            // 1) already loaded (we're done)
            // 2) not already started loading (start)
            // 3) already started loading (do nothing - just wait for loading callback to fire)
            if ($.fn[pluginName].autoCountry) {
                this.handleAutoCountry();
            } else if (!$.fn[pluginName].startedLoadingAutoCountry) {
                // don't do this twice!
                $.fn[pluginName].startedLoadingAutoCountry = true;

                if (typeof this.options.geoIpLookup === 'function') {
                    this.options.geoIpLookup(function (countryCode) {
                        $.fn[pluginName].autoCountry = countryCode.toLowerCase();
                        // tell all instances the auto country is ready
                        // TODO: this should just be the current instances
                        // UPDATE: use setTimeout in case their geoIpLookup function calls this callback straight away (e.g. if they have already done the geo ip lookup somewhere else). Using setTimeout means that the current thread of execution will finish before executing this, which allows the plugin to finish initialising.
                        setTimeout(function () {
                            $(".country-select input").nationalitySelect("handleAutoCountry");
                        });
                    });
                }
            }
        },
        // Focus input and put the cursor at the end
        _focus: function () {
            this.countryInput.focus();
            var input = this.countryInput[0];
            // works for Chrome, FF, Safari, IE9+
            if (input.setSelectionRange) {
                var len = this.countryInput.val().length;
                input.setSelectionRange(len, len);
            }
        },
        // Show the dropdown
        _showDropdown: function () {
            this._setDropdownPosition();
            // update highlighting and scroll to active list item
            var activeListItem = this.countryList.children(".active");
            this._highlightListItem(activeListItem);
            // show it
            this.countryList.removeClass("hide");
            this._scrollTo(activeListItem);
            // bind all the dropdown-related listeners: mouseover, click, click-off, keydown
            this._bindDropdownListeners();
            // update the arrow
            this.selectedFlagInner.parent().children(".arrow").addClass("up");
        },
        // decide where to position dropdown (depends on position within viewport, and scroll)
        _setDropdownPosition: function () {
            var inputTop = this.countryInput.offset().top, windowTop = $(window).scrollTop(),
                dropdownFitsBelow = inputTop + this.countryInput.outerHeight() + this.dropdownHeight < windowTop + $(window).height(), dropdownFitsAbove = inputTop - this.dropdownHeight > windowTop;
            // dropdownHeight - 1 for border
            var cssTop = !dropdownFitsBelow && dropdownFitsAbove ? "-" + (this.dropdownHeight - 1) + "px" : "";
            this.countryList.css("top", cssTop);
        },
        // we only bind dropdown listeners when the dropdown is open
        _bindDropdownListeners: function () {
            var that = this;
            // when mouse over a list item, just highlight that one
            // we add the class "highlight", so if they hit "enter" we know which one to select
            this.countryList.on("mouseover" + this.ns, ".country", function (e) {
                that._highlightListItem($(this));
            });
            // listen for country selection
            this.countryList.on("click" + this.ns, ".country", function (e) {
                that._selectListItem($(this));
            });
            // click off to close
            // (except when this initial opening click is bubbling up)
            // we cannot just stopPropagation as it may be needed to close another instance
            var isOpening = true;
            $("html").on("click" + this.ns, function (e) {
                e.preventDefault();
                if (!isOpening) {
                    that._closeDropdown();
                }
                isOpening = false;
            });
            // Listen for up/down scrolling, enter to select, or letters to jump to country name.
            // Use keydown as keypress doesn't fire for non-char keys and we want to catch if they
            // just hit down and hold it to scroll down (no keyup event).
            // Listen on the document because that's where key events are triggered if no input has focus
            $(document).on("keydown" + this.ns, function (e) {
                // prevent down key from scrolling the whole page,
                // and enter key from submitting a form etc
                e.preventDefault();
                if (e.which == keys.UP || e.which == keys.DOWN) {
                    // up and down to navigate
                    that._handleUpDownKey(e.which);
                } else if (e.which == keys.ENTER) {
                    // enter to select
                    that._handleEnterKey();
                } else if (e.which == keys.ESC) {
                    // esc to close
                    that._closeDropdown();
                } else if (e.which >= keys.A && e.which <= keys.Z || e.which === keys.SPACE) {
                    that.typedLetters += String.fromCharCode(e.which);
                    that._filterCountries(that.typedLetters);
                } else if (e.which === keys.BACKSPACE) {
                    that.typedLetters = that.typedLetters.slice(0, -1);
                    that._filterCountries(that.typedLetters);
                }
            });
        },
        // Highlight the next/prev item in the list (and ensure it is visible)
        _handleUpDownKey: function (key) {
            var current = this.countryList.children(".highlight").first();
            var next = key == keys.UP ? current.prev() : current.next();
            if (next.length) {
                // skip the divider
                if (next.hasClass("divider")) {
                    next = key == keys.UP ? next.prev() : next.next();
                }
                this._highlightListItem(next);
                this._scrollTo(next);
            }
        },
        // select the currently highlighted item
        _handleEnterKey: function () {
            var currentCountry = this.countryList.children(".highlight").first();
            if (currentCountry.length) {
                this._selectListItem(currentCountry);
            }
        },
        _filterCountries: function (letters) {
            var countries = this.countryListItems.filter(function () {
                return $(this).text().toUpperCase().indexOf(letters) === 0 && !$(this).hasClass("preferred");
            });
            if (countries.length) {
                // if one is already highlighted, then we want the next one
                var highlightedCountry = countries.filter(".highlight").first(), listItem;
                if (highlightedCountry && highlightedCountry.next() && highlightedCountry.next().text().toUpperCase().indexOf(letters) === 0) {
                    listItem = highlightedCountry.next();
                } else {
                    listItem = countries.first();
                }
                // update highlighting and scroll
                this._highlightListItem(listItem);
                this._scrollTo(listItem);
            }
        },
        // Update the selected flag using the input's current value
        _updateFlagFromInputVal: function () {
            var that = this;
            // try and extract valid country from input
            var value = this.countryInput.val().replace(/(?=[() ])/g, '\\');
            if (value) {
                var countryCodes = [];
                var matcher = new RegExp(value, "i");
                // Check for ISO codes only
                if (value.length <= 2) {
                    for (var i = 0; i < this.countries.length; i++) {
                        if (this.countries[i].iso2.match(matcher)) {
                            countryCodes.push(this.countries[i].iso2);
                        }
                    }
                }
                // If no previous matches / larger than 2 chars, then search country name
                if (countryCodes.length == 0) {
                    for (var i = 0; i < this.countries.length; i++) {
                        if (this.countries[i].name.match(matcher)) {
                            countryCodes.push(this.countries[i].iso2);
                        }
                    }
                }
                // Check if one of the matching countries is already selected
                var alreadySelected = false;
                $.each(countryCodes, function (i, c) {
                    if (that.selectedFlagInner.hasClass(c)) {
                        alreadySelected = true;
                    }
                });
                if (!alreadySelected) {
                    this._selectFlag(countryCodes[0]);
                    this.countryCodeInput.val(countryCodes[0]).trigger("change");
                }
                // Matching country found
                return true;
            }
            // No match found
            return false;
        },
        // remove highlighting from other list items and highlight the given item
        _highlightListItem: function (listItem) {
            this.countryListItems.removeClass("highlight");
            listItem.addClass("highlight");
        },
        // find the country data for the given country code
        // the ignoreOnlyCountriesOption is only used during init() while parsing the onlyCountries array
        _getCountryData: function (countryCode, ignoreOnlyCountriesOption) {
            var countryList = ignoreOnlyCountriesOption ? allCountries : this.countries;
            for (var i = 0; i < countryList.length; i++) {
                if (countryList[i].iso2 == countryCode) {
                    return countryList[i];
                }
            }
            return null;
        },
        // update the selected flag and the active list item
        _selectFlag: function (countryCode) {
            if (!countryCode) {
                return false;
            }
            this.selectedFlagInner.attr("class", "flag " + countryCode);
            // update the title attribute
            var countryData = this._getCountryData(countryCode);
            this.selectedFlagInner.parent().attr("title", countryData.name);
            // update the active list item
            var listItem = this.countryListItems.children(".flag." + countryCode).first().parent();
            this.countryListItems.removeClass("active");
            listItem.addClass("active");
        },
        // called when the user selects a list item from the dropdown
        _selectListItem: function (listItem) {
            // update selected flag and active list item
            var countryCode = listItem.attr("data-country-code");
            this._selectFlag(countryCode);
            this._closeDropdown();
            // update input value
            this._updateName(countryCode);
            this.countryInput.trigger("change");
            this.countryCodeInput.trigger("change");
            // focus the input
            this._focus();
        },
        // close the dropdown and unbind any listeners
        _closeDropdown: function () {
            this.countryList.addClass("hide");
            // update the arrow
            this.selectedFlagInner.parent().children(".arrow").removeClass("up");
            // unbind event listeners
            $(document).off("keydown" + this.ns);
            $("html").off("click" + this.ns);
            // unbind both hover and click listeners
            this.countryList.off(this.ns);
            this.typedLetters = "";
        },
        // check if an element is visible within its container, else scroll until it is
        _scrollTo: function (element) {
            if (!element || !element.offset()) {
                return;
            }
            var container = this.countryList, containerHeight = container.height(), containerTop = container.offset().top, containerBottom = containerTop + containerHeight, elementHeight = element.outerHeight(), elementTop = element.offset().top, elementBottom = elementTop + elementHeight, newScrollTop = elementTop - containerTop + container.scrollTop();
            if (elementTop < containerTop) {
                // scroll up
                container.scrollTop(newScrollTop);
            } else if (elementBottom > containerBottom) {
                // scroll down
                var heightDifference = containerHeight - elementHeight;
                container.scrollTop(newScrollTop - heightDifference);
            }
        },
        // Replace any existing country name with the new one
        _updateName: function (countryCode) {
            this.countryCodeInput.val(countryCode).trigger("change");
            this.countryInput.val(this._getCountryData(countryCode).name);
        },
        /********************
         *  PUBLIC METHODS
         ********************/
        // this is called when the geoip call returns
        handleAutoCountry: function () {
            if (this.options.initialCountry === "auto") {
                // we must set this even if there is an initial val in the input: in case the initial val is invalid and they delete it - they should see their auto country
                this.defaultCountry = $.fn[pluginName].autoCountry;
                // if there's no initial value in the input, then update the flag
                if (!this.countryInput.val()) {
                    this.selectCountry(this.defaultCountry);
                }
                this.autoCountryDeferred.resolve();
            }
        },
        // get the country data for the currently selected flag
        getSelectedCountryData: function () {
            // rely on the fact that we only set 2 classes on the selected flag element:
            // the first is "flag" and the second is the 2-char country code
            var countryCode = this.selectedFlagInner.attr("class").split(" ")[1];
            return this._getCountryData(countryCode);
        },
        // update the selected flag
        selectCountry: function (countryCode) {
            countryCode = countryCode.toLowerCase();
            // check if already selected
            if (!this.selectedFlagInner.hasClass(countryCode)) {
                this._selectFlag(countryCode);
                this._updateName(countryCode);
            }
        },
        // set the input value and update the flag
        setCountry: function (country) {
            this.countryInput.val(country);
            this._updateFlagFromInputVal();
        },
        // remove plugin
        destroy: function () {
            // stop listeners
            this.countryInput.off(this.ns);
            this.selectedFlagInner.parent().off(this.ns);
            // remove markup
            var container = this.countryInput.parent();
            container.before(this.countryInput).remove();
        }
    };
    // adapted to allow public functions
    // using https://github.com/jquery-boilerplate/jquery-boilerplate/wiki/Extending-jQuery-Boilerplate
    $.fn[pluginName] = function (options) {
        var args = arguments;
        // Is the first parameter an object (options), or was omitted,
        // instantiate a new instance of the plugin.
        if (options === undefined || typeof options === "object") {
            return this.each(function () {
                if (!$.data(this, "plugin_" + pluginName)) {
                    $.data(this, "plugin_" + pluginName, new Plugin(this, options));
                }
            });
        } else if (typeof options === "string" && options[0] !== "_" && options !== "init") {
            // If the first parameter is a string and it doesn't start
            // with an underscore or "contains" the `init`-function,
            // treat this as a call to a public method.
            // Cache the method call to make it possible to return a value
            var returns;
            this.each(function () {
                var instance = $.data(this, "plugin_" + pluginName);
                // Tests that there's already a plugin-instance
                // and checks that the requested public method exists
                if (instance instanceof Plugin && typeof instance[options] === "function") {
                    // Call the method of our plugin instance,
                    // and pass it the supplied arguments.
                    returns = instance[options].apply(instance, Array.prototype.slice.call(args, 1));
                }
                // Allow instances to be destroyed via the 'destroy' method
                if (options === "destroy") {
                    $.data(this, "plugin_" + pluginName, null);
                }
            });
            // If the earlier cached method gives a value back return the value,
            // otherwise return this to preserve chainability.
            return returns !== undefined ? returns : this;
        }
    };
    /********************
   *  STATIC METHODS
   ********************/
    // get the country data object
    $.fn[pluginName].getCountryData = function () {
        return allCountries;
    };
    // set the country data object
    $.fn[pluginName].setCountryData = function (obj) {
        allCountries = obj;
    };
    // Tell JSHint to ignore this warning: "character may get silently deleted by one or more browsers"
    // jshint -W100
    // Array of country objects for the flag dropdown.
    // Each contains a name and country code (ISO 3166-1 alpha-2).
    //
    // Note: using single char property names to keep filesize down
    // n = name
    // i = iso2 (2-char country code)
    var allCountries = $.each([{
        n: "Veuillez sélectionner votre nationalité.",
        i: "null"
    }, {
        n: "Afghane",
        i: "af"
    }, {
        n: "Ålandaise",
        i: "ax"
    }, {
        n: "Albanaise",
        i: "al"
    }, {
        n: "Algérienne",
        i: "dz"
    }, {
        n: "Allemande",
        i: "de"
    }, {
        n: "Américaine",
        i: "us"
    }, {
        n: "Andorrane",
        i: "ad"
    }, {
        n: "Angolaise",
        i: "ao"
    }, {
        n: "Antiguayenne/Barbudienne",
        i: "ag"
    }, {
        n: "Argentine",
        i: "ar"
    }, {
        n: "Arménienne",
        i: "am"
    }, {
        n: "Arubaise",
        i: "aw"
    }, {
        n: "Australienne",
        i: "au"
    }, {
        n: "Autrichienne",
        i: "at"
    }, {
        n: "Azerbaïdjanaise",
        i: "az"
    }, {
        n: "Bahaméenne",
        i: "bs"
    }, {
        n: "Bahraïnie",
        i: "bh"
    }, {
        n: "Bangladaise",
        i: "bd"
    }, {
        n: "Barbadienne",
        i: "bb"
    }, {
        n: "Belge (België)",
        i: "be"
    }, {
        n: "Bélizienne",
        i: "bz"
    }, {
        n: "Béninoise",
        i: "bj"
    }, {
        n: "Bermudienne",
        i: "bm"
    }, {
        n: "Biélorusse",
        i: "by"
    }, {
        n: "Birmane",
        i: "mm"
    }, {
        n: "Bissaoguinéenne",
        i: "gw"
    }, {
        n: "Bhoutanaise",
        i: "bt"
    }, {
        n: "Bolivienne",
        i: "bo"
    }, {
        n: "Bosnienne",
        i: "ba"
    }, {
        n: "Botswanaise",
        i: "bw"
    }, {
        n: "Brézilienne",
        i: "br"
    }, {
        n: "Britannique",
        i: "gb"
    }, {
        n: "Brunéienne",
        i: "bn"
    }, {
        n: "Bulgare",
        i: "bg"
    }, {
        n: "Burkinabè",
        i: "bf"
    }, {
        n: "Burundienne",
        i: "bi"
    }, {
        n: "Cambodgienne",
        i: "kh"
    }, {
        n: "Camerounaise",
        i: "cm"
    }, {
        n: "Canadienne",
        i: "ca"
    }, {
        n: "Capverdienne",
        i: "cv"
    }, {
        n: "Centrafricaine",
        i: "cf"
    }, {
        n: "Chilienne",
        i: "cl"
    }, {
        n: "Chinoise",
        i: "cn"
    }, {
        n: "Christophienne",
        i: "kn"
    }, {
        n: "Colombienne",
        i: "co"
    }, {
        n: "Comorienne",
        i: "km"
    }, {
        n: "Congolaise (RDC)",
        i: "cd"
    }, {
        n: "Congolaise (Republique)",
        i: "cg"
    }, {
        n: "Cookienne",
        i: "ck"
    }, {
        n: "Costaricienne",
        i: "cr"
    }, {
        n: "Ivoirienne",
        i: "ci"
    }, {
        n: "Croate",
        i: "hr"
    }, {
        n: "Cubaine",
        i: "cu"
    }, {
        n: "Curacienne",
        i: "cw"
    }, {
        n: "Chypriote",
        i: "cy"
    }, {
        n: "Danoise",
        i: "dk"
    }, {
        n: "Djiboutienne",
        i: "dj"
    }, {
        n: "Dominiquaise",
        i: "dm"
    }, {
        n: "Dominicaine",
        i: "do"
    }, {
        n: "Emirati",
        i: "ae"
    }, {
        n: "Equatorienne",
        i: "ec"
    }, {
        n: "Egyptienne",
        i: "eg"
    }, {
        n: "Équatoguinéenne",
        i: "gq"
    }, {
        n: "Érythréenne",
        i: "er"
    }, {
        n: "Espagnole",
        i: "es"
    }, {
        n: "Estonienne",
        i: "ee"
    }, {
        n: "Eswatinienne",
        i: "sz"
    }, {
        n: "Éthiopienne",
        i: "et"
    }, {
        n: "Fidjienne",
        i: "fj"
    }, {
        n: "Finlandaise",
        i: "fi"
    }, {
        n: "Française",
        i: "fr"
    }, {
        n: "Gabonaise",
        i: "ga"
    }, {
        n: "Gambienne",
        i: "gm"
    }, {
        n: "Géorgienne",
        i: "ge"
    }, {
        n: "Ghanéenne",
        i: "gh"
    }, {
        n: "Grecque",
        i: "gr"
    }, {
        n: "Grenadienne",
        i: "gd"
    }, {
        n: "Guatémaltèque",
        i: "gt"
    }, {
        n: "Guinéenne",
        i: "gn"
    }, {
        n: "Guyanienne",
        i: "gy"
    }, {
        n: "Haïtienne",
        i: "ht"
    }, {
        n: "Hondurienne",
        i: "hn"
    }, {
        n: "Hongkongaise",
        i: "hk"
    }, {
        n: "Hongroise",
        i: "hu"
    }, {
        n: "Indienne",
        i: "in"
    }, {
        n: "Indonésienne",
        i: "id"
    }, {
        n: "Iranienne",
        i: "ir"
    }, {
        n: "Irakienne",
        i: "iq"
    }, {
        n: "Irlandaise",
        i: "ie"
    }, {
        n: "Islandaise",
        i: "is"
    }, {
        n: "Israelienne",
        i: "il"
    }, {
        n: "Italienne",
        i: "it"
    }, {
        n: "Jamaïcaine",
        i: "jm"
    }, {
        n: "Japonaise",
        i: "jp"
    }, {
        n: "Jordanienne",
        i: "jo"
    }, {
        n: "Kazakhe",
        i: "kz"
    }, {
        n: "Kényane",
        i: "ke"
    }, {
        n: "Kirghize",
        i: "kg"
    }, {
        n: "Kiribati",
        i: "ki"
    }, {
        n: "Kosovare",
        i: "xk"
    }, {
        n: "Koweïtienne",
        i: "kw"
    }, {
        n: "Laotienne",
        i: "la"
    }, {
        n: "Lésothienne",
        i: "ls"
    }, {
        n: "Lettone",
        i: "lv"
    }, {
        n: "Libanaise",
        i: "lb"
    }, {
        n: "Libérienne",
        i: "lr"
    }, {
        n: "Libyenne",
        i: "ly"
    }, {
        n: "Liechtensteinoise",
        i: "li"
    }, {
        n: "Lituanienne",
        i: "lt"
    }, {
        n: "Luxembourgeoise",
        i: "lu"
    }, {
        n: "Macédonienne",
        i: "mk"
    }, {
        n: "Malgache",
        i: "mg"
    }, {
        n: "Malaisienne",
        i: "my"
    }, {
        n: "Malawite",
        i: "mw"
    }, {
        n: "Maldivienne",
        i: "mv"
    }, {
        n: "Malienne",
        i: "ml"
    }, {
        n: "Maltaise",
        i: "mt"
    }, {
        n: "Marocaine",
        i: "ma"
    }, {
        n: "Marshallaise",
        i: "mh"
    }, {
        n: "Mauricienne",
        i: "mu"
    }, {
        n: "Mauritanienne",
        i: "mr"
    }, {
        n: "Mexicaine",
        i: "mx"
    }, {
        n: "Micronésienne",
        i: "fm"
    }, {
        n: "Moldave",
        i: "md"
    }, {
        n: "Monégasque",
        i: "mc"
    }, {
        n: "Mongole",
        i: "mn"
    }, {
        n: "Monténégrine",
        i: "me"
    }, {
        n: "Mozambicaine",
        i: "mz"
    }, {
        n: "Namibienne",
        i: "na"
    }, {
        n: "Nauruane",
        i: "nr"
    }, {
        n: "Népalaise",
        i: "np"
    }, {
        n: "Néerlandaise",
        i: "nl"
    }, {
        n: "Néo-zélandaise",
        i: "nz"
    }, {
        n: "Nicaraguayenne",
        i: "ni"
    }, {
        n: "Nigerienne",
        i: "ne"
    }, {
        n: "Nigériane",
        i: "ng"
    }, {
        n: "Niuéenne",
        i: "nu"
    }, {
        n: "Nord-Coréenne",
        i: "kp"
    }, {
        n: "Norvégienne",
        i: "no"
    }, {
        n: "Omanaise",
        i: "om"
    }, {
        n: "Ougandaise",
        i: "ug"
    }, {
        n: "Ouzbek",
        i: "uz"
    }, {
        n: "Pakistanaise",
        i: "pk"
    }, {
        n: "Palaosienne",
        i: "pw"
    }, {
        n: "Palestinienne",
        i: "ps"
    }, {
        n: "Panaméenne",
        i: "pa"
    }, {
        n: "Papouan-néo-guinéenne",
        i: "pg"
    }, {
        n: "Paraguayenne",
        i: "py"
    }, {
        n: "Péruvienne",
        i: "pe"
    }, {
        n: "Philippine",
        i: "ph"
    }, {
        n: "Polanaise",
        i: "pl"
    }, {
        n: "Portugaise",
        i: "pt"
    }, {
        n: "Qatari",
        i: "qa"
    }, {
        n: "Roumaine",
        i: "ro"
    }, {
        n: "Russe",
        i: "ru"
    }, {
        n: "Rwandaise",
        i: "rw"
    }, {
        n: "Saint-Lucienne",
        i: "lc"
    }, {
        n: "Saint-Marinaise",
        i: "lc"
    }, {
        n: "Salomonaise",
        i: "sb"
    }, {
        n: "Salvadorienne",
        i: "sv"
    }, {
        n: "Samoane",
        i: "ws"
    }, {
        n: "Santoméene",
        i: "st"
    }, {
        n: "Saoudienne",
        i: "sa"
    }, {
        n: "Senegalaise",
        i: "sn"
    }, {
        n: "Serbe",
        i: "rs"
    }, {
        n: "Seychelloise",
        i: "sc"
    }, {
        n: "Sierra-Léonaise",
        i: "sl"
    }, {
        n: "Singapourienne",
        i: "sg"
    }, {
        n: "Slovaque",
        i: "sk"
    }, {
        n: "Slovène",
        i: "si"
    }, {
        n: "Somalienne",
        i: "so"
    }, {
        n: "Soudanaise",
        i: "sd"
    }, {
        n: "Sud-Africaine",
        i: "za"
    }, {
        n: "Sud-Coréenne",
        i: "kr"
    }, {
        n: "Sud-Soudanaise",
        i: "ss"
    }, {
        n: "Sri Lankaise",
        i: "lk"
    }, {
        n: "Suédoise",
        i: "se"
    }, {
        n: "Suisse",
        i: "ch"
    }, {
        n: "Surinamienne",
        i: "sr"
    }, {
        n: "Syrienne",
        i: "sy"
    }, {
        n: "Tadjik",
        i: "tj"
    }, {
        n: "Taïwanaise",
        i: "tw"
    }, {
        n: "Tanzanienne",
        i: "tz"
    }, {
        n: "Tchadienne",
        i: "td"
    }, {
        n: "Tchèque",
        i: "cz"
    }, {
        n: "Thaïlandaise",
        i: "th"
    }, {
        n: "Timoraise",
        i: "tl"
    }, {
        n: "Togolaise",
        i: "tg"
    }, {
        n: "Tonganne",
        i: "to"
    }, {
        n: "Trinidienne et Tobagonienne",
        i: "tt"
    }, {
        n: "Tunisienne",
        i: "tn"
    }, {
        n: "Turque",
        i: "tr"
    }, {
        n: "Turkmène",
        i: "tm"
    }, {
        n: "Tuvalaise",
        i: "tv"
    }, {
        n: "Ukrainienne",
        i: "ua"
    }, {
        n: "Uruguayenne",
        i: "uy"
    }, {
        n: "Vanuataise",
        i: "vu"
    }, {
        n: "Vaticane",
        i: "va"
    }, {
        n: "Vénézuélienne",
        i: "ve"
    }, {
        n: "Vietnamienne",
        i: "vn"
    }, {
        n: "Vincentaise",
        i: "vc"
    }, {
        n: "Yéménite",
        i: "ye"
    }, {
        n: "Zambienne",
        i: "zm"
    }, {
        n: "Zimbabwéenne",
        i: "zw"
    }], function (i, c) {
        c.name = c.n;
        c.iso2 = c.i;
        delete c.n;
        delete c.i;
    });
});
