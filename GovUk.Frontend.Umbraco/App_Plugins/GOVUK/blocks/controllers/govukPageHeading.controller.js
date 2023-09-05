angular.module("umbraco").controller("govukPageHeadingController", function ($scope, editorState) {
    $scope.pageHeading = editorState.current.variants.filter(x => x.active)[0].name;
});