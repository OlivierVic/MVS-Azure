/**
 * Loader view model
 * Used for generic loader in all our view models
 * @param value (optional): the value to give to our loader
 */
function LoaderViewModel(value) {
    var self = this;

    /**
     * State of our view model
     */
    self.isLoading = ko.observable(value || false);

    /**
     * Switch the current state of our loading
     * @param newValue (optional): force the state to a particular loading value
     */
    self.switchLoading = function (newValue) {
        if (newValue !== undefined) {
            self.isLoading(Boolean(newValue));
        } else {
            self.isLoading(!(self.isLoading()));
        }
    }
}

/**
 Simple html example of how to use this loader:
 ```
 <!-- `name` is the id of the template to use, `data` is the LoaderViewModel field bound in our current view model. -->
 <div data-bind="template: { name: 'loader', data: loader1 }"></div>
 <div data-bind="template: { name: 'loader', data: loader2 }"></div>
 <button data-bind="click: clickMe">Click</button>
 <script>
 function MyViewModel() {
    var self = this;

    self.loader1 = new LoaderViewModel();
    self.loader2 = new LoaderViewModel();

    self.clickMe = function() {
        self.loader1.switchLoading();
    }
}

 var vm = new ViewModel();
 ko.applyBindings(vm, ctx);
 </script>
 ```

 **/
