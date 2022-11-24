
var tempFilterText = '',
	filterTextTimeout;


app.controller('QBGrid', ['$scope', '$http', '$timeout', '$templateCache', 'uiGridConstants', function ($scope, $http, $timeout, $templateCache, uiGridConstants) {

	$templateCache.put('ui-grid/selectionRowHeaderButtons',
		""
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
	
	$scope.data = [];
	$scope.filterValue = '';
	


	$scope.gridOptions = {
		infiniteScrollRowsFromEnd: 40,
		infiniteScrollUp: true,
		infiniteScrollDown: true,
		enableColumnMenus: false,
		enableRowSelection: false,
		enableSelectAll: false,
		useExternalSorting: false,
		showGridFooter: false,
		showFooter: false,
		enableHorizontalScrollbar: false,
		selectionRowHeaderWidth: 0,
		rowHeight: 40,
		enableSorting: false,
		headerRowHeight:35,
		columnDefs: [
			/*{ name: "Id", visible: false, enableSorting: true },
			{ name: "ExchangeName", displayName: ExchangeNameStr, width: '31%', enableSorting: false },
			{ name: "SubPublisherName", displayName: SubPublisherStr, width: '32%', enableSorting: true, cellTemplate: '<div><span class="trimed-span" style="padding: 4px;width: initial!important;" title="{{ COL_FIELD }}">{{ COL_FIELD }}</span></div>' },
			{ name: "SubPublisherId", displayName: SubPublisherIdStr, width: '32%', enableSorting: true },
			{ name: "SubPublisherMarketId", visible: false, enableSorting: true }*/

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
		

			$scope.gridApi = gridApi;
		}
	};
	$scope.gridOptions.multiSelect = false;
	$scope.getData = function () {
		JSONFilter.pageNumber = $scope.getCount();
		var resutsJSOn = JSON.stringify(JSONFilter);
		return $http({
			method: 'POST',
			url: DataQBGridUrl,
			data: resutsJSOn,
			headers: { 'Content-Type': 'application/json; charset=UTF-8' }
		})


		



	}
	$scope.getFirstData = function () {
		$scope.loading = true;
		// $scope.reSize(0);

	
		$scope.Count = 0;
		$scope.firstPage = 1;
		$scope.lastPage = 1;
		$scope.noMoreData = false;
		return $scope.getData()
			.then(function (response) {
				$scope.loading = false;
				//$scope.Spinner.hideSpinner();

                if (response.data.Massage.length != 0) {
                    $('#gridContainer').hide();
					showErrorMessage(response.data.Massage, true);
					//$("#warnings").html(data.Massage);
					//$("#resultTableDiv").html('');
					$scope.reset();
					return;
				}/* else {
					//$("#resultTableDiv").html(response.data.finalTable);


                    if (response.data.warnings != "") {
                        showErrorMessage(response.data.warnings, true);
                        $('#gridContainer').hide();
                    }
				}*/

                if (response.data.warnings.length != 0) {
                    showWarningMessage(response.data.warnings, true);
                }
				var newData = response.data.data.Rows;
				$scope.AddColumns(response.data.data.Columns)
				$scope.data = newData;
				$scope.reSize(newData.length);
					if($scope.Count==0)
					{
				  let totalCountElem = document.getElementById('totalCount');
				  totalCountElem.innerHTML =response.data.data.Count;
				
					}
                $scope.Count++;
              
				if (newData && newData.length > 99) {
					return $scope.gridApi.infiniteScroll.dataLoaded(false, true);
				}
				
			});
	};
	$scope.AddColumns = function (columns) {


        $scope.gridOptions.columnDefs = new Array();
        let WholeWidth = 0;
        if (columns.length > 9) {
            for (var i = 0; i < columns.length; ++i) {
                let tempWidth = columns[i].length < 6 ? (columns[i].length * 16) : (columns[i].length * 10) + 5;
                $scope.gridOptions.columnDefs.push({

                    field: columns[i], displayName: columns[i], width: tempWidth
                });
                WholeWidth += tempWidth;
            }
        } else {
            for (var i = 0; i < columns.length; ++i) {
                let tempWidth = columns[i].length < 6 ? (columns[i].length * 16) : (columns[i].length * 10) + 5;
                $scope.gridOptions.columnDefs.push({

                    field: columns[i], displayName: columns[i]
                });
                WholeWidth += tempWidth;
            }
        }
        document.getElementById('QBGridDiv').style.width = WholeWidth + "px";
		//$scope.gridApi.grid.refresh();
		
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

	$scope.filterFirst = function () {

			$scope.getFirstData().then(function () {
				$timeout(function () {
					// timeout needed to allow digest cycle to complete,and grid to finish ingesting the data
					$scope.gridApi.infiniteScroll.resetScroll($scope.firstPage > 1, true);
				});
			});
	
	};
	$scope.getDataDown = function () {
		$scope.loading = true;

		if (!$scope.noMoreData) {
			return $scope.getData()
				.then(function (response) {
					var newData = response.data.data.Rows;
					//$scope.AddColumns(response.data.data.Columns)
					$scope.loading = false;

					if (newData == null || newData.length == 0) {
						$scope.noMoreData = true;
						return;
					}

				if($scope.Count==0)
					{
				  let totalCountElem = document.getElementById('totalCount');
				  totalCountElem.innerHTML =response.data.data.Count;
				
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

	$scope.getCount = function () {
		
		return $scope.Count;
	};
	$scope.getPage = function (data, page) {
		var res = [];
		for (var i = (page * 100); i < (page + 1) * 100 && i < data.length; ++i) {
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
			angular.element(document.getElementsByClassName('grid')[0]).css('height', "225PX");
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
		if (length == 2) {
			newHeight = 130;
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
	
		$scope.gridOptions.columnDefs = new Array();


		$scope.data = [];

	};
	$scope.resetAll = function () {
		$scope.reset();

		/*
		$scope.gridApi.grid.sortColumn($scope.gridApi.grid.columns[1],
			uiGridConstants.DESC);*/
		$scope.gridApi.grid.notifyDataChange(uiGridConstants.dataChange.ALL);

	}

	

}]);


