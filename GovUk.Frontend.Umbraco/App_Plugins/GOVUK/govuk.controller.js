angular.module('umbraco').controller('GOVUKPluginController', function ($scope, $http, editorState) {
    $http.get('/umbraco/backoffice/govuk/modelproperty/fordocumenttype?alias=' + encodeURIComponent(editorState.current.contentTypeAlias))
        .then(function successCallback(response) {
            $scope.propertyNames = [''].concat(response.data);
        });
});