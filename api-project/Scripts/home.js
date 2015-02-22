﻿(function ($) {
    $(function () {
        var ViewModel = function () {
            var self = this;

            self.city = ko.observable('');
            self.region = ko.observable('');
            self.country = ko.observable('');

            self.pictures = ko.observableArray([]);
            self.visiblePictures = ko.observableArray([]);

            self.requestMade = ko.observable(false);

            self.location = ko.computed(function () {
                return self.city() + ', ' + self.region();
            });

            self.pictureUrl = function (picture) {
                return 'https://farm' + picture.farm + '.staticflickr.com/' + picture.server + '/' + picture.id + '_' + picture.secret + '.jpg';
            };

            self.picturePageUrl = function (picture) {
                return 'https://www.flickr.com/photos/' + picture.owner + '/' + picture.id;
            };

            self.userUrl = function (picture) {
                return 'https://www.flickr.com/people/' + picture.owner;
            };

            self.appendToMasonry = function (element, index, data) {
                var $container = $('#container');
                
                $(element).imagesLoaded(function () {
                    $container.masonry('appended', element, true);
                });
            };

            self.loadMorePictures = function () {
                if (self.pictures().length < 25)
                    return;

                self.visiblePictures.splice.apply(self.visiblePictures, [self.visiblePictures().length, 0].concat(self.pictures.splice(0, 25)));
            };

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

                        self.pictures(d.photos.photo);
                        
                        self.loadMorePictures();
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


            var $container = $('#container');

            $container.masonry({
                columnWidth: 275,
                itemSelector: 'div.picture'
            });

            self.loadContactInfo();
        };

        ko.applyBindings(new ViewModel());
    });
})(jQuery);