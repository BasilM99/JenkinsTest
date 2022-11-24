
var tempFilterText = '',
  filterTextTimeout;

var tempFilterTextExchangeName = '',
  filterTextTimeoutExchangeName;
app.controller('SSPGrid', ['$scope', '$http', '$timeout', '$templateCache', 'uiGridConstants', function ($scope, $http, $timeout, $templateCache, uiGridConstants) {

    $templateCache.put('ui-grid/selectionRowHeaderButtons',
    "<div   style=\" vertical-align: middle;\" class=\"ui-grid-selection-row-header-buttons ui-grid-icon-ok ng-scope\" ng-class=\"{'ui-grid-row-selected': row.isSelected}\" ng-click=\"selectButtonClick(row, $event)\">&nbsp;</div>"
    );
    $scope.showToolBar = false;
    $scope.noMoreData = false;
    $scope.loading = true;
    $scope.firstPage = 1;
    $scope.lastPage = 1;
    $scope.Spinner = null;
    $scope.Count = 0;
    $scope.columnName = "";
    $scope.direction = "";
    $scope.matcher = "";
    $scope.mySelectedRows = [];
    $scope.data = [];
    $scope.filterValue = '';
    $scope.filterValueExchangeName = "";


    $scope.gridOptions = {
        infiniteScrollRowsFromEnd: 40,
        infiniteScrollUp: true,
        infiniteScrollDown: true,
        enableColumnMenus: false,
        enableRowSelection: true,
        enableSelectAll: false,
        useExternalSorting: true,
        showGridFooter: false,
        showFooter: false,
        enableHorizontalScrollbar: false,
        selectionRowHeaderWidth: 35,
        rowHeight: 35,
        columnDefs: [
                { name: "Id", visible: false, enableSorting: true },
                { name: "ExchangeName", displayName: ExchangeNameStr, width: '31%', enableSorting: false },
                { name: "SubPublisherName", displayName: SubPublisherStr, width: '32%', enableSorting: true, cellTemplate: '<div><span class="trimed-span" style="padding: 4px;width: initial!important;" title="{{ COL_FIELD }}">{{ COL_FIELD }}</span></div>' },
			{ name: "SubPublisherId", displayName: SubPublisherIdStr, width: '32%', enableSorting: true },
			{ name: "SubPublisherMarketId", visible: false, enableSorting: true }
        ],
        data: 'data',
        onRegisterApi: function (gridApi) {
            gridApi.infiniteScroll.on.needLoadMoreData($scope, $scope.getDataDown);
            //gridApi.infiniteScroll.on.needLoadMoreDataTop($scope, $scope.getDataUp);
            gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                $scope.columnName = typeof (sortColumns[0]) != "undefined" ? sortColumns[0].name : "";
                $scope.direction = typeof (sortColumns[0]) != "undefined" ? sortColumns[0].sort.direction : "";

                $scope.getFirstData().then(function () {
                    $timeout(function () {
                        // timeout needed to allow digest cycle to complete,and grid to finish ingesting the data
                        $scope.gridApi.infiniteScroll.resetScroll($scope.firstPage > 1, true);
                    });
                });

            });
            gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                $scope.getSelectedRows();
            });

            $scope.gridApi = gridApi;
        }
    };
    $scope.gridOptions.multiSelect = true;
    $scope.getData = function () {
        return $http.get(DataSSPGridUrl + '?pageNumber=' + $scope.Count + "&Ids=" + SSPImageSelectedList + "&filterParam=" + $scope.filterValue + "&filterExchangeName=" + $scope.filterValueExchangeName + "&columnName=" + $scope.columnName + "&direction=" + $scope.direction)

    }
    $scope.getFirstData = function () {
        $scope.loading = true;
        // $scope.reSize(0);

        $scope.ClearSelection();
        $scope.Count = 0;
        $scope.firstPage = 1;
        $scope.lastPage = 1;
        $scope.noMoreData = false;
        return $scope.getData()
        .then(function (response) {
            $scope.loading = false;
            //$scope.Spinner.hideSpinner();

            var newData = response.data;
            $scope.data = newData;
            $scope.reSize(newData.length);
            $scope.Count++;
            if (newData && newData.length > 99) {
                return $scope.gridApi.infiniteScroll.dataLoaded(false, true);
            }
        });
    };


    $scope.filter = function () {
        if (filterTextTimeout) $timeout.cancel(filterTextTimeout);


        filterTextTimeout = $timeout(function () {
            $scope.getFirstData().then(function () {
                $timeout(function () {
                    // timeout needed to allow digest cycle to complete,and grid to finish ingesting the data
                    $scope.gridApi.infiniteScroll.resetScroll($scope.firstPage > 1, true);
                });
            });
        }, 1000); // delay 250 ms
    };
    $scope.filterExchangeName = function () {
        if (filterTextTimeoutExchangeName) $timeout.cancel(filterTextTimeoutExchangeName);


        filterTextTimeoutExchangeName = $timeout(function () {
            $scope.getFirstData().then(function () {
                $timeout(function () {
                    // timeout needed to allow digest cycle to complete,and grid to finish ingesting the data
                    $scope.gridApi.infiniteScroll.resetScroll($scope.firstPage > 1, true);
                });
            });
        }, 1000); // delay 250 ms

    };
    $scope.getDataDown = function () {
        $scope.loading = true;

        if (!$scope.noMoreData) {
            return $scope.getData()
            .then(function (response) {
                var newData = response.data;
                $scope.loading = false;

                if (newData == null || newData.length == 0) {
                    $scope.noMoreData = true;
                    return;
                }


                $scope.lastPage++;
                $scope.Count++;

                $scope.gridApi.infiniteScroll.saveScrollPercentage();
                $scope.data = $scope.data.concat(newData);
                return $scope.gridApi.infiniteScroll.dataLoaded(false, true);
            })
            .catch(function (error) {
                alert(error);
            });
        }
    };


    $scope.getPage = function (data, page) {
        var res = [];
        for (var i = (page * 100) ; i < (page + 1) * 100 && i < data.length; ++i) {
            res.push(data[i]);
        }
        return res;
    };

    $scope.checkDataLength = function (discardDirection) {
        // work out whether we need to discard a page, if so discard from the direction passed in
        if ($scope.lastPage - $scope.firstPage > 3) {
            // we want to remove a page
            $scope.gridApi.infiniteScroll.saveScrollPercentage();

            if (discardDirection === 'up') {
                $scope.data = $scope.data.slice(100);
                $scope.firstPage++;
                $timeout(function () {
                    // wait for grid to ingest data changes
                    $scope.gridApi.infiniteScroll.dataRemovedTop($scope.firstPage > 0, $scope.lastPage < 4);
                });
            } else {
                $scope.data = $scope.data.slice(0, 400);
                $scope.lastPage--;
                $timeout(function () {
                    // wait for grid to ingest data changes
                    $scope.gridApi.infiniteScroll.dataRemovedBottom($scope.firstPage > 0, $scope.lastPage < 4);
                });
            }
        }
    };
    $scope.reSize = function (length) {

        var newHeight = 250;

        if (length > 5) {
            angular.element(document.getElementsByClassName('grid')[0]).css('height', "250PX");
            return;
        }
        else {


            var newHeight = 45 * length;
        }
        if (length == 0) {
            newHeight = 80;
        }
        if (length == 1) {
            newHeight = 60;
        }
        angular.element(document.getElementsByClassName('grid')[0]).css('height', newHeight + 'px');

    };
    $scope.reset = function () {
        $scope.loading = true;
        $scope.showToolBar = true;
        $scope.firstPage = 1;
        $scope.lastPage = 1;
        $scope.Count = 0;
        $scope.noMoreData = false;

        $scope.matcher = "";
        $scope.filterValue = "";
        $scope.filterValueExchangeName = "";
        $scope.mySelectedRows = [];
        $scope.ClearSelection();

        $scope.data = [];

    };
    $scope.resetAll = function () {
        $scope.reset();


        $scope.gridApi.grid.sortColumn($scope.gridApi.grid.columns[1],
uiGridConstants.DESC);
        $scope.gridApi.grid.notifyDataChange(uiGridConstants.dataChange.ALL);

    }
    $scope.ClearSelection = function () {
        $scope.gridApi.selection.clearSelectedRows();
        $scope.mySelectedRows = [];
    };
    $scope.AddToInventorySource = function () {
        $scope.gridApi.selection.clearSelectedRows();

        for (var i = 0; i < $scope.mySelectedRows.length  ; i++) {
			selectInventorySubPublishers({ subPublisherId: $scope.mySelectedRows[i].SubPublisherId, SubPublisherName: $scope.mySelectedRows[i].SubPublisherName, ExchangeName: $scope.mySelectedRows[i].ExchangeName, SubAppSiteId: $scope.mySelectedRows[i].Id, ExchangeId: $scope.mySelectedRows[i].ExchangeId, AppSiteId: $scope.mySelectedRows[i].AppSiteId, SubPublisherMarketId: $scope.mySelectedRows[i].SubPublisherMarketId  });
        }

        $scope.mySelectedRows = [];
    };
    $scope.getSelectedRows = function () {
        $scope.mySelectedRows = $scope.gridApi.selection.getSelectedRows();
    };
}]);


