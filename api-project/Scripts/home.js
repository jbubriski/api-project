(function ($) {
    $(function () {
        var ViewModel = function () {
            var self = this;

            self.city = ko.observable('');
            self.region = ko.observable('');
            self.country = ko.observable('');

            self.pictures = ko.observableArray([]);

            self.requestMade = ko.observable(false);

            self.location = ko.computed(function () {
                return self.city() + ', ' + self.region();
            }, self);

            self.loadPictures = function(latitude, longitude) {
                $.ajax({
                    type: "POST",
                    url: picturesEndpoint,
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ latitude: latitude, longitude: longitude }),
                    dataType: "jsonp",
                    jsonpCallback: "jsonFlickrApi",
                    success: function (d) {
                        console.log('Picture data:');
                        console.log(d);

                        self.requestMade(true);
                        self.pictures(d.photos.photo.slice(0, 25));


                        var $container = $('#container');
                        
                        $container.imagesLoaded(function() {
                            $container.masonry({
                                columnWidth: 200,
                                itemSelector: 'div'
                            });
                        });
                    },
                    error: function () {
                        alert('Failure getting picture data.');
                    }
                });
            };

            self.loadContactInfo = function() {
                $.ajax({
                    type: "POST",
                    url: locationEndpoint,
                    contentType: "application/json; charset=utf-8",
                    success: function (d) {
                        console.log('Location data:');
                        console.log(d);

                        self.city(d.city);
                        self.region(d.region);
                        self.country(d.country);

                        var location = d.loc.split(',');
                        var latitude = location[0];
                        var longitude = location[1];

                        self.loadPictures(latitude, longitude);
                    },
                    error: function () {
                        alert('Failure getting location data.');
                    }
                });
            };

            self.loadContactInfo();
        };

        ko.applyBindings(new ViewModel());
    });
})(jQuery);