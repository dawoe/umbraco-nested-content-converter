(function () {
  'use strict';

  function DashboardController($scope, resource) {
    var vm = this;

    vm.loading = true;
    vm.title = "Nested content converter"

    function getDataTypeMigrationState() {
      resource.getDataTypeMigrationState().then(function (result) {
        if (result.State !== 'Done') {
          vm.loading = false;
        }
      });
    }        

    function init() {
      getDataTypeMigrationState();
    }

    init();

  }

  angular.module('umbraco').controller('Umbraco.Community.NestedContentConverters.DashboardController',
    [
      '$scope',      
      'Umbraco.Community.NestedContentConverter.Resource',
      DashboardController
    ]);

})();
