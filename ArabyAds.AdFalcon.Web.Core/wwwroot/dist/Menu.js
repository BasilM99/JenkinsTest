(function webpackUniversalModuleDefinition(root, factory) {
	if(typeof exports === 'object' && typeof module === 'object')
		module.exports = factory();
	else if(typeof define === 'function' && define.amd)
		define("app", [], factory);
	else if(typeof exports === 'object')
		exports["app"] = factory();
	else
		root["app"] = factory();
})(self, function() {
return /******/ (() => { // webpackBootstrap
/******/ 	var __webpack_modules__ = ({

/***/ "./ClientApp/MenuModule.jsx":
/*!**********************************!*\
  !*** ./ClientApp/MenuModule.jsx ***!
  \**********************************/
/*! namespace exports */
/*! exports [not provided] [maybe used in Menu (runtime-defined)] */
/*! runtime requirements: __webpack_require__, __webpack_require__.r, __webpack_exports__, __webpack_require__.* */
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-dom */ "./node_modules/react-dom/index.js");
/* harmony import */ var _components_Menu_MenuComponent__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./components/Menu/MenuComponent */ "./ClientApp/components/Menu/MenuComponent.jsx");



react_dom__WEBPACK_IMPORTED_MODULE_1__.render( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement(_components_Menu_MenuComponent__WEBPACK_IMPORTED_MODULE_2__.default, null), document.getElementById("MenuComponent"));

/***/ }),

/***/ "./ClientApp/components/Menu/MenuComponent.jsx":
/*!*****************************************************!*\
  !*** ./ClientApp/components/Menu/MenuComponent.jsx ***!
  \*****************************************************/
/*! namespace exports */
/*! export default [provided] [no usage info] [missing usage info prevents renaming] */
/*! other exports [not provided] [no usage info] */
/*! runtime requirements: __webpack_require__, __webpack_require__.n, __webpack_require__.r, __webpack_exports__, __webpack_require__.d, __webpack_require__.* */
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "default": () => /* binding */ MenuComponent
/* harmony export */ });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var axios__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! axios */ "./node_modules/axios/index.js");
/* harmony import */ var axios__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(axios__WEBPACK_IMPORTED_MODULE_1__);
function _typeof(obj) { "@babel/helpers - typeof"; if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }

function _createSuper(Derived) { var hasNativeReflectConstruct = _isNativeReflectConstruct(); return function _createSuperInternal() { var Super = _getPrototypeOf(Derived), result; if (hasNativeReflectConstruct) { var NewTarget = _getPrototypeOf(this).constructor; result = Reflect.construct(Super, arguments, NewTarget); } else { result = Super.apply(this, arguments); } return _possibleConstructorReturn(this, result); }; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _isNativeReflectConstruct() { if (typeof Reflect === "undefined" || !Reflect.construct) return false; if (Reflect.construct.sham) return false; if (typeof Proxy === "function") return true; try { Date.prototype.toString.call(Reflect.construct(Date, [], function () {})); return true; } catch (e) { return false; } }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }


 // declare global {
//     interface Window {
//         isDSP:any;
//         isDataProvider:any;
//         isNormal:any;
//         ReportScheduleCount:any;
//         PMPDealCount:any;
//         externalList:any;
// 		visibilityValue:any;
// 		dashBoardUrl:any;
// 		dashBoardUrllmpressionlog:any;
// 		ImpressionLogsUrl:any;
//     }
// }

var list = null;

var MenuComponent = /*#__PURE__*/function (_React$Component) {
  _inherits(MenuComponent, _React$Component);

  var _super = _createSuper(MenuComponent);

  function MenuComponent(props) {
    var _this;

    _classCallCheck(this, MenuComponent);

    _this = _super.call(this, props);

    _defineProperty(_assertThisInitialized(_this), "componentWillMount", function () {
      //var externalList = null;
      var self = _assertThisInitialized(_this);

      axios__WEBPACK_IMPORTED_MODULE_1___default().get("/en/User/GetExternalDataProviderQueryResultAllResultActionResult").then(function (response) {
        // handle success
        //debugger;
        //externalList = response.data;
        self.setState({
          externalList: response.data
        });
        console.log(response);
      });
    });

    _defineProperty(_assertThisInitialized(_this), "callfunct", function (funcName) {
      console.log(funcName);
      eval(funcName);
    });

    _defineProperty(_assertThisInitialized(_this), "getTranslations", function () {
      var element = window.document.querySelector('[data-translations]');

      if (element) {
        return JSON.parse(element.getAttribute('data-translations'));
      }

      return {};
    });

    _defineProperty(_assertThisInitialized(_this), "renderExternalList", function (externalList) {
      var liList = [];
      var TranslationsContext = /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createContext(_this.getTranslations());
      externalList.result.forEach(function (element) {
        liList.push( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement(TranslationsContext.Consumer, null, function (t) {
          return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", {
            id: "DataProviderMenuNameLink",
            key: element.ID
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            id: "DataProviderMenuName"
          }, element.Name), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("ul", {
            className: "dropdownAdFalcon nextlevel",
            style: {
              left: 19 + 'px'
            }
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", {
            id: "DataProviderMenuForgetpassword" + element.ID,
            style: {
              display: 'none'
            }
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            id: "DataProviderMenuForgetpasswordURL",
            href: "/en/DataProvider/LandingDataProvider?Id =" + element.ID + "&Source=Forgetpassword"
          }, t.componentTranslations.Forgetpassword_Tran)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", {
            id: "DataProviderMenuChangePassword" + element.ID,
            style: {
              display: 'none'
            }
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            id: "DataProviderMenuChangePasswordURL",
            href: "/en/DataProvider/LandingDataProvider?Id =" + element.ID + "&Source=ChangePassword"
          }, t.componentTranslations.ChangePassword_Tran)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", {
            id: "DataProviderMenuLogin" + element.ID
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            id: "DataProviderMenuLoginURL",
            href: "/en/DataProvider/LandingDataProvider?Id =" + element.ID + "&Source=Login"
          }, t.componentTranslations.Login_Tran)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", {
            id: "DataProviderMenuRegister" + element.ID,
            style: {
              display: 'none'
            }
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            id: "DataProviderMenuRegisterURL",
            href: "/en/DataProvider/LandingDataProvider?Id =" + element.ID + "&Source=Register"
          }, t.componentTranslations.Register_Tran)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", {
            id: "DataProviderMenuAudiance" + element.ID,
            style: {
              display: 'none'
            }
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            id: "DataProviderMenuAudianceURL",
            href: "/en/DataProvider/LandingDataProvider?Id =" + element.ID + "&Source=Audiance"
          }, t.componentTranslations.AudienceLists_Tran)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", {
            id: "DataProviderMenuLogOut" + element.ID,
            style: {
              display: 'none'
            }
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            className: "",
            href: "#",
            onClick: function onClick() {
              return _this.callfunct("HandleDataProviderMenuLogOut" + element.ID + "();");
            }
          }, t.componentTranslations.Logout_Tran))));
        }));
      });
      return liList;
    });

    _defineProperty(_assertThisInitialized(_this), "reunderMenu", function () {
      var externalList = _this.state.externalList;
      var TranslationsContext = /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createContext(_this.getTranslations());

      if (window.isDSP) {
        //if(true){
        return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement(TranslationsContext.Consumer, null, function (t) {
          return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("ul", {
            id: "menu-nav",
            style: {
              visibility: window.visibilityValue
            }
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", {
            id: "ListMenuDashboard"
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            className: "parent gradient",
            href: window.dashboardadUrl
          }, t.componentTranslations.Dashboard_Tran)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", {
            id: "ListMenuAdvertiser"
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            className: "parent gradient",
            href: window.AccountAdvertisersUrl
          }, t.componentTranslations.Advertisers_Tran), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("ul", {
            className: "dropdownAdFalcon"
          }, window.TrafficPlannerCount == true ? /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            href: "/en/Campaign/TrafficPlanner"
          }, t.componentTranslations.TrafficPlanner_Tran)) : null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            className: "",
            href: window.MasterAppSitesUrl
          }, t.componentTranslations.ContentLists_Tran)))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", {
            id: "ListMenuReports"
          }, window.ReportScheduleCount == true ? /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            className: "parent gradient",
            href: "/en/reports/IndexReportsJob?reportType=ad"
          }, t.componentTranslations.Reports_Tran) : /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            className: "parent gradient",
            href: "/en/reports?reportType=ad"
          }, t.componentTranslations.Reports_Tran)), window.PMPDealCount == true ? /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", {
            id: "ListMenuDeals"
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            className: "parent gradient"
          }, t.componentTranslations.Deals_Tran), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("ul", {
            className: "dropdownAdFalcon "
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            className: "",
            href: "/en/deals/Menu?id="
          }, t.componentTranslations.PMPDeals_Tran)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            className: "",
            href: "/en/dashboard?chartType=deal"
          }, t.componentTranslations.DealMonitoring_Tran)))) : null, externalList != null && externalList.result.length > 0 ? /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", {
            id: "ListMenuDataProviders"
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            className: "parent gradient",
            href: "#"
          }, t.componentTranslations.Providers_Tran), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("ul", {
            className: "dropdownAdFalcon"
          }, _this.renderExternalList(externalList).map(function (li) {
            return li;
          }))) : null);
        });
      } else if (window.isDataProvider) {
        return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement(TranslationsContext.Consumer, null, function (t) {
          return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("ul", {
            id: "menu-nav",
            style: {
              visibility: window.visibilityValue
            }
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", {
            id: "ListMenuDashboard"
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            className: "parent gradient",
            href: window.dashBoardUrllmpressionlog
          }, t.componentTranslations.Dashboard_Tran)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", {
            id: "ListMenuAdvertiser"
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            className: "parent gradient",
            href: window.ImpressionLogsUrl
          }, t.componentTranslations.ImpressionLogs_Tran)));
        });
      } else {
        return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement(TranslationsContext.Consumer, null, function (t) {
          return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("ul", {
            id: "menu-nav",
            style: {
              visibility: window.visibilityValue
            }
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", {
            id: "ListMenuAdvertiser"
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            className: "parent gradient",
            href: window.dashBoardUrl
          }, t.componentTranslations.Advertiser_Tran), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("ul", {
            className: "dropdownAdFalcon"
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            href: "/en/dashboard?chartType=ad"
          }, t.componentTranslations.Dashboard_Tran)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            href: "/en/campaign/AccountAdvertisers"
          }, t.componentTranslations.Advertisers_Tran)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            className: "",
            href: "/en/campaign/MasterAppSites"
          }, t.componentTranslations.ContentLists_Tran)), window.ReportScheduleCount == true ? /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            href: "/en/reports/IndexReportsJob?reportType=ad"
          }, t.componentTranslations.Reports_Tran)) : /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            href: "/en/reports?reportType=ad"
          }, t.componentTranslations.Reports_Tran)), window.PMPDealCount == true ? /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", {
            className: "menu-last"
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", null, t.componentTranslations.Deals_Tran), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("ul", {
            className: "dropdownAdFalcon nextlevel",
            style: {
              left: 190
            }
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            className: "",
            href: "/en/deals?id"
          }, t.componentTranslations.PMPDeals_Tran)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            className: "",
            href: "/en/dashboard?chartType=deal"
          }, t.componentTranslations.DealMonitoring_Tran)))) : null)), window.isDSP == false ? /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", {
            id: "ListMenuPublisher"
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            className: "parent gradient",
            href: ""
          }, t.componentTranslations.Publisher_Tran), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("ul", {
            className: "dropdownAdFalcon"
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            href: "/en/dashboard?chartType=app"
          }, t.componentTranslations.Dashboard_Tran)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            href: "/en/appsite"
          }, t.componentTranslations.Apps_Tran)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            href: "/en/HouseAd"
          }, t.componentTranslations.HouseAd_Tran)), window.ReportScheduleCount ? /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            href: "/en/reports/IndexReportsJob?reportType=ad"
          }, t.componentTranslations.Reports_Tran)) : /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            href: "/en/reports?reportType=ad"
          }, t.componentTranslations.Reports_Tran)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("li", {
            className: "menu-last"
          }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0__.createElement("a", {
            href: "https://localhost:44324/en/download-sdk.html"
          }, t.componentTranslations.SDKs_Tran)))) : null);
        });
      }
    });

    _this.state = {
      externalList: null
    };
    return _this;
  }

  _createClass(MenuComponent, [{
    key: "render",
    value: function render() {
      return this.reunderMenu();
    }
  }]);

  return MenuComponent;
}(react__WEBPACK_IMPORTED_MODULE_0__.Component);



/***/ }),

/***/ "./node_modules/react-dom/index.js":
/*!******************************************************************************************************!*\
  !*** delegated ./node_modules/react-dom/index.js from dll-reference vendor_lib_afacacc3b8da05b89fd7 ***!
  \******************************************************************************************************/
/*! dynamic exports */
/*! export __SECRET_INTERNALS_DO_NOT_USE_OR_YOU_WILL_BE_FIRED [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export __esModule [not provided] [no usage info] [missing usage info prevents renaming] */
/*! export createPortal [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export findDOMNode [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export flushSync [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export hydrate [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export render [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export unmountComponentAtNode [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export unstable_batchedUpdates [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export unstable_createPortal [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export unstable_renderSubtreeIntoContainer [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export version [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! other exports [not provided] [no usage info] */
/*! runtime requirements: module, __webpack_require__ */
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {

module.exports = (__webpack_require__(/*! dll-reference vendor_lib_afacacc3b8da05b89fd7 */ "dll-reference vendor_lib_afacacc3b8da05b89fd7"))("./node_modules/react-dom/index.js");

/***/ }),

/***/ "./node_modules/react/index.js":
/*!**************************************************************************************************!*\
  !*** delegated ./node_modules/react/index.js from dll-reference vendor_lib_afacacc3b8da05b89fd7 ***!
  \**************************************************************************************************/
/*! dynamic exports */
/*! export Children [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export Component [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export Fragment [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export Profiler [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export PureComponent [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export StrictMode [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export Suspense [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export __SECRET_INTERNALS_DO_NOT_USE_OR_YOU_WILL_BE_FIRED [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export __esModule [not provided] [no usage info] [missing usage info prevents renaming] */
/*! export cloneElement [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export createContext [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export createElement [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export createFactory [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export createRef [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export forwardRef [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export isValidElement [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export lazy [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export memo [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export useCallback [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export useContext [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export useDebugValue [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export useEffect [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export useImperativeHandle [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export useLayoutEffect [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export useMemo [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export useReducer [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export useRef [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export useState [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! export version [provided] [no usage info] [provision prevents renaming (no use info)] */
/*! other exports [not provided] [no usage info] */
/*! runtime requirements: module, __webpack_require__ */
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {

module.exports = (__webpack_require__(/*! dll-reference vendor_lib_afacacc3b8da05b89fd7 */ "dll-reference vendor_lib_afacacc3b8da05b89fd7"))("./node_modules/react/index.js");

/***/ }),

/***/ "dll-reference vendor_lib_afacacc3b8da05b89fd7":
/*!**************************************************!*\
  !*** external "vendor_lib_afacacc3b8da05b89fd7" ***!
  \**************************************************/
/*! dynamic exports */
/*! exports [maybe provided (runtime-defined)] [no usage info] */
/*! runtime requirements: module */
/***/ ((module) => {

"use strict";
module.exports = vendor_lib_afacacc3b8da05b89fd7;

/***/ })

/******/ 	});
/************************************************************************/
/******/ 	// The module cache
/******/ 	var __webpack_module_cache__ = {};
/******/ 	
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/ 		// Check if module is in cache
/******/ 		if(__webpack_module_cache__[moduleId]) {
/******/ 			return __webpack_module_cache__[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = __webpack_module_cache__[moduleId] = {
/******/ 			// no module.id needed
/******/ 			// no module.loaded needed
/******/ 			exports: {}
/******/ 		};
/******/ 	
/******/ 		// Execute the module function
/******/ 		__webpack_modules__[moduleId](module, module.exports, __webpack_require__);
/******/ 	
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/ 	
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = __webpack_modules__;
/******/ 	
/************************************************************************/
/******/ 	/* webpack/runtime/compat get default export */
/******/ 	(() => {
/******/ 		// getDefaultExport function for compatibility with non-harmony modules
/******/ 		__webpack_require__.n = (module) => {
/******/ 			var getter = module && module.__esModule ?
/******/ 				() => module['default'] :
/******/ 				() => module;
/******/ 			__webpack_require__.d(getter, { a: getter });
/******/ 			return getter;
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/define property getters */
/******/ 	(() => {
/******/ 		// define getter functions for harmony exports
/******/ 		__webpack_require__.d = (exports, definition) => {
/******/ 			for(var key in definition) {
/******/ 				if(__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 					Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
/******/ 				}
/******/ 			}
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/hasOwnProperty shorthand */
/******/ 	(() => {
/******/ 		__webpack_require__.o = (obj, prop) => Object.prototype.hasOwnProperty.call(obj, prop)
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/make namespace object */
/******/ 	(() => {
/******/ 		// define __esModule on exports
/******/ 		__webpack_require__.r = (exports) => {
/******/ 			if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 				Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 			}
/******/ 			Object.defineProperty(exports, '__esModule', { value: true });
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/jsonp chunk loading */
/******/ 	(() => {
/******/ 		// no baseURI
/******/ 		
/******/ 		// object to store loaded and loading chunks
/******/ 		// undefined = chunk not loaded, null = chunk preloaded/prefetched
/******/ 		// Promise = chunk loading, 0 = chunk loaded
/******/ 		var installedChunks = {
/******/ 			"Menu": 0
/******/ 		};
/******/ 		
/******/ 		var deferredModules = [
/******/ 			["./ClientApp/MenuModule.jsx","vendors-node_modules_axios_index_js"]
/******/ 		];
/******/ 		// no chunk on demand loading
/******/ 		
/******/ 		// no prefetching
/******/ 		
/******/ 		// no preloaded
/******/ 		
/******/ 		// no HMR
/******/ 		
/******/ 		// no HMR manifest
/******/ 		
/******/ 		var checkDeferredModules = () => {
/******/ 		
/******/ 		};
/******/ 		function checkDeferredModulesImpl() {
/******/ 			var result;
/******/ 			for(var i = 0; i < deferredModules.length; i++) {
/******/ 				var deferredModule = deferredModules[i];
/******/ 				var fulfilled = true;
/******/ 				for(var j = 1; j < deferredModule.length; j++) {
/******/ 					var depId = deferredModule[j];
/******/ 					if(installedChunks[depId] !== 0) fulfilled = false;
/******/ 				}
/******/ 				if(fulfilled) {
/******/ 					deferredModules.splice(i--, 1);
/******/ 					result = __webpack_require__(__webpack_require__.s = deferredModule[0]);
/******/ 				}
/******/ 			}
/******/ 			if(deferredModules.length === 0) {
/******/ 				__webpack_require__.x();
/******/ 				__webpack_require__.x = () => {
/******/ 		
/******/ 				}
/******/ 			}
/******/ 			return result;
/******/ 		}
/******/ 		__webpack_require__.x = () => {
/******/ 			// reset startup function so it can be called again when more startup code is added
/******/ 			__webpack_require__.x = () => {
/******/ 		
/******/ 			}
/******/ 			chunkLoadingGlobal = chunkLoadingGlobal.slice();
/******/ 			for(var i = 0; i < chunkLoadingGlobal.length; i++) webpackJsonpCallback(chunkLoadingGlobal[i]);
/******/ 			return (checkDeferredModules = checkDeferredModulesImpl)();
/******/ 		};
/******/ 		
/******/ 		// install a JSONP callback for chunk loading
/******/ 		var webpackJsonpCallback = (data) => {
/******/ 			var [chunkIds, moreModules, runtime, executeModules] = data;
/******/ 			// add "moreModules" to the modules object,
/******/ 			// then flag all "chunkIds" as loaded and fire callback
/******/ 			var moduleId, chunkId, i = 0, resolves = [];
/******/ 			for(;i < chunkIds.length; i++) {
/******/ 				chunkId = chunkIds[i];
/******/ 				if(__webpack_require__.o(installedChunks, chunkId) && installedChunks[chunkId]) {
/******/ 					resolves.push(installedChunks[chunkId][0]);
/******/ 				}
/******/ 				installedChunks[chunkId] = 0;
/******/ 			}
/******/ 			for(moduleId in moreModules) {
/******/ 				if(__webpack_require__.o(moreModules, moduleId)) {
/******/ 					__webpack_require__.m[moduleId] = moreModules[moduleId];
/******/ 				}
/******/ 			}
/******/ 			if(runtime) runtime(__webpack_require__);
/******/ 			parentChunkLoadingFunction(data);
/******/ 			while(resolves.length) {
/******/ 				resolves.shift()();
/******/ 			}
/******/ 		
/******/ 			// add entry modules from loaded chunk to deferred list
/******/ 			if(executeModules) deferredModules.push.apply(deferredModules, executeModules);
/******/ 		
/******/ 			// run deferred modules when all chunks ready
/******/ 			return checkDeferredModules();
/******/ 		}
/******/ 		
/******/ 		var chunkLoadingGlobal = self["webpackChunkapp"] = self["webpackChunkapp"] || [];
/******/ 		var parentChunkLoadingFunction = chunkLoadingGlobal.push.bind(chunkLoadingGlobal);
/******/ 		chunkLoadingGlobal.push = webpackJsonpCallback;
/******/ 	})();
/******/ 	
/************************************************************************/
/******/ 	// module exports must be returned from runtime so entry inlining is disabled
/******/ 	// run startup
/******/ 	return __webpack_require__.x();
/******/ })()
;
});
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIndlYnBhY2s6Ly9hcHAvd2VicGFjay91bml2ZXJzYWxNb2R1bGVEZWZpbml0aW9uIiwid2VicGFjazovL2FwcC8uL0NsaWVudEFwcC9NZW51TW9kdWxlLmpzeCIsIndlYnBhY2s6Ly9hcHAvLi9DbGllbnRBcHAvY29tcG9uZW50cy9NZW51L01lbnVDb21wb25lbnQuanN4Iiwid2VicGFjazovL2FwcC9kZWxlZ2F0ZWQgXCIuL25vZGVfbW9kdWxlcy9yZWFjdC1kb20vaW5kZXguanNcIiBmcm9tIGRsbC1yZWZlcmVuY2UgdmVuZG9yX2xpYl9hZmFjYWNjM2I4ZGEwNWI4OWZkNyIsIndlYnBhY2s6Ly9hcHAvZGVsZWdhdGVkIFwiLi9ub2RlX21vZHVsZXMvcmVhY3QvaW5kZXguanNcIiBmcm9tIGRsbC1yZWZlcmVuY2UgdmVuZG9yX2xpYl9hZmFjYWNjM2I4ZGEwNWI4OWZkNyIsIndlYnBhY2s6Ly9hcHAvZXh0ZXJuYWwgXCJ2ZW5kb3JfbGliX2FmYWNhY2MzYjhkYTA1Yjg5ZmQ3XCIiLCJ3ZWJwYWNrOi8vYXBwL3dlYnBhY2svYm9vdHN0cmFwIiwid2VicGFjazovL2FwcC93ZWJwYWNrL3J1bnRpbWUvY29tcGF0IGdldCBkZWZhdWx0IGV4cG9ydCIsIndlYnBhY2s6Ly9hcHAvd2VicGFjay9ydW50aW1lL2RlZmluZSBwcm9wZXJ0eSBnZXR0ZXJzIiwid2VicGFjazovL2FwcC93ZWJwYWNrL3J1bnRpbWUvaGFzT3duUHJvcGVydHkgc2hvcnRoYW5kIiwid2VicGFjazovL2FwcC93ZWJwYWNrL3J1bnRpbWUvbWFrZSBuYW1lc3BhY2Ugb2JqZWN0Iiwid2VicGFjazovL2FwcC93ZWJwYWNrL3J1bnRpbWUvanNvbnAgY2h1bmsgbG9hZGluZyIsIndlYnBhY2s6Ly9hcHAvd2VicGFjay9zdGFydHVwIl0sIm5hbWVzIjpbIlJlYWN0RE9NIiwiZG9jdW1lbnQiLCJnZXRFbGVtZW50QnlJZCIsImxpc3QiLCJNZW51Q29tcG9uZW50IiwicHJvcHMiLCJzZWxmIiwiYXhpb3MiLCJ0aGVuIiwicmVzcG9uc2UiLCJzZXRTdGF0ZSIsImV4dGVybmFsTGlzdCIsImRhdGEiLCJjb25zb2xlIiwibG9nIiwiZnVuY05hbWUiLCJldmFsIiwiZWxlbWVudCIsIndpbmRvdyIsInF1ZXJ5U2VsZWN0b3IiLCJKU09OIiwicGFyc2UiLCJnZXRBdHRyaWJ1dGUiLCJsaUxpc3QiLCJUcmFuc2xhdGlvbnNDb250ZXh0IiwiUmVhY3QiLCJnZXRUcmFuc2xhdGlvbnMiLCJyZXN1bHQiLCJmb3JFYWNoIiwicHVzaCIsInQiLCJJRCIsIk5hbWUiLCJsZWZ0IiwiZGlzcGxheSIsImNvbXBvbmVudFRyYW5zbGF0aW9ucyIsIkZvcmdldHBhc3N3b3JkX1RyYW4iLCJDaGFuZ2VQYXNzd29yZF9UcmFuIiwiTG9naW5fVHJhbiIsIlJlZ2lzdGVyX1RyYW4iLCJBdWRpZW5jZUxpc3RzX1RyYW4iLCJjYWxsZnVuY3QiLCJMb2dvdXRfVHJhbiIsInN0YXRlIiwiaXNEU1AiLCJ2aXNpYmlsaXR5IiwidmlzaWJpbGl0eVZhbHVlIiwiZGFzaGJvYXJkYWRVcmwiLCJEYXNoYm9hcmRfVHJhbiIsIkFjY291bnRBZHZlcnRpc2Vyc1VybCIsIkFkdmVydGlzZXJzX1RyYW4iLCJUcmFmZmljUGxhbm5lckNvdW50IiwiVHJhZmZpY1BsYW5uZXJfVHJhbiIsIk1hc3RlckFwcFNpdGVzVXJsIiwiQ29udGVudExpc3RzX1RyYW4iLCJSZXBvcnRTY2hlZHVsZUNvdW50IiwiUmVwb3J0c19UcmFuIiwiUE1QRGVhbENvdW50IiwiRGVhbHNfVHJhbiIsIlBNUERlYWxzX1RyYW4iLCJEZWFsTW9uaXRvcmluZ19UcmFuIiwibGVuZ3RoIiwiUHJvdmlkZXJzX1RyYW4iLCJyZW5kZXJFeHRlcm5hbExpc3QiLCJtYXAiLCJsaSIsImlzRGF0YVByb3ZpZGVyIiwiZGFzaEJvYXJkVXJsbG1wcmVzc2lvbmxvZyIsIkltcHJlc3Npb25Mb2dzVXJsIiwiSW1wcmVzc2lvbkxvZ3NfVHJhbiIsImRhc2hCb2FyZFVybCIsIkFkdmVydGlzZXJfVHJhbiIsIlB1Ymxpc2hlcl9UcmFuIiwiQXBwc19UcmFuIiwiSG91c2VBZF9UcmFuIiwiU0RLc19UcmFuIiwicmV1bmRlck1lbnUiXSwibWFwcGluZ3MiOiJBQUFBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLENBQUM7QUFDRCxPOzs7Ozs7Ozs7Ozs7Ozs7OztBQ1ZBO0FBQ0E7QUFDQTtBQUdBQSw2Q0FBQSxlQUFnQixpREFBQyxtRUFBRCxPQUFoQixFQUFrQ0MsUUFBUSxDQUFDQyxjQUFULENBQXdCLGVBQXhCLENBQWxDLEU7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7QUNMQTtDQUdBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBQ0EsSUFBSUMsSUFBSSxHQUFHLElBQVg7O0lBRXFCQyxhOzs7OztBQUNwQix5QkFBWUMsS0FBWixFQUFrQjtBQUFBOztBQUFBOztBQUNYLDhCQUFNQSxLQUFOOztBQURXLHlFQVFHLFlBQU07QUFDMUI7QUFDQSxVQUFJQyxJQUFJLGdDQUFSOztBQUNBQyxzREFBQSxDQUFVLGtFQUFWLEVBQ0NDLElBREQsQ0FDTSxVQUFTQyxRQUFULEVBQW1CO0FBQ3RCO0FBQ0E7QUFDQTtBQUNISCxZQUFJLENBQUNJLFFBQUwsQ0FBYztBQUFFQyxzQkFBWSxFQUFHRixRQUFRLENBQUNHO0FBQTFCLFNBQWQ7QUFDR0MsZUFBTyxDQUFDQyxHQUFSLENBQVlMLFFBQVo7QUFDRixPQVBEO0FBUUEsS0FuQmlCOztBQUFBLGdFQW9CTixVQUFDTSxRQUFELEVBQWM7QUFDekJGLGFBQU8sQ0FBQ0MsR0FBUixDQUFZQyxRQUFaO0FBQ0FDLFVBQUksQ0FBQ0QsUUFBRCxDQUFKO0FBQ0EsS0F2QmlCOztBQUFBLHNFQXlCQSxZQUFPO0FBQ3hCLFVBQU1FLE9BQU8sR0FBR0MsTUFBTSxDQUFDakIsUUFBUCxDQUFnQmtCLGFBQWhCLENBQThCLHFCQUE5QixDQUFoQjs7QUFDQSxVQUFJRixPQUFKLEVBQWE7QUFDWixlQUFPRyxJQUFJLENBQUNDLEtBQUwsQ0FBV0osT0FBTyxDQUFDSyxZQUFSLENBQXFCLG1CQUFyQixDQUFYLENBQVA7QUFDQTs7QUFFRCxhQUFPLEVBQVA7QUFDQSxLQWhDaUI7O0FBQUEseUVBaUNHLFVBQUNYLFlBQUQsRUFBaUI7QUFDckMsVUFBSVksTUFBTSxHQUFHLEVBQWI7QUFFQSxVQUFJQyxtQkFBbUIsZ0JBQUdDLGdEQUFBLENBQW9CLE1BQUtDLGVBQUwsRUFBcEIsQ0FBMUI7QUFDQWYsa0JBQVksQ0FBQ2dCLE1BQWIsQ0FBb0JDLE9BQXBCLENBQTRCLFVBQUFYLE9BQU8sRUFBSTtBQUN0Q00sY0FBTSxDQUFDTSxJQUFQLGVBQ0MsaURBQUMsbUJBQUQsQ0FBcUIsUUFBckIsUUFDRSxVQUFBQyxDQUFDO0FBQUEsOEJBQ0Y7QUFBSSxjQUFFLEVBQUMsMEJBQVA7QUFBa0MsZUFBRyxFQUFFYixPQUFPLENBQUNjO0FBQS9DLDBCQUNBO0FBQUcsY0FBRSxFQUFDO0FBQU4sYUFBOEJkLE9BQU8sQ0FBQ2UsSUFBdEMsQ0FEQSxlQUdBO0FBQUkscUJBQVMsRUFBQyw0QkFBZDtBQUEyQyxpQkFBSyxFQUFFO0FBQUNDLGtCQUFJLEVBQUMsS0FBSztBQUFYO0FBQWxELDBCQUNDO0FBQUksY0FBRSxFQUFFLG1DQUFpQ2hCLE9BQU8sQ0FBQ2MsRUFBakQ7QUFBdUQsaUJBQUssRUFBRTtBQUFDRyxxQkFBTyxFQUFDO0FBQVQ7QUFBOUQsMEJBQWdGO0FBQUssY0FBRSxFQUFDLG1DQUFSO0FBQTRDLGdCQUFJLEVBQUUsOENBQTRDakIsT0FBTyxDQUFDYyxFQUFwRCxHQUF1RDtBQUF6RyxhQUFvSUQsQ0FBQyxDQUFDSyxxQkFBRixDQUF3QkMsbUJBQTVKLENBQWhGLENBREQsZUFFQztBQUFJLGNBQUUsRUFBRSxtQ0FBaUNuQixPQUFPLENBQUNjLEVBQWpEO0FBQXNELGlCQUFLLEVBQUU7QUFBQ0cscUJBQU8sRUFBQztBQUFUO0FBQTdELDBCQUErRTtBQUFJLGNBQUUsRUFBQyxtQ0FBUDtBQUEyQyxnQkFBSSxFQUFFLDhDQUE0Q2pCLE9BQU8sQ0FBQ2MsRUFBcEQsR0FBdUQ7QUFBeEcsYUFBbUlELENBQUMsQ0FBQ0sscUJBQUYsQ0FBd0JFLG1CQUEzSixDQUEvRSxDQUZELGVBSUM7QUFBSSxjQUFFLEVBQUUsMEJBQXdCcEIsT0FBTyxDQUFDYztBQUF4QywwQkFBNEM7QUFBSyxjQUFFLEVBQUMsMEJBQVI7QUFBbUMsZ0JBQUksRUFBRSw4Q0FBNENkLE9BQU8sQ0FBQ2MsRUFBcEQsR0FBdUQ7QUFBaEcsYUFBa0hELENBQUMsQ0FBQ0sscUJBQUYsQ0FBd0JHLFVBQTFJLENBQTVDLENBSkQsZUFLQztBQUFJLGNBQUUsRUFBRSw2QkFBMkJyQixPQUFPLENBQUNjLEVBQTNDO0FBQWdELGlCQUFLLEVBQUU7QUFBQ0cscUJBQU8sRUFBQztBQUFUO0FBQXZELDBCQUF5RTtBQUFLLGNBQUUsRUFBQyw2QkFBUjtBQUFzQyxnQkFBSSxFQUFFLDhDQUE0Q2pCLE9BQU8sQ0FBQ2MsRUFBcEQsR0FBdUQ7QUFBbkcsYUFBd0hELENBQUMsQ0FBQ0sscUJBQUYsQ0FBd0JJLGFBQWhKLENBQXpFLENBTEQsZUFPQztBQUFJLGNBQUUsRUFBRSw2QkFBMkJ0QixPQUFPLENBQUNjLEVBQTNDO0FBQWdELGlCQUFLLEVBQUU7QUFBQ0cscUJBQU8sRUFBQztBQUFUO0FBQXZELDBCQUF5RTtBQUFLLGNBQUUsRUFBQyw2QkFBUjtBQUFzQyxnQkFBSSxFQUFFLDhDQUE0Q2pCLE9BQU8sQ0FBQ2MsRUFBcEQsR0FBdUQ7QUFBbkcsYUFBd0hELENBQUMsQ0FBQ0sscUJBQUYsQ0FBd0JLLGtCQUFoSixDQUF6RSxDQVBELGVBU0M7QUFBSSxjQUFFLEVBQUUsMkJBQXlCdkIsT0FBTyxDQUFDYyxFQUF6QztBQUE4QyxpQkFBSyxFQUFFO0FBQUNHLHFCQUFPLEVBQUM7QUFBVDtBQUFyRCwwQkFBdUU7QUFBRyxxQkFBUyxFQUFDLEVBQWI7QUFBZ0IsZ0JBQUksRUFBQyxHQUFyQjtBQUF5QixtQkFBTyxFQUFFO0FBQUEscUJBQU0sTUFBS08sU0FBTCxDQUFlLGlDQUErQnhCLE9BQU8sQ0FBQ2MsRUFBdkMsR0FBMEMsS0FBekQsQ0FBTjtBQUFBO0FBQWxDLGFBQTRHRCxDQUFDLENBQUNLLHFCQUFGLENBQXdCTyxXQUFwSSxDQUF2RSxDQVRELENBSEEsQ0FERTtBQUFBLFNBREgsQ0FERDtBQXVCQSxPQXhCRDtBQTBCQSxhQUFPbkIsTUFBUDtBQUVBLEtBakVpQjs7QUFBQSxrRUFrRUosWUFBSztBQUNsQixVQUFJWixZQUFZLEdBQUcsTUFBS2dDLEtBQUwsQ0FBV2hDLFlBQTlCO0FBQ0MsVUFBSWEsbUJBQW1CLGdCQUFHQyxnREFBQSxDQUFvQixNQUFLQyxlQUFMLEVBQXBCLENBQTFCOztBQUNELFVBQUdSLE1BQU0sQ0FBQzBCLEtBQVYsRUFBZ0I7QUFDaEI7QUFFQyw0QkFDQyxpREFBQyxtQkFBRCxDQUFxQixRQUFyQixRQUNFLFVBQUFkLENBQUM7QUFBQSw4QkFDSDtBQUFJLGNBQUUsRUFBQyxVQUFQO0FBQWtCLGlCQUFLLEVBQUU7QUFBQ2Usd0JBQVUsRUFBRTNCLE1BQU0sQ0FBQzRCO0FBQXBCO0FBQXpCLDBCQUNDO0FBQUksY0FBRSxFQUFDO0FBQVAsMEJBQ0M7QUFBRyxxQkFBUyxFQUFDLGlCQUFiO0FBQStCLGdCQUFJLEVBQUU1QixNQUFNLENBQUM2QjtBQUE1QyxhQUE2RGpCLENBQUMsQ0FBQ0sscUJBQUYsQ0FBd0JhLGNBQXJGLENBREQsQ0FERCxlQUlDO0FBQUksY0FBRSxFQUFDO0FBQVAsMEJBQ0M7QUFBRyxxQkFBUyxFQUFDLGlCQUFiO0FBQStCLGdCQUFJLEVBQUU5QixNQUFNLENBQUMrQjtBQUE1QyxhQUFvRW5CLENBQUMsQ0FBQ0sscUJBQUYsQ0FBd0JlLGdCQUE1RixDQURELGVBR0M7QUFBSSxxQkFBUyxFQUFDO0FBQWQsYUFFRWhDLE1BQU0sQ0FBQ2lDLG1CQUFQLElBQThCLElBQTlCLGdCQUNBLDBFQUFJO0FBQUcsZ0JBQUksRUFBQztBQUFSLGFBQXVDckIsQ0FBQyxDQUFDSyxxQkFBRixDQUF3QmlCLG1CQUEvRCxDQUFKLENBREEsR0FFQyxJQUpILGVBTUMsMEVBQUk7QUFBRyxxQkFBUyxFQUFDLEVBQWI7QUFBZ0IsZ0JBQUksRUFBRWxDLE1BQU0sQ0FBQ21DO0FBQTdCLGFBQWlEdkIsQ0FBQyxDQUFDSyxxQkFBRixDQUF3Qm1CLGlCQUF6RSxDQUFKLENBTkQsQ0FIRCxDQUpELGVBZ0JDO0FBQUksY0FBRSxFQUFDO0FBQVAsYUFHRXBDLE1BQU0sQ0FBQ3FDLG1CQUFQLElBQThCLElBQTlCLGdCQUFtQztBQUFHLHFCQUFTLEVBQUMsaUJBQWI7QUFBK0IsZ0JBQUksRUFBQztBQUFwQyxhQUFpRnpCLENBQUMsQ0FBQ0sscUJBQUYsQ0FBd0JxQixZQUF6RyxDQUFuQyxnQkFBOEo7QUFBRyxxQkFBUyxFQUFDLGlCQUFiO0FBQStCLGdCQUFJLEVBQUM7QUFBcEMsYUFBaUUxQixDQUFDLENBQUNLLHFCQUFGLENBQXdCcUIsWUFBekYsQ0FIaEssQ0FoQkQsRUEyQkd0QyxNQUFNLENBQUN1QyxZQUFQLElBQXVCLElBQXZCLGdCQUNFO0FBQUksY0FBRSxFQUFDO0FBQVAsMEJBQ0E7QUFBRyxxQkFBUyxFQUFDO0FBQWIsYUFBZ0MzQixDQUFDLENBQUNLLHFCQUFGLENBQXdCdUIsVUFBeEQsQ0FEQSxlQUdBO0FBQUkscUJBQVMsRUFBQztBQUFkLDBCQUNDLDBFQUFJO0FBQUcscUJBQVMsRUFBQyxFQUFiO0FBQWdCLGdCQUFJLEVBQUM7QUFBckIsYUFBMkM1QixDQUFDLENBQUNLLHFCQUFGLENBQXdCd0IsYUFBbkUsQ0FBSixDQURELGVBRUMsMEVBQUk7QUFBRyxxQkFBUyxFQUFDLEVBQWI7QUFBZ0IsZ0JBQUksRUFBQztBQUFyQixhQUFxRDdCLENBQUMsQ0FBQ0sscUJBQUYsQ0FBd0J5QixtQkFBN0UsQ0FBSixDQUZELENBSEEsQ0FERixHQVFRLElBbkNYLEVBd0NHakQsWUFBWSxJQUFLLElBQWpCLElBQXlCQSxZQUFZLENBQUNnQixNQUFiLENBQW9Ca0MsTUFBcEIsR0FBNkIsQ0FBdEQsZ0JBQ0M7QUFBSSxjQUFFLEVBQUM7QUFBUCwwQkFDQTtBQUFHLHFCQUFTLEVBQUMsaUJBQWI7QUFBK0IsZ0JBQUksRUFBQztBQUFwQyxhQUF5Qy9CLENBQUMsQ0FBQ0sscUJBQUYsQ0FBd0IyQixjQUFqRSxDQURBLGVBRUE7QUFBSSxxQkFBUyxFQUFDO0FBQWQsYUFHQyxNQUFLQyxrQkFBTCxDQUF3QnBELFlBQXhCLEVBQXNDcUQsR0FBdEMsQ0FDQyxVQUFDQyxFQUFELEVBQVE7QUFDUCxtQkFBT0EsRUFBUDtBQUNBLFdBSEYsQ0FIRCxDQUZBLENBREQsR0FpQkcsSUF6RE4sQ0FERztBQUFBLFNBREgsQ0FERDtBQW9FQSxPQXZFRCxNQXdFSyxJQUFHL0MsTUFBTSxDQUFDZ0QsY0FBVixFQUF5QjtBQUM3Qiw0QkFDQyxpREFBQyxtQkFBRCxDQUFxQixRQUFyQixRQUNFLFVBQUFwQyxDQUFDO0FBQUEsOEJBQ0g7QUFBSSxjQUFFLEVBQUMsVUFBUDtBQUFrQixpQkFBSyxFQUFFO0FBQUNlLHdCQUFVLEVBQUUzQixNQUFNLENBQUM0QjtBQUFwQjtBQUF6QiwwQkFDQztBQUFJLGNBQUUsRUFBQztBQUFQLDBCQUNDO0FBQUcscUJBQVMsRUFBQyxpQkFBYjtBQUErQixnQkFBSSxFQUFFNUIsTUFBTSxDQUFDaUQ7QUFBNUMsYUFBd0VyQyxDQUFDLENBQUNLLHFCQUFGLENBQXdCYSxjQUFoRyxDQURELENBREQsZUFJQztBQUFJLGNBQUUsRUFBQztBQUFQLDBCQUNDO0FBQUcscUJBQVMsRUFBQyxpQkFBYjtBQUErQixnQkFBSSxFQUFFOUIsTUFBTSxDQUFDa0Q7QUFBNUMsYUFBZ0V0QyxDQUFDLENBQUNLLHFCQUFGLENBQXdCa0MsbUJBQXhGLENBREQsQ0FKRCxDQURHO0FBQUEsU0FESCxDQUREO0FBZUEsT0FoQkksTUFnQkE7QUFDSiw0QkFDQSxpREFBQyxtQkFBRCxDQUFxQixRQUFyQixRQUNHLFVBQUF2QyxDQUFDO0FBQUEsOEJBQ0g7QUFBSSxjQUFFLEVBQUMsVUFBUDtBQUFrQixpQkFBSyxFQUFFO0FBQUNlLHdCQUFVLEVBQUUzQixNQUFNLENBQUM0QjtBQUFwQjtBQUF6QiwwQkFDRTtBQUFJLGNBQUUsRUFBQztBQUFQLDBCQUNBO0FBQUcscUJBQVMsRUFBQyxpQkFBYjtBQUErQixnQkFBSSxFQUFFNUIsTUFBTSxDQUFDb0Q7QUFBNUMsYUFBMkR4QyxDQUFDLENBQUNLLHFCQUFGLENBQXdCb0MsZUFBbkYsQ0FEQSxlQUVBO0FBQUkscUJBQVMsRUFBQztBQUFkLDBCQUVDLDBFQUFJO0FBQUcsZ0JBQUksRUFBQztBQUFSLGFBQXNDekMsQ0FBQyxDQUFDSyxxQkFBRixDQUF3QmEsY0FBOUQsQ0FBSixDQUZELGVBSUMsMEVBQUk7QUFBRyxnQkFBSSxFQUFDO0FBQVIsYUFBMkNsQixDQUFDLENBQUNLLHFCQUFGLENBQXdCZSxnQkFBbkUsQ0FBSixDQUpELGVBS0MsMEVBQUk7QUFBRyxxQkFBUyxFQUFDLEVBQWI7QUFBZ0IsZ0JBQUksRUFBQztBQUFyQixhQUFvRHBCLENBQUMsQ0FBQ0sscUJBQUYsQ0FBd0JtQixpQkFBNUUsQ0FBSixDQUxELEVBUUVwQyxNQUFNLENBQUNxQyxtQkFBUCxJQUE4QixJQUE5QixnQkFBbUMsMEVBQUk7QUFBRyxnQkFBSSxFQUFDO0FBQVIsYUFBcUR6QixDQUFDLENBQUNLLHFCQUFGLENBQXdCcUIsWUFBN0UsQ0FBSixDQUFuQyxnQkFBMkksMEVBQUk7QUFBRyxnQkFBSSxFQUFDO0FBQVIsYUFBcUMxQixDQUFDLENBQUNLLHFCQUFGLENBQXdCcUIsWUFBN0QsQ0FBSixDQVI3SSxFQWFFdEMsTUFBTSxDQUFDdUMsWUFBUCxJQUF1QixJQUF2QixnQkFDQztBQUFJLHFCQUFTLEVBQUM7QUFBZCwwQkFDQSw0REFBSTNCLENBQUMsQ0FBQ0sscUJBQUYsQ0FBd0J1QixVQUE1QixDQURBLGVBR0E7QUFBSSxxQkFBUyxFQUFDLDRCQUFkO0FBQTJDLGlCQUFLLEVBQUU7QUFBQ3pCLGtCQUFJLEVBQUM7QUFBTjtBQUFsRCwwQkFDQywwRUFBSTtBQUFHLHFCQUFTLEVBQUMsRUFBYjtBQUFnQixnQkFBSSxFQUFDO0FBQXJCLGFBQXFDSCxDQUFDLENBQUNLLHFCQUFGLENBQXdCd0IsYUFBN0QsQ0FBSixDQURELGVBRUMsMEVBQUk7QUFBRyxxQkFBUyxFQUFDLEVBQWI7QUFBZ0IsZ0JBQUksRUFBQztBQUFyQixhQUFxRDdCLENBQUMsQ0FBQ0sscUJBQUYsQ0FBd0J5QixtQkFBN0UsQ0FBSixDQUZELENBSEEsQ0FERCxHQVFPLElBckJULENBRkEsQ0FERixFQTZCSTFDLE1BQU0sQ0FBQzBCLEtBQVAsSUFBZ0IsS0FBaEIsZ0JBRUM7QUFBSSxjQUFFLEVBQUM7QUFBUCwwQkFDQztBQUFHLHFCQUFTLEVBQUMsaUJBQWI7QUFBK0IsZ0JBQUksRUFBQztBQUFwQyxhQUF3Q2QsQ0FBQyxDQUFDSyxxQkFBRixDQUF3QnFDLGNBQWhFLENBREQsZUFFQztBQUFJLHFCQUFTLEVBQUM7QUFBZCwwQkFDQywwRUFBSTtBQUFHLGdCQUFJLEVBQUM7QUFBUixhQUF1QzFDLENBQUMsQ0FBQ0sscUJBQUYsQ0FBd0JhLGNBQS9ELENBQUosQ0FERCxlQUVDLDBFQUFJO0FBQUcsZ0JBQUksRUFBQztBQUFSLGFBQXVCbEIsQ0FBQyxDQUFDSyxxQkFBRixDQUF3QnNDLFNBQS9DLENBQUosQ0FGRCxlQUdDLDBFQUFJO0FBQUcsZ0JBQUksRUFBQztBQUFSLGFBQXVCM0MsQ0FBQyxDQUFDSyxxQkFBRixDQUF3QnVDLFlBQS9DLENBQUosQ0FIRCxFQUtHeEQsTUFBTSxDQUFDcUMsbUJBQVAsZ0JBRUEsMEVBQUk7QUFBRyxnQkFBSSxFQUFDO0FBQVIsYUFBcUR6QixDQUFDLENBQUNLLHFCQUFGLENBQXdCcUIsWUFBN0UsQ0FBSixDQUZBLGdCQU9BLDBFQUFJO0FBQUcsZ0JBQUksRUFBQztBQUFSLGFBQXFDMUIsQ0FBQyxDQUFDSyxxQkFBRixDQUF3QnFCLFlBQTdELENBQUosQ0FaSCxlQWtCQztBQUFJLHFCQUFTLEVBQUM7QUFBZCwwQkFBMEI7QUFBRyxnQkFBSSxFQUFDO0FBQVIsYUFBd0QxQixDQUFDLENBQUNLLHFCQUFGLENBQXdCd0MsU0FBaEYsQ0FBMUIsQ0FsQkQsQ0FGRCxDQUZELEdBMEJFLElBdkROLENBREc7QUFBQSxTQURKLENBREE7QUFvRUE7QUFDRCxLQW5PaUI7O0FBRWpCLFVBQUtoQyxLQUFMLEdBQWE7QUFDWmhDLGtCQUFZLEVBQUc7QUFESCxLQUFiO0FBRmlCO0FBS2pCOzs7OzZCQWlPTztBQUVQLGFBQ0MsS0FBS2lFLFdBQUwsRUFERDtBQUdBOzs7O0VBNU95Q25ELDRDOzs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7QUNuQjNDLGtMOzs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7QUNBQSw4Szs7Ozs7Ozs7Ozs7Ozs7QUNBQSxpRDs7Ozs7O1VDQUE7VUFDQTs7VUFFQTtVQUNBO1VBQ0E7VUFDQTtVQUNBO1VBQ0E7VUFDQTtVQUNBO1VBQ0E7VUFDQTtVQUNBO1VBQ0E7O1VBRUE7VUFDQTs7VUFFQTtVQUNBO1VBQ0E7O1VBRUE7VUFDQTs7Ozs7V0N4QkE7V0FDQTtXQUNBO1dBQ0E7V0FDQTtXQUNBLGdDQUFnQyxZQUFZO1dBQzVDO1dBQ0EsRTs7Ozs7V0NQQTtXQUNBO1dBQ0E7V0FDQTtXQUNBLHdDQUF3Qyx5Q0FBeUM7V0FDakY7V0FDQTtXQUNBLEU7Ozs7O1dDUEEsc0Y7Ozs7O1dDQUE7V0FDQTtXQUNBO1dBQ0Esc0RBQXNELGtCQUFrQjtXQUN4RTtXQUNBLCtDQUErQyxjQUFjO1dBQzdELEU7Ozs7O1dDTkE7O1dBRUE7V0FDQTtXQUNBO1dBQ0E7V0FDQTtXQUNBOztXQUVBO1dBQ0E7V0FDQTtXQUNBOztXQUVBOztXQUVBOztXQUVBOztXQUVBOztXQUVBOztXQUVBO1dBQ0E7V0FDQTtXQUNBLGVBQWUsNEJBQTRCO1dBQzNDO1dBQ0E7V0FDQSxnQkFBZ0IsMkJBQTJCO1dBQzNDO1dBQ0E7V0FDQTtXQUNBO1dBQ0E7V0FDQTtXQUNBO1dBQ0E7V0FDQTtXQUNBO1dBQ0E7O1dBRUE7V0FDQTtXQUNBO1dBQ0E7V0FDQTtXQUNBO1dBQ0E7O1dBRUE7V0FDQTtXQUNBLGVBQWUsK0JBQStCO1dBQzlDO1dBQ0E7O1dBRUE7V0FDQTtXQUNBO1dBQ0E7V0FDQTtXQUNBO1dBQ0EsTUFBTSxvQkFBb0I7V0FDMUI7V0FDQTtXQUNBO1dBQ0E7V0FDQTtXQUNBO1dBQ0E7V0FDQTtXQUNBO1dBQ0E7V0FDQTtXQUNBO1dBQ0E7V0FDQTtXQUNBO1dBQ0E7O1dBRUE7V0FDQTs7V0FFQTtXQUNBO1dBQ0E7O1dBRUE7V0FDQTtXQUNBLCtDOzs7O1VDMUZBO1VBQ0E7VUFDQSIsImZpbGUiOiJNZW51LmpzIiwic291cmNlc0NvbnRlbnQiOlsiKGZ1bmN0aW9uIHdlYnBhY2tVbml2ZXJzYWxNb2R1bGVEZWZpbml0aW9uKHJvb3QsIGZhY3RvcnkpIHtcblx0aWYodHlwZW9mIGV4cG9ydHMgPT09ICdvYmplY3QnICYmIHR5cGVvZiBtb2R1bGUgPT09ICdvYmplY3QnKVxuXHRcdG1vZHVsZS5leHBvcnRzID0gZmFjdG9yeSgpO1xuXHRlbHNlIGlmKHR5cGVvZiBkZWZpbmUgPT09ICdmdW5jdGlvbicgJiYgZGVmaW5lLmFtZClcblx0XHRkZWZpbmUoXCJhcHBcIiwgW10sIGZhY3RvcnkpO1xuXHRlbHNlIGlmKHR5cGVvZiBleHBvcnRzID09PSAnb2JqZWN0Jylcblx0XHRleHBvcnRzW1wiYXBwXCJdID0gZmFjdG9yeSgpO1xuXHRlbHNlXG5cdFx0cm9vdFtcImFwcFwiXSA9IGZhY3RvcnkoKTtcbn0pKHNlbGYsIGZ1bmN0aW9uKCkge1xucmV0dXJuICIsImltcG9ydCAqIGFzIFJlYWN0IGZyb20gJ3JlYWN0JztcclxuaW1wb3J0ICogYXMgUmVhY3RET00gZnJvbSAncmVhY3QtZG9tJztcclxuaW1wb3J0ICBNZW51Q29tcG9uZW50IGZyb20gJy4vY29tcG9uZW50cy9NZW51L01lbnVDb21wb25lbnQnO1xyXG5cclxuXHJcblJlYWN0RE9NLnJlbmRlcig8TWVudUNvbXBvbmVudCAvPixkb2N1bWVudC5nZXRFbGVtZW50QnlJZChcIk1lbnVDb21wb25lbnRcIikpXHJcbiIsImltcG9ydCAqIGFzIFJlYWN0IGZyb20gJ3JlYWN0JztcclxuXHJcbmltcG9ydCBheGlvcyBmcm9tICdheGlvcyc7XHJcbi8vIGRlY2xhcmUgZ2xvYmFsIHtcclxuLy8gICAgIGludGVyZmFjZSBXaW5kb3cge1xyXG4vLyAgICAgICAgIGlzRFNQOmFueTtcclxuLy8gICAgICAgICBpc0RhdGFQcm92aWRlcjphbnk7XHJcbi8vICAgICAgICAgaXNOb3JtYWw6YW55O1xyXG4vLyAgICAgICAgIFJlcG9ydFNjaGVkdWxlQ291bnQ6YW55O1xyXG4vLyAgICAgICAgIFBNUERlYWxDb3VudDphbnk7XHJcbi8vICAgICAgICAgZXh0ZXJuYWxMaXN0OmFueTtcclxuLy8gXHRcdHZpc2liaWxpdHlWYWx1ZTphbnk7XHJcbi8vIFx0XHRkYXNoQm9hcmRVcmw6YW55O1xyXG4vLyBcdFx0ZGFzaEJvYXJkVXJsbG1wcmVzc2lvbmxvZzphbnk7XHJcbi8vIFx0XHRJbXByZXNzaW9uTG9nc1VybDphbnk7XHJcbi8vICAgICB9XHJcbi8vIH1cclxubGV0IGxpc3QgPSBudWxsO1xyXG5cclxuZXhwb3J0IGRlZmF1bHQgY2xhc3MgTWVudUNvbXBvbmVudCBleHRlbmRzIFJlYWN0LkNvbXBvbmVudHtcclxuXHRjb25zdHJ1Y3Rvcihwcm9wcyl7XHJcbiAgICAgICAgc3VwZXIocHJvcHMpO1xyXG5cdFx0dGhpcy5zdGF0ZSA9IHtcclxuXHRcdFx0ZXh0ZXJuYWxMaXN0IDogbnVsbFxyXG4gICAgICAgIH07XHJcblx0fTtcclxuXHJcblx0XHJcblx0Y29tcG9uZW50V2lsbE1vdW50ID0gKCkgPT4ge1xyXG5cdFx0Ly92YXIgZXh0ZXJuYWxMaXN0ID0gbnVsbDtcclxuXHRcdGxldCBzZWxmPXRoaXM7XHJcblx0XHRheGlvcy5nZXQoXCIvZW4vVXNlci9HZXRFeHRlcm5hbERhdGFQcm92aWRlclF1ZXJ5UmVzdWx0QWxsUmVzdWx0QWN0aW9uUmVzdWx0XCIpXHJcblx0XHQudGhlbihmdW5jdGlvbihyZXNwb25zZSApe1xyXG5cdFx0ICAgLy8gaGFuZGxlIHN1Y2Nlc3NcclxuXHRcdCAgIC8vZGVidWdnZXI7XHJcblx0XHQgICAvL2V4dGVybmFsTGlzdCA9IHJlc3BvbnNlLmRhdGE7XHJcblx0XHRzZWxmLnNldFN0YXRlKHsgZXh0ZXJuYWxMaXN0IDogcmVzcG9uc2UuZGF0YX0pO1xyXG5cdFx0ICAgY29uc29sZS5sb2cocmVzcG9uc2UpO1xyXG5cdFx0fSk7XHJcblx0fVxyXG5cdGNhbGxmdW5jdCA9IChmdW5jTmFtZSkgPT4ge1xyXG5cdFx0Y29uc29sZS5sb2coZnVuY05hbWUpO1xyXG5cdFx0ZXZhbChmdW5jTmFtZSk7XHJcblx0fVxyXG5cclxuXHRnZXRUcmFuc2xhdGlvbnMgPSAoKSAgPT4ge1xyXG5cdFx0Y29uc3QgZWxlbWVudCA9IHdpbmRvdy5kb2N1bWVudC5xdWVyeVNlbGVjdG9yKCdbZGF0YS10cmFuc2xhdGlvbnNdJyk7XHJcblx0XHRpZiAoZWxlbWVudCkge1xyXG5cdFx0XHRyZXR1cm4gSlNPTi5wYXJzZShlbGVtZW50LmdldEF0dHJpYnV0ZSgnZGF0YS10cmFuc2xhdGlvbnMnKSk7XHJcblx0XHR9XHJcblx0XHRcclxuXHRcdHJldHVybiB7fSA7XHJcblx0fTtcclxuXHRyZW5kZXJFeHRlcm5hbExpc3QgPSAoZXh0ZXJuYWxMaXN0KSA9PntcclxuXHRcdGxldCBsaUxpc3QgPSBbXTtcclxuXHJcblx0XHRsZXQgVHJhbnNsYXRpb25zQ29udGV4dCA9IFJlYWN0LmNyZWF0ZUNvbnRleHQodGhpcy5nZXRUcmFuc2xhdGlvbnMoKSk7XHJcblx0XHRleHRlcm5hbExpc3QucmVzdWx0LmZvckVhY2goZWxlbWVudCA9PiB7XHJcblx0XHRcdGxpTGlzdC5wdXNoKFxyXG5cdFx0XHRcdDxUcmFuc2xhdGlvbnNDb250ZXh0LkNvbnN1bWVyPlxyXG5cdFx0XHRcdHsgdCA9PiBcclxuXHRcdFx0XHRcdDxsaSBpZD1cIkRhdGFQcm92aWRlck1lbnVOYW1lTGlua1wiIGtleT17ZWxlbWVudC5JRH0+XHJcblx0XHRcdFx0XHQ8YSBpZD1cIkRhdGFQcm92aWRlck1lbnVOYW1lXCI+e2VsZW1lbnQuTmFtZX08L2E+XHJcblxyXG5cdFx0XHRcdFx0PHVsIGNsYXNzTmFtZT1cImRyb3Bkb3duQWRGYWxjb24gbmV4dGxldmVsXCIgc3R5bGU9e3tsZWZ0OjE5ICsgJ3B4J319PlxyXG5cdFx0XHRcdFx0XHQ8bGkgaWQ9e1wiRGF0YVByb3ZpZGVyTWVudUZvcmdldHBhc3N3b3JkXCIrZWxlbWVudC5JRH0gICBzdHlsZT17e2Rpc3BsYXk6J25vbmUnfX0+PGEgICBpZD1cIkRhdGFQcm92aWRlck1lbnVGb3JnZXRwYXNzd29yZFVSTFwiIGhyZWY9e1wiL2VuL0RhdGFQcm92aWRlci9MYW5kaW5nRGF0YVByb3ZpZGVyP0lkID1cIitlbGVtZW50LklEK1wiJlNvdXJjZT1Gb3JnZXRwYXNzd29yZFwifT57dC5jb21wb25lbnRUcmFuc2xhdGlvbnMuRm9yZ2V0cGFzc3dvcmRfVHJhbn08L2E+PC9saT5cclxuXHRcdFx0XHRcdFx0PGxpIGlkPXtcIkRhdGFQcm92aWRlck1lbnVDaGFuZ2VQYXNzd29yZFwiK2VsZW1lbnQuSUR9ICBzdHlsZT17e2Rpc3BsYXk6J25vbmUnfX0+PGEgIGlkPVwiRGF0YVByb3ZpZGVyTWVudUNoYW5nZVBhc3N3b3JkVVJMXCIgaHJlZj17XCIvZW4vRGF0YVByb3ZpZGVyL0xhbmRpbmdEYXRhUHJvdmlkZXI/SWQgPVwiK2VsZW1lbnQuSUQrXCImU291cmNlPUNoYW5nZVBhc3N3b3JkXCJ9Pnt0LmNvbXBvbmVudFRyYW5zbGF0aW9ucy5DaGFuZ2VQYXNzd29yZF9UcmFufTwvYT48L2xpPlxyXG5cclxuXHRcdFx0XHRcdFx0PGxpIGlkPXtcIkRhdGFQcm92aWRlck1lbnVMb2dpblwiK2VsZW1lbnQuSUR9PjxhICAgaWQ9XCJEYXRhUHJvdmlkZXJNZW51TG9naW5VUkxcIiBocmVmPXtcIi9lbi9EYXRhUHJvdmlkZXIvTGFuZGluZ0RhdGFQcm92aWRlcj9JZCA9XCIrZWxlbWVudC5JRCtcIiZTb3VyY2U9TG9naW5cIn0+e3QuY29tcG9uZW50VHJhbnNsYXRpb25zLkxvZ2luX1RyYW59PC9hPjwvbGk+XHJcblx0XHRcdFx0XHRcdDxsaSBpZD17XCJEYXRhUHJvdmlkZXJNZW51UmVnaXN0ZXJcIitlbGVtZW50LklEfSAgc3R5bGU9e3tkaXNwbGF5Oidub25lJ319PjxhICAgaWQ9XCJEYXRhUHJvdmlkZXJNZW51UmVnaXN0ZXJVUkxcIiBocmVmPXtcIi9lbi9EYXRhUHJvdmlkZXIvTGFuZGluZ0RhdGFQcm92aWRlcj9JZCA9XCIrZWxlbWVudC5JRCtcIiZTb3VyY2U9UmVnaXN0ZXJcIn0+e3QuY29tcG9uZW50VHJhbnNsYXRpb25zLlJlZ2lzdGVyX1RyYW59PC9hPjwvbGk+XHJcblxyXG5cdFx0XHRcdFx0XHQ8bGkgaWQ9e1wiRGF0YVByb3ZpZGVyTWVudUF1ZGlhbmNlXCIrZWxlbWVudC5JRH0gIHN0eWxlPXt7ZGlzcGxheTonbm9uZSd9fT48YSAgIGlkPVwiRGF0YVByb3ZpZGVyTWVudUF1ZGlhbmNlVVJMXCIgaHJlZj17XCIvZW4vRGF0YVByb3ZpZGVyL0xhbmRpbmdEYXRhUHJvdmlkZXI/SWQgPVwiK2VsZW1lbnQuSUQrXCImU291cmNlPUF1ZGlhbmNlXCJ9Pnt0LmNvbXBvbmVudFRyYW5zbGF0aW9ucy5BdWRpZW5jZUxpc3RzX1RyYW59PC9hPjwvbGk+XHJcblxyXG5cdFx0XHRcdFx0XHQ8bGkgaWQ9e1wiRGF0YVByb3ZpZGVyTWVudUxvZ091dFwiK2VsZW1lbnQuSUR9ICBzdHlsZT17e2Rpc3BsYXk6J25vbmUnfX0+PGEgY2xhc3NOYW1lPVwiXCIgaHJlZj1cIiNcIiBvbkNsaWNrPXsoKSA9PiB0aGlzLmNhbGxmdW5jdChcIkhhbmRsZURhdGFQcm92aWRlck1lbnVMb2dPdXRcIitlbGVtZW50LklEK1wiKCk7XCIpfSAgPnt0LmNvbXBvbmVudFRyYW5zbGF0aW9ucy5Mb2dvdXRfVHJhbn08L2E+PC9saT5cclxuXHRcdFx0XHRcdFxyXG5cdFx0XHRcdFx0XHRcclxuXHRcdFx0XHRcdDwvdWw+XHJcblx0XHRcdFx0PC9saT5cclxuXHRcdFx0XHR9XHJcblx0XHRcdFx0PC9UcmFuc2xhdGlvbnNDb250ZXh0LkNvbnN1bWVyPlxyXG5cdFx0XHQpO1xyXG5cdFx0fSk7XHJcblxyXG5cdFx0cmV0dXJuIGxpTGlzdDtcclxuXHRcdFxyXG5cdH1cclxuXHRyZXVuZGVyTWVudSA9ICgpID0+e1xyXG5cdFx0dmFyIGV4dGVybmFsTGlzdCA9IHRoaXMuc3RhdGUuZXh0ZXJuYWxMaXN0O1xyXG4gXHRcdGxldCBUcmFuc2xhdGlvbnNDb250ZXh0ID0gUmVhY3QuY3JlYXRlQ29udGV4dCh0aGlzLmdldFRyYW5zbGF0aW9ucygpKTtcclxuXHRcdGlmKHdpbmRvdy5pc0RTUCl7XHJcblx0XHQvL2lmKHRydWUpe1xyXG5cdFx0XHRcclxuXHRcdFx0cmV0dXJuIChcclxuXHRcdFx0XHQ8VHJhbnNsYXRpb25zQ29udGV4dC5Db25zdW1lcj5cclxuXHRcdFx0XHR7IHQgPT4gXHJcblx0XHRcdFx0PHVsIGlkPVwibWVudS1uYXZcIiBzdHlsZT17e3Zpc2liaWxpdHk6IHdpbmRvdy52aXNpYmlsaXR5VmFsdWV9fT5cclxuXHRcdFx0XHRcdDxsaSBpZD1cIkxpc3RNZW51RGFzaGJvYXJkXCI+XHJcblx0XHRcdFx0XHRcdDxhIGNsYXNzTmFtZT1cInBhcmVudCBncmFkaWVudFwiIGhyZWY9e3dpbmRvdy5kYXNoYm9hcmRhZFVybH0+e3QuY29tcG9uZW50VHJhbnNsYXRpb25zLkRhc2hib2FyZF9UcmFufTwvYT5cclxuXHRcdFx0XHRcdDwvbGk+XHJcblx0XHRcdFx0XHQ8bGkgaWQ9XCJMaXN0TWVudUFkdmVydGlzZXJcIj5cclxuXHRcdFx0XHRcdFx0PGEgY2xhc3NOYW1lPVwicGFyZW50IGdyYWRpZW50XCIgaHJlZj17d2luZG93LkFjY291bnRBZHZlcnRpc2Vyc1VybH0+e3QuY29tcG9uZW50VHJhbnNsYXRpb25zLkFkdmVydGlzZXJzX1RyYW59PC9hPlxyXG5cdFx0XHJcblx0XHRcdFx0XHRcdDx1bCBjbGFzc05hbWU9XCJkcm9wZG93bkFkRmFsY29uXCI+XHJcblx0XHRcdFx0XHRcdFx0XHJcblx0XHRcdFx0XHRcdFx0e3dpbmRvdy5UcmFmZmljUGxhbm5lckNvdW50ID09IHRydWU/KFxyXG5cdFx0XHRcdFx0XHRcdFx0PGxpPjxhIGhyZWY9XCIvZW4vQ2FtcGFpZ24vVHJhZmZpY1BsYW5uZXJcIj57dC5jb21wb25lbnRUcmFuc2xhdGlvbnMuVHJhZmZpY1BsYW5uZXJfVHJhbn08L2E+PC9saT5cclxuXHRcdFx0XHRcdFx0XHQpOm51bGxcclxuXHRcdFx0XHRcdFx0XHR9XHJcblx0XHRcdFx0XHRcdFx0PGxpPjxhIGNsYXNzTmFtZT1cIlwiIGhyZWY9e3dpbmRvdy5NYXN0ZXJBcHBTaXRlc1VybH0+e3QuY29tcG9uZW50VHJhbnNsYXRpb25zLkNvbnRlbnRMaXN0c19UcmFufTwvYT48L2xpPlxyXG5cdFx0XHRcdFx0XHQ8L3VsPlxyXG5cdFx0XHRcdFx0PC9saT5cclxuXHRcdFx0XHRcdDxsaSBpZD1cIkxpc3RNZW51UmVwb3J0c1wiPlxyXG5cdFx0XHJcblx0XHRcdFx0XHRcdHtcclxuXHRcdFx0XHRcdFx0XHR3aW5kb3cuUmVwb3J0U2NoZWR1bGVDb3VudCA9PSB0cnVlPzxhIGNsYXNzTmFtZT1cInBhcmVudCBncmFkaWVudFwiIGhyZWY9XCIvZW4vcmVwb3J0cy9JbmRleFJlcG9ydHNKb2I/cmVwb3J0VHlwZT1hZFwiPnt0LmNvbXBvbmVudFRyYW5zbGF0aW9ucy5SZXBvcnRzX1RyYW59PC9hPjo8YSBjbGFzc05hbWU9XCJwYXJlbnQgZ3JhZGllbnRcIiBocmVmPVwiL2VuL3JlcG9ydHM/cmVwb3J0VHlwZT1hZFwiPnt0LmNvbXBvbmVudFRyYW5zbGF0aW9ucy5SZXBvcnRzX1RyYW59PC9hPlxyXG5cdFx0XHRcdFx0XHR9XHJcblx0XHRcdFx0XHQ8L2xpPlxyXG5cdFx0XHRcdFx0XHRcclxuXHRcdFx0XHRcdFx0XHJcblx0XHRcdFx0XHRcdFxyXG5cdFx0XHRcdFx0XHR7XHJcblx0XHRcclxuXHRcdFx0XHRcdFx0XHR3aW5kb3cuUE1QRGVhbENvdW50ID09IHRydWU/XHJcblx0XHRcdFx0XHRcdFx0XHQoPGxpIGlkPVwiTGlzdE1lbnVEZWFsc1wiPlxyXG5cdFx0XHRcdFx0XHRcdFx0XHQ8YSBjbGFzc05hbWU9XCJwYXJlbnQgZ3JhZGllbnRcIj57dC5jb21wb25lbnRUcmFuc2xhdGlvbnMuRGVhbHNfVHJhbn08L2E+XHJcblxyXG5cdFx0XHRcdFx0XHRcdFx0XHQ8dWwgY2xhc3NOYW1lPVwiZHJvcGRvd25BZEZhbGNvbiBcIiA+XHJcblx0XHRcdFx0XHRcdFx0XHRcdFx0PGxpPjxhIGNsYXNzTmFtZT1cIlwiIGhyZWY9XCIvZW4vZGVhbHMvTWVudT9pZD1cIj57dC5jb21wb25lbnRUcmFuc2xhdGlvbnMuUE1QRGVhbHNfVHJhbn08L2E+PC9saT5cclxuXHRcdFx0XHRcdFx0XHRcdFx0XHQ8bGk+PGEgY2xhc3NOYW1lPVwiXCIgaHJlZj1cIi9lbi9kYXNoYm9hcmQ/Y2hhcnRUeXBlPWRlYWxcIj57dC5jb21wb25lbnRUcmFuc2xhdGlvbnMuRGVhbE1vbml0b3JpbmdfVHJhbn08L2E+PC9saT5cclxuXHRcdFx0XHRcdFx0XHRcdFx0PC91bD5cclxuXHRcdFx0XHRcdFx0XHRcdDwvbGk+KTpudWxsXHJcblx0XHRcdFx0XHRcdFx0XHRcclxuXHRcdFx0XHRcdFx0fVxyXG5cdFx0XHRcdFxyXG5cdFx0XHRcdFx0XHR7XHJcblx0XHRcdFx0XHRcdFx0ZXh0ZXJuYWxMaXN0ICAhPSBudWxsICYmIGV4dGVybmFsTGlzdC5yZXN1bHQubGVuZ3RoID4gMCA/IChcclxuXHRcdFx0XHRcdFx0XHRcdDxsaSBpZD1cIkxpc3RNZW51RGF0YVByb3ZpZGVyc1wiPlxyXG5cdFx0XHRcdFx0XHRcdFx0PGEgY2xhc3NOYW1lPVwicGFyZW50IGdyYWRpZW50XCIgaHJlZj1cIiNcIj57dC5jb21wb25lbnRUcmFuc2xhdGlvbnMuUHJvdmlkZXJzX1RyYW59PC9hPlxyXG5cdFx0XHRcdFx0XHRcdFx0PHVsIGNsYXNzTmFtZT1cImRyb3Bkb3duQWRGYWxjb25cIj5cclxuXHRcclxuXHRcdFx0XHRcdFx0XHRcdHtcclxuXHRcdFx0XHRcdFx0XHRcdFx0dGhpcy5yZW5kZXJFeHRlcm5hbExpc3QoZXh0ZXJuYWxMaXN0KS5tYXAoXHJcblx0XHRcdFx0XHRcdFx0XHRcdFx0KGxpKSA9PiB7XHJcblx0XHRcdFx0XHRcdFx0XHRcdFx0XHRyZXR1cm4gbGk7XHJcblx0XHRcdFx0XHRcdFx0XHRcdFx0fVxyXG5cdFx0XHRcdFx0XHRcdFx0XHQpXHJcblx0XHRcdFx0XHRcdFx0XHR9XHJcblx0XHRcdFx0XHRcdFx0XHQgIFxyXG5cdFxyXG5cdFx0XHRcdFx0XHRcdFx0PC91bD5cclxuXHRcdFx0XHRcdFx0XHQ8L2xpPlxyXG5cdFx0XHRcdFx0XHRcdFxyXG5cdFx0XHRcdFx0XHRcdCk6IG51bGxcclxuXHRcdFx0XHRcdFx0fVxyXG5cdFx0XHRcdFxyXG5cdFxyXG5cdFx0XHQ8L3VsPlxyXG5cdFx0XHR9XHJcblx0XHRcdDwvVHJhbnNsYXRpb25zQ29udGV4dC5Db25zdW1lcj5cclxuXHRcdFx0KVxyXG5cdFx0fVxyXG5cdFx0ZWxzZSBpZih3aW5kb3cuaXNEYXRhUHJvdmlkZXIpe1xyXG5cdFx0XHRyZXR1cm4gKFxyXG5cdFx0XHRcdDxUcmFuc2xhdGlvbnNDb250ZXh0LkNvbnN1bWVyPlxyXG5cdFx0XHRcdHsgdCA9PiBcclxuXHRcdFx0XHQ8dWwgaWQ9XCJtZW51LW5hdlwiIHN0eWxlPXt7dmlzaWJpbGl0eTogd2luZG93LnZpc2liaWxpdHlWYWx1ZX19PlxyXG5cdFx0XHRcdFx0PGxpIGlkPVwiTGlzdE1lbnVEYXNoYm9hcmRcIj5cclxuXHRcdFx0XHRcdFx0PGEgY2xhc3NOYW1lPVwicGFyZW50IGdyYWRpZW50XCIgaHJlZj17d2luZG93LmRhc2hCb2FyZFVybGxtcHJlc3Npb25sb2d9Pnt0LmNvbXBvbmVudFRyYW5zbGF0aW9ucy5EYXNoYm9hcmRfVHJhbn08L2E+XHJcblx0XHRcdFx0XHQ8L2xpPlxyXG5cdFx0XHRcdFx0PGxpIGlkPVwiTGlzdE1lbnVBZHZlcnRpc2VyXCI+XHJcblx0XHRcdFx0XHRcdDxhIGNsYXNzTmFtZT1cInBhcmVudCBncmFkaWVudFwiIGhyZWY9e3dpbmRvdy5JbXByZXNzaW9uTG9nc1VybH0+e3QuY29tcG9uZW50VHJhbnNsYXRpb25zLkltcHJlc3Npb25Mb2dzX1RyYW59PC9hPlxyXG5cdFx0XHRcdFx0PC9saT5cclxuXHJcblx0XHRcdFx0PC91bD5cclxuXHRcdFx0XHR9XHJcblx0XHRcdFx0PC9UcmFuc2xhdGlvbnNDb250ZXh0LkNvbnN1bWVyPlxyXG5cdFx0XHQpXHJcblx0XHR9ZWxzZXtcclxuXHRcdFx0cmV0dXJuIChcclxuXHRcdFx0PFRyYW5zbGF0aW9uc0NvbnRleHQuQ29uc3VtZXI+XHJcblx0XHRcdFx0eyB0ID0+IFxyXG5cdFx0XHRcdDx1bCBpZD1cIm1lbnUtbmF2XCIgc3R5bGU9e3t2aXNpYmlsaXR5OiB3aW5kb3cudmlzaWJpbGl0eVZhbHVlfX0+XHJcblx0XHRcdFx0XHQgPGxpIGlkPVwiTGlzdE1lbnVBZHZlcnRpc2VyXCI+XHJcblx0XHRcdFx0XHRcdDxhIGNsYXNzTmFtZT1cInBhcmVudCBncmFkaWVudFwiIGhyZWY9e3dpbmRvdy5kYXNoQm9hcmRVcmx9Pnt0LmNvbXBvbmVudFRyYW5zbGF0aW9ucy5BZHZlcnRpc2VyX1RyYW59PC9hPlxyXG5cdFx0XHRcdFx0XHQ8dWwgY2xhc3NOYW1lPVwiZHJvcGRvd25BZEZhbGNvblwiPlxyXG5cdFx0XHRcdFx0XHRcclxuXHRcdFx0XHRcdFx0XHQ8bGk+PGEgaHJlZj1cIi9lbi9kYXNoYm9hcmQ/Y2hhcnRUeXBlPWFkXCI+e3QuY29tcG9uZW50VHJhbnNsYXRpb25zLkRhc2hib2FyZF9UcmFufTwvYT48L2xpPlxyXG5cdFx0XHRcdFx0XHRcclxuXHRcdFx0XHRcdFx0XHQ8bGk+PGEgaHJlZj1cIi9lbi9jYW1wYWlnbi9BY2NvdW50QWR2ZXJ0aXNlcnNcIj57dC5jb21wb25lbnRUcmFuc2xhdGlvbnMuQWR2ZXJ0aXNlcnNfVHJhbn08L2E+PC9saT5cclxuXHRcdFx0XHRcdFx0XHQ8bGk+PGEgY2xhc3NOYW1lPVwiXCIgaHJlZj1cIi9lbi9jYW1wYWlnbi9NYXN0ZXJBcHBTaXRlc1wiPnt0LmNvbXBvbmVudFRyYW5zbGF0aW9ucy5Db250ZW50TGlzdHNfVHJhbn08L2E+PC9saT5cclxuXHJcblx0XHRcdFx0XHRcdFx0e1xyXG5cdFx0XHRcdFx0XHRcdFx0d2luZG93LlJlcG9ydFNjaGVkdWxlQ291bnQgPT0gdHJ1ZT88bGk+PGEgaHJlZj1cIi9lbi9yZXBvcnRzL0luZGV4UmVwb3J0c0pvYj9yZXBvcnRUeXBlPWFkXCI+e3QuY29tcG9uZW50VHJhbnNsYXRpb25zLlJlcG9ydHNfVHJhbn08L2E+PC9saT46PGxpPjxhIGhyZWY9XCIvZW4vcmVwb3J0cz9yZXBvcnRUeXBlPWFkXCI+e3QuY29tcG9uZW50VHJhbnNsYXRpb25zLlJlcG9ydHNfVHJhbn08L2E+PC9saT5cclxuXHRcdFx0XHRcdFx0XHR9XHJcblx0XHRcdFx0XHRcdFx0XHRcclxuXHRcdFx0XHRcdFx0XHRcdFxyXG5cdFx0XHRcdFx0XHRcdHtcclxuXHRcdFx0XHRcdFx0XHRcdHdpbmRvdy5QTVBEZWFsQ291bnQgPT0gdHJ1ZT9cclxuXHRcdFx0XHRcdFx0XHRcdCg8bGkgY2xhc3NOYW1lPVwibWVudS1sYXN0XCI+XHJcblx0XHRcdFx0XHRcdFx0XHRcdDxhPnt0LmNvbXBvbmVudFRyYW5zbGF0aW9ucy5EZWFsc19UcmFufTwvYT5cclxuXHJcblx0XHRcdFx0XHRcdFx0XHRcdDx1bCBjbGFzc05hbWU9XCJkcm9wZG93bkFkRmFsY29uIG5leHRsZXZlbFwiIHN0eWxlPXt7bGVmdDoxOTB9fT5cclxuXHRcdFx0XHRcdFx0XHRcdFx0XHQ8bGk+PGEgY2xhc3NOYW1lPVwiXCIgaHJlZj1cIi9lbi9kZWFscz9pZFwiPnt0LmNvbXBvbmVudFRyYW5zbGF0aW9ucy5QTVBEZWFsc19UcmFufTwvYT48L2xpPlxyXG5cdFx0XHRcdFx0XHRcdFx0XHRcdDxsaT48YSBjbGFzc05hbWU9XCJcIiBocmVmPVwiL2VuL2Rhc2hib2FyZD9jaGFydFR5cGU9ZGVhbFwiPnt0LmNvbXBvbmVudFRyYW5zbGF0aW9ucy5EZWFsTW9uaXRvcmluZ19UcmFufTwvYT48L2xpPlxyXG5cdFx0XHRcdFx0XHRcdFx0XHQ8L3VsPlxyXG5cdFx0XHRcdFx0XHRcdFx0PC9saT4pOm51bGxcclxuXHRcdFx0XHRcdFx0XHR9XHJcblx0XHRcdFx0XHRcdDwvdWw+XHJcblx0XHRcdFx0XHQ8L2xpPlxyXG5cdFx0XHRcdFx0XHRcdHsgIFxyXG5cdFx0XHRcdFx0XHRcdFx0d2luZG93LmlzRFNQID09IGZhbHNlP1xyXG5cdFx0XHRcdFx0XHRcdFx0KFxyXG5cdFx0XHRcdFx0XHRcdFx0XHQ8bGkgaWQ9XCJMaXN0TWVudVB1Ymxpc2hlclwiPlxyXG5cdFx0XHRcdFx0XHRcdFx0XHRcdDxhIGNsYXNzTmFtZT1cInBhcmVudCBncmFkaWVudFwiIGhyZWY9XCJcIj57dC5jb21wb25lbnRUcmFuc2xhdGlvbnMuUHVibGlzaGVyX1RyYW59PC9hPlxyXG5cdFx0XHRcdFx0XHRcdFx0XHRcdDx1bCBjbGFzc05hbWU9XCJkcm9wZG93bkFkRmFsY29uXCI+XHJcblx0XHRcdFx0XHRcdFx0XHRcdFx0XHQ8bGk+PGEgaHJlZj1cIi9lbi9kYXNoYm9hcmQ/Y2hhcnRUeXBlPWFwcFwiPnt0LmNvbXBvbmVudFRyYW5zbGF0aW9ucy5EYXNoYm9hcmRfVHJhbn08L2E+PC9saT5cclxuXHRcdFx0XHRcdFx0XHRcdFx0XHRcdDxsaT48YSBocmVmPVwiL2VuL2FwcHNpdGVcIj57dC5jb21wb25lbnRUcmFuc2xhdGlvbnMuQXBwc19UcmFufTwvYT48L2xpPlxyXG5cdFx0XHRcdFx0XHRcdFx0XHRcdFx0PGxpPjxhIGhyZWY9XCIvZW4vSG91c2VBZFwiPnt0LmNvbXBvbmVudFRyYW5zbGF0aW9ucy5Ib3VzZUFkX1RyYW59PC9hPjwvbGk+XHJcblx0XHRcdFx0XHRcdFx0XHRcdFx0XHR7XHJcblx0XHRcdFx0XHRcdFx0XHRcdFx0XHRcdCB3aW5kb3cuUmVwb3J0U2NoZWR1bGVDb3VudD8oXHJcblx0XHRcdFx0XHRcdFx0XHRcdFx0XHRcdFxyXG5cdFx0XHRcdFx0XHRcdFx0XHRcdFx0XHRcdDxsaT48YSBocmVmPVwiL2VuL3JlcG9ydHMvSW5kZXhSZXBvcnRzSm9iP3JlcG9ydFR5cGU9YWRcIj57dC5jb21wb25lbnRUcmFuc2xhdGlvbnMuUmVwb3J0c19UcmFufTwvYT48L2xpPlxyXG5cclxuXHRcdFx0XHRcdFx0XHRcdFx0XHRcdFx0IClcclxuXHRcdFx0XHRcdFx0XHRcdFx0XHRcdFx0OlxyXG5cdFx0XHRcdFx0XHRcdFx0XHRcdFx0XHQoXHJcblx0XHRcdFx0XHRcdFx0XHRcdFx0XHRcdFx0PGxpPjxhIGhyZWY9XCIvZW4vcmVwb3J0cz9yZXBvcnRUeXBlPWFkXCI+e3QuY29tcG9uZW50VHJhbnNsYXRpb25zLlJlcG9ydHNfVHJhbn08L2E+PC9saT5cclxuXHJcblx0XHRcdFx0XHRcdFx0XHRcdFx0XHRcdClcclxuXHRcdFx0XHRcdFx0XHRcdFx0XHRcdH1cclxuXHJcblxyXG5cdFx0XHRcdFx0XHRcdFx0XHRcdFx0PGxpIGNsYXNzTmFtZT1cIm1lbnUtbGFzdFwiPjxhIGhyZWY9XCJodHRwczovL2xvY2FsaG9zdDo0NDMyNC9lbi9kb3dubG9hZC1zZGsuaHRtbFwiPnt0LmNvbXBvbmVudFRyYW5zbGF0aW9ucy5TREtzX1RyYW59PC9hPjwvbGk+XHJcblx0XHRcdFx0XHRcdFx0XHRcdFx0PC91bD5cclxuXHRcdFx0XHRcdFx0XHRcdFx0PC9saT5cclxuXHRcdFx0XHRcdFx0XHRcdFx0XHJcblx0XHRcdFx0XHRcdFx0XHQpOm51bGxcclxuXHRcdFx0XHRcdFx0fVxyXG5cclxuXHRcdFx0XHRcdFxyXG5cclxuXHJcbiAgICAgICAgXHRcdDwvdWw+XHJcblx0XHRcdFx0fVxyXG5cdFx0XHRcdDwvVHJhbnNsYXRpb25zQ29udGV4dC5Db25zdW1lcj5cclxuXHRcdFx0KVxyXG5cdFx0fVxyXG5cdH1cclxuXHJcblxyXG5cdHJlbmRlcigpe1xyXG5cdFx0XHJcblx0XHRyZXR1cm4gKFxyXG5cdFx0XHR0aGlzLnJldW5kZXJNZW51KClcclxuXHRcdClcclxuXHR9XHJcbn0iLCJtb2R1bGUuZXhwb3J0cyA9IChfX3dlYnBhY2tfcmVxdWlyZV9fKC8qISBkbGwtcmVmZXJlbmNlIHZlbmRvcl9saWJfYWZhY2FjYzNiOGRhMDViODlmZDcgKi8gXCJkbGwtcmVmZXJlbmNlIHZlbmRvcl9saWJfYWZhY2FjYzNiOGRhMDViODlmZDdcIikpKFwiLi9ub2RlX21vZHVsZXMvcmVhY3QtZG9tL2luZGV4LmpzXCIpOyIsIm1vZHVsZS5leHBvcnRzID0gKF9fd2VicGFja19yZXF1aXJlX18oLyohIGRsbC1yZWZlcmVuY2UgdmVuZG9yX2xpYl9hZmFjYWNjM2I4ZGEwNWI4OWZkNyAqLyBcImRsbC1yZWZlcmVuY2UgdmVuZG9yX2xpYl9hZmFjYWNjM2I4ZGEwNWI4OWZkN1wiKSkoXCIuL25vZGVfbW9kdWxlcy9yZWFjdC9pbmRleC5qc1wiKTsiLCJtb2R1bGUuZXhwb3J0cyA9IHZlbmRvcl9saWJfYWZhY2FjYzNiOGRhMDViODlmZDc7IiwiLy8gVGhlIG1vZHVsZSBjYWNoZVxudmFyIF9fd2VicGFja19tb2R1bGVfY2FjaGVfXyA9IHt9O1xuXG4vLyBUaGUgcmVxdWlyZSBmdW5jdGlvblxuZnVuY3Rpb24gX193ZWJwYWNrX3JlcXVpcmVfXyhtb2R1bGVJZCkge1xuXHQvLyBDaGVjayBpZiBtb2R1bGUgaXMgaW4gY2FjaGVcblx0aWYoX193ZWJwYWNrX21vZHVsZV9jYWNoZV9fW21vZHVsZUlkXSkge1xuXHRcdHJldHVybiBfX3dlYnBhY2tfbW9kdWxlX2NhY2hlX19bbW9kdWxlSWRdLmV4cG9ydHM7XG5cdH1cblx0Ly8gQ3JlYXRlIGEgbmV3IG1vZHVsZSAoYW5kIHB1dCBpdCBpbnRvIHRoZSBjYWNoZSlcblx0dmFyIG1vZHVsZSA9IF9fd2VicGFja19tb2R1bGVfY2FjaGVfX1ttb2R1bGVJZF0gPSB7XG5cdFx0Ly8gbm8gbW9kdWxlLmlkIG5lZWRlZFxuXHRcdC8vIG5vIG1vZHVsZS5sb2FkZWQgbmVlZGVkXG5cdFx0ZXhwb3J0czoge31cblx0fTtcblxuXHQvLyBFeGVjdXRlIHRoZSBtb2R1bGUgZnVuY3Rpb25cblx0X193ZWJwYWNrX21vZHVsZXNfX1ttb2R1bGVJZF0obW9kdWxlLCBtb2R1bGUuZXhwb3J0cywgX193ZWJwYWNrX3JlcXVpcmVfXyk7XG5cblx0Ly8gUmV0dXJuIHRoZSBleHBvcnRzIG9mIHRoZSBtb2R1bGVcblx0cmV0dXJuIG1vZHVsZS5leHBvcnRzO1xufVxuXG4vLyBleHBvc2UgdGhlIG1vZHVsZXMgb2JqZWN0IChfX3dlYnBhY2tfbW9kdWxlc19fKVxuX193ZWJwYWNrX3JlcXVpcmVfXy5tID0gX193ZWJwYWNrX21vZHVsZXNfXztcblxuIiwiLy8gZ2V0RGVmYXVsdEV4cG9ydCBmdW5jdGlvbiBmb3IgY29tcGF0aWJpbGl0eSB3aXRoIG5vbi1oYXJtb255IG1vZHVsZXNcbl9fd2VicGFja19yZXF1aXJlX18ubiA9IChtb2R1bGUpID0+IHtcblx0dmFyIGdldHRlciA9IG1vZHVsZSAmJiBtb2R1bGUuX19lc01vZHVsZSA/XG5cdFx0KCkgPT4gbW9kdWxlWydkZWZhdWx0J10gOlxuXHRcdCgpID0+IG1vZHVsZTtcblx0X193ZWJwYWNrX3JlcXVpcmVfXy5kKGdldHRlciwgeyBhOiBnZXR0ZXIgfSk7XG5cdHJldHVybiBnZXR0ZXI7XG59OyIsIi8vIGRlZmluZSBnZXR0ZXIgZnVuY3Rpb25zIGZvciBoYXJtb255IGV4cG9ydHNcbl9fd2VicGFja19yZXF1aXJlX18uZCA9IChleHBvcnRzLCBkZWZpbml0aW9uKSA9PiB7XG5cdGZvcih2YXIga2V5IGluIGRlZmluaXRpb24pIHtcblx0XHRpZihfX3dlYnBhY2tfcmVxdWlyZV9fLm8oZGVmaW5pdGlvbiwga2V5KSAmJiAhX193ZWJwYWNrX3JlcXVpcmVfXy5vKGV4cG9ydHMsIGtleSkpIHtcblx0XHRcdE9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBrZXksIHsgZW51bWVyYWJsZTogdHJ1ZSwgZ2V0OiBkZWZpbml0aW9uW2tleV0gfSk7XG5cdFx0fVxuXHR9XG59OyIsIl9fd2VicGFja19yZXF1aXJlX18ubyA9IChvYmosIHByb3ApID0+IE9iamVjdC5wcm90b3R5cGUuaGFzT3duUHJvcGVydHkuY2FsbChvYmosIHByb3ApIiwiLy8gZGVmaW5lIF9fZXNNb2R1bGUgb24gZXhwb3J0c1xuX193ZWJwYWNrX3JlcXVpcmVfXy5yID0gKGV4cG9ydHMpID0+IHtcblx0aWYodHlwZW9mIFN5bWJvbCAhPT0gJ3VuZGVmaW5lZCcgJiYgU3ltYm9sLnRvU3RyaW5nVGFnKSB7XG5cdFx0T2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIFN5bWJvbC50b1N0cmluZ1RhZywgeyB2YWx1ZTogJ01vZHVsZScgfSk7XG5cdH1cblx0T2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsICdfX2VzTW9kdWxlJywgeyB2YWx1ZTogdHJ1ZSB9KTtcbn07IiwiLy8gbm8gYmFzZVVSSVxuXG4vLyBvYmplY3QgdG8gc3RvcmUgbG9hZGVkIGFuZCBsb2FkaW5nIGNodW5rc1xuLy8gdW5kZWZpbmVkID0gY2h1bmsgbm90IGxvYWRlZCwgbnVsbCA9IGNodW5rIHByZWxvYWRlZC9wcmVmZXRjaGVkXG4vLyBQcm9taXNlID0gY2h1bmsgbG9hZGluZywgMCA9IGNodW5rIGxvYWRlZFxudmFyIGluc3RhbGxlZENodW5rcyA9IHtcblx0XCJNZW51XCI6IDBcbn07XG5cbnZhciBkZWZlcnJlZE1vZHVsZXMgPSBbXG5cdFtcIi4vQ2xpZW50QXBwL01lbnVNb2R1bGUuanN4XCIsXCJ2ZW5kb3JzLW5vZGVfbW9kdWxlc19heGlvc19pbmRleF9qc1wiXVxuXTtcbi8vIG5vIGNodW5rIG9uIGRlbWFuZCBsb2FkaW5nXG5cbi8vIG5vIHByZWZldGNoaW5nXG5cbi8vIG5vIHByZWxvYWRlZFxuXG4vLyBubyBITVJcblxuLy8gbm8gSE1SIG1hbmlmZXN0XG5cbnZhciBjaGVja0RlZmVycmVkTW9kdWxlcyA9ICgpID0+IHtcblxufTtcbmZ1bmN0aW9uIGNoZWNrRGVmZXJyZWRNb2R1bGVzSW1wbCgpIHtcblx0dmFyIHJlc3VsdDtcblx0Zm9yKHZhciBpID0gMDsgaSA8IGRlZmVycmVkTW9kdWxlcy5sZW5ndGg7IGkrKykge1xuXHRcdHZhciBkZWZlcnJlZE1vZHVsZSA9IGRlZmVycmVkTW9kdWxlc1tpXTtcblx0XHR2YXIgZnVsZmlsbGVkID0gdHJ1ZTtcblx0XHRmb3IodmFyIGogPSAxOyBqIDwgZGVmZXJyZWRNb2R1bGUubGVuZ3RoOyBqKyspIHtcblx0XHRcdHZhciBkZXBJZCA9IGRlZmVycmVkTW9kdWxlW2pdO1xuXHRcdFx0aWYoaW5zdGFsbGVkQ2h1bmtzW2RlcElkXSAhPT0gMCkgZnVsZmlsbGVkID0gZmFsc2U7XG5cdFx0fVxuXHRcdGlmKGZ1bGZpbGxlZCkge1xuXHRcdFx0ZGVmZXJyZWRNb2R1bGVzLnNwbGljZShpLS0sIDEpO1xuXHRcdFx0cmVzdWx0ID0gX193ZWJwYWNrX3JlcXVpcmVfXyhfX3dlYnBhY2tfcmVxdWlyZV9fLnMgPSBkZWZlcnJlZE1vZHVsZVswXSk7XG5cdFx0fVxuXHR9XG5cdGlmKGRlZmVycmVkTW9kdWxlcy5sZW5ndGggPT09IDApIHtcblx0XHRfX3dlYnBhY2tfcmVxdWlyZV9fLngoKTtcblx0XHRfX3dlYnBhY2tfcmVxdWlyZV9fLnggPSAoKSA9PiB7XG5cblx0XHR9XG5cdH1cblx0cmV0dXJuIHJlc3VsdDtcbn1cbl9fd2VicGFja19yZXF1aXJlX18ueCA9ICgpID0+IHtcblx0Ly8gcmVzZXQgc3RhcnR1cCBmdW5jdGlvbiBzbyBpdCBjYW4gYmUgY2FsbGVkIGFnYWluIHdoZW4gbW9yZSBzdGFydHVwIGNvZGUgaXMgYWRkZWRcblx0X193ZWJwYWNrX3JlcXVpcmVfXy54ID0gKCkgPT4ge1xuXG5cdH1cblx0Y2h1bmtMb2FkaW5nR2xvYmFsID0gY2h1bmtMb2FkaW5nR2xvYmFsLnNsaWNlKCk7XG5cdGZvcih2YXIgaSA9IDA7IGkgPCBjaHVua0xvYWRpbmdHbG9iYWwubGVuZ3RoOyBpKyspIHdlYnBhY2tKc29ucENhbGxiYWNrKGNodW5rTG9hZGluZ0dsb2JhbFtpXSk7XG5cdHJldHVybiAoY2hlY2tEZWZlcnJlZE1vZHVsZXMgPSBjaGVja0RlZmVycmVkTW9kdWxlc0ltcGwpKCk7XG59O1xuXG4vLyBpbnN0YWxsIGEgSlNPTlAgY2FsbGJhY2sgZm9yIGNodW5rIGxvYWRpbmdcbnZhciB3ZWJwYWNrSnNvbnBDYWxsYmFjayA9IChkYXRhKSA9PiB7XG5cdHZhciBbY2h1bmtJZHMsIG1vcmVNb2R1bGVzLCBydW50aW1lLCBleGVjdXRlTW9kdWxlc10gPSBkYXRhO1xuXHQvLyBhZGQgXCJtb3JlTW9kdWxlc1wiIHRvIHRoZSBtb2R1bGVzIG9iamVjdCxcblx0Ly8gdGhlbiBmbGFnIGFsbCBcImNodW5rSWRzXCIgYXMgbG9hZGVkIGFuZCBmaXJlIGNhbGxiYWNrXG5cdHZhciBtb2R1bGVJZCwgY2h1bmtJZCwgaSA9IDAsIHJlc29sdmVzID0gW107XG5cdGZvcig7aSA8IGNodW5rSWRzLmxlbmd0aDsgaSsrKSB7XG5cdFx0Y2h1bmtJZCA9IGNodW5rSWRzW2ldO1xuXHRcdGlmKF9fd2VicGFja19yZXF1aXJlX18ubyhpbnN0YWxsZWRDaHVua3MsIGNodW5rSWQpICYmIGluc3RhbGxlZENodW5rc1tjaHVua0lkXSkge1xuXHRcdFx0cmVzb2x2ZXMucHVzaChpbnN0YWxsZWRDaHVua3NbY2h1bmtJZF1bMF0pO1xuXHRcdH1cblx0XHRpbnN0YWxsZWRDaHVua3NbY2h1bmtJZF0gPSAwO1xuXHR9XG5cdGZvcihtb2R1bGVJZCBpbiBtb3JlTW9kdWxlcykge1xuXHRcdGlmKF9fd2VicGFja19yZXF1aXJlX18ubyhtb3JlTW9kdWxlcywgbW9kdWxlSWQpKSB7XG5cdFx0XHRfX3dlYnBhY2tfcmVxdWlyZV9fLm1bbW9kdWxlSWRdID0gbW9yZU1vZHVsZXNbbW9kdWxlSWRdO1xuXHRcdH1cblx0fVxuXHRpZihydW50aW1lKSBydW50aW1lKF9fd2VicGFja19yZXF1aXJlX18pO1xuXHRwYXJlbnRDaHVua0xvYWRpbmdGdW5jdGlvbihkYXRhKTtcblx0d2hpbGUocmVzb2x2ZXMubGVuZ3RoKSB7XG5cdFx0cmVzb2x2ZXMuc2hpZnQoKSgpO1xuXHR9XG5cblx0Ly8gYWRkIGVudHJ5IG1vZHVsZXMgZnJvbSBsb2FkZWQgY2h1bmsgdG8gZGVmZXJyZWQgbGlzdFxuXHRpZihleGVjdXRlTW9kdWxlcykgZGVmZXJyZWRNb2R1bGVzLnB1c2guYXBwbHkoZGVmZXJyZWRNb2R1bGVzLCBleGVjdXRlTW9kdWxlcyk7XG5cblx0Ly8gcnVuIGRlZmVycmVkIG1vZHVsZXMgd2hlbiBhbGwgY2h1bmtzIHJlYWR5XG5cdHJldHVybiBjaGVja0RlZmVycmVkTW9kdWxlcygpO1xufVxuXG52YXIgY2h1bmtMb2FkaW5nR2xvYmFsID0gc2VsZltcIndlYnBhY2tDaHVua2FwcFwiXSA9IHNlbGZbXCJ3ZWJwYWNrQ2h1bmthcHBcIl0gfHwgW107XG52YXIgcGFyZW50Q2h1bmtMb2FkaW5nRnVuY3Rpb24gPSBjaHVua0xvYWRpbmdHbG9iYWwucHVzaC5iaW5kKGNodW5rTG9hZGluZ0dsb2JhbCk7XG5jaHVua0xvYWRpbmdHbG9iYWwucHVzaCA9IHdlYnBhY2tKc29ucENhbGxiYWNrOyIsIi8vIG1vZHVsZSBleHBvcnRzIG11c3QgYmUgcmV0dXJuZWQgZnJvbSBydW50aW1lIHNvIGVudHJ5IGlubGluaW5nIGlzIGRpc2FibGVkXG4vLyBydW4gc3RhcnR1cFxucmV0dXJuIF9fd2VicGFja19yZXF1aXJlX18ueCgpO1xuIl0sInNvdXJjZVJvb3QiOiIifQ==