/*Google Places API*/
// This example displays an address form, using the autocomplete feature
// of the Google Places API to help users fill in the information.

// This example requires the Places library. Include the libraries=places
// parameter when you first load the API. For example:
// <script src="https://maps.googleapis.com/maps/api/js?key=YOUR_API_KEY&libraries=places">

var placeSearch, autocomplete;
var componentForm = {
    street_number: 'short_name', /* va  Direccion*/
    route: 'long_name', /* va  Direccion*/
    locality: 'long_name', /* va  Localidad*/
    administrative_area_level_1: 'short_name', /* va EstadoProvincia*/
    country: 'long_name', /* va nombrePais  xx */
    country: 'short_name', /* va PaisCodigoGoogle short_name */
    postal_code: 'short_name' /* va codigoPostal xx*/
};


function initAutocomplete() {
    // Create the autocomplete object, restricting the search to geographical
    // location types.
    autocomplete = new google.maps.places.Autocomplete(
        /** @type {!HTMLInputElement} */(document.getElementById('autocomplete')),
        {  });

    // When the user selects an address from the dropdown, populate the address
    // fields in the form.
    autocomplete.addListener('place_changed', fillInAddress);
}

function fillInAddress() {
    // Get the place details from the autocomplete object.
    var place = autocomplete.getPlace();

    document.getElementById("Direccion").value = '';
    document.getElementById("Localidad").value = '';
    document.getElementById("EstadoProvincia").value = '';
    //document.getElementById("nombrePais").value = '';
    document.getElementById("PaisCodigoGoogle").value = '';
    //document.getElementById("codigoPostal").value = '';

    var domicilioConNumero = "";
    var domicilioCalle = "";
    var domicilioNumero = "";

    // Get each component of the address from the place details
    // and fill the corresponding field on the form.

    for (var i = 0; i < place.address_components.length; i++) {
        var addressType = place.address_components[i].types[0];

        if (componentForm[addressType]) {

            var val = place.address_components[i][componentForm[addressType]];
            //alert(addressType);
            if (addressType === "street_number") {
                domicilioNumero = val;
            }
            else if (addressType === "route") {
                domicilioCalle = val;
            }
            else if (addressType === "locality") {
                document.getElementById("Localidad").value = val;
            }
            else if (addressType === "administrative_area_level_1") {
                document.getElementById("EstadoProvincia").value = val;
            }
            else if (addressType === "postal_code") {
               // document.getElementById("codigoPostal").value = val;
            }
            else if (addressType === "country") {
                var val1 = place.address_components[i]["long_name"];

                document.getElementById("PaisCodigoGoogle").value = val;
                //console.log(document.getElementById("PaisCodigoGoogle").value);
                //document.getElementById("nombrePais").value = val1;

            }

        }
    }
    domicilioConNumero = domicilioCalle + " " + domicilioNumero;
    
    document.getElementById("Direccion").value = domicilioConNumero;
}

// Bias the autocomplete object to the user's geographical location,
// as supplied by the browser's 'navigator.geolocation' object.
function geolocate() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var geolocation = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };
            var circle = new google.maps.Circle({
                center: geolocation,
                radius: position.coords.accuracy
            });
            autocomplete.setBounds(circle.getBounds());
        });
    }
}
