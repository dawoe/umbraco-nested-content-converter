(function () {
  'use strict';

  function DashboardController($scope, resource) {
    var vm = this;

    vm.loading = true;
    vm.title = "Nested content converter"
    vm.dataTypeMigrationState = "";
    vm.dataTypesToMigrate = [];

    function getDataTypeMigrationState() {
      resource.getDataTypeMigrationState().then(function (result) {
        vm.dataTypeMigrationState = result.State;
        vm.dataTypesToMigrate = result.DataTypesToMigrate;

        vm.loading = false;

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
