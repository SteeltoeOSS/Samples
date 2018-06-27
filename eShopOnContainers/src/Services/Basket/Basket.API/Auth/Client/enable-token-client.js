(function ($, swaggerUi) {
    $(function () {
        var settings = {
            authority: 'https://identityapi.example.com',
            client_id: 'js',
            popup_redirect_uri: window.location.protocol
                + '//'
                + window.location.host
                + '/tokenclient/popup.html',

            response_type: 'id_token token',
            scope: 'openid profile basket',

            filter_protocol_claims: true
        },
        manager = new OidcTokenManager(settings),
        $inputApiKey = $('#input_apiKey');

        $inputApiKey.on('dblclick', function () {
            manager.openPopupForTokenAsync()
                .then(function () {
                    $inputApiKey.val(manager.access_token).change();
                }, function (error) {
                    console.error(error);
                });
        });
    });
})(jQuery, window.swaggerUi);