(function () {
  'use strict';

  function NestedContentConverterResource($http, umbRequestHelper) {

    var resource = {
      getDataTypeMigrationState: getDataTypeMigrationState,      
    };

    return resource;

    function getDataTypeMigrationState() {

      return umbRequestHelper.resourcePromise(
        $http.get(Umbraco.Sys.ServerVariables.NestedContentConverter.DataTypeMigrationStateUrl),
        'Failed to get datatype migration state'
      );
    };
    
  }

  angular.module('umbraco.resources').factory('Umbraco.Community.NestedContentConverter.Resource', NestedContentConverterResource);
