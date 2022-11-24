/* eslint-disable react/destructuring-assignment */
import React, { Component } from 'react';
import { Col, Container, Row } from 'reactstrap';
import { withTranslation } from 'react-i18next';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import TotalProducts from './components/TotalProducts';
import TotalProfit from './components/TotalProfit';
import OrdersToday from './components/OrdersToday';
import Subscriptions from './components/Subscriptions';
import TopSellingProducts from './components/TopSellingProducts';
import BasicCard from './components/BasicCard';
import SalesStatistic from './components/SalesStatistic';
import RecentOrders from './components/RecentOrders';
import ProductSales from './components/ProductSales';
import NewOrders from './components/NewOrders';
import SalesStatistisBar from './components/SalesStatistisBar';
import MyTodos from './components/MyTodos';
import Emails from './components/Emails';
import SalesReport from './components/SalesReport';
import ShortReminders from './components/ShortReminders';
import { deleteNewOrderTableData } from '../../../redux/actions/newOrderTableActions';
import { NewOrderTableProps } from '../../../shared/prop-types/TablesProps';
import { RTLProps } from '../../../shared/prop-types/ReducerProps';
import { editTodoElement, fetchTodoListData } from '../../Todo/redux/actions';
import todoCard from '../../Todo/types';

const onDeleteRow = (dispatch, newOrder) => (index) => {
  const arrayCopy = [...newOrder];
  arrayCopy.splice(index, 1);
  dispatch(deleteNewOrderTableData(arrayCopy));
};

class ECommerceDashboard extends Component {
  static propTypes = {
    newOrder: NewOrderTableProps.isRequired,
    dispatch: PropTypes.func.isRequired,
    rtl: RTLProps.isRequired,
    t: PropTypes.func.isRequired,
    fetchTodoListData: PropTypes.func.isRequired,
    editTodoElement: PropTypes.func.isRequired,
    todoElements: PropTypes.arrayOf(todoCard).isRequired,
  };

  componentDidMount() {
    if (this.props.todoElements.length === 0) { // You can delete it if you need
      this.props.fetchTodoListData();
    }
  }

  render() {
    const {
      t, newOrder, rtl, dispatch, todoElements,
    } = this.props;
    return (
      <Container className="dashboard">
        <Row>
          <Col md={12}>
            <h3 className="page-title">{t('dashboard_commerce.page_title')}</h3>
          </Col>
        </Row>
        <Row>
          <TotalProducts />
          <TotalProfit />
          <OrdersToday />
          <Subscriptions />
        </Row>
        <Row>
          <ProductSales rtl={rtl.direction} />
          <BasicCard />
          <SalesStatistic />
          <MyTodos
            todoElements={todoElements}
            editTodoElement={this.props.editTodoElement}
          />
          <SalesStatistisBar />
          <SalesReport />
          <Emails />
          <ShortReminders />
          <TopSellingProducts dir={rtl.direction} />
          <NewOrders newOrder={newOrder} onDeleteRow={onDeleteRow(dispatch, newOrder)} />
          <RecentOrders />
        </Row>
      </Container>
    );
  }
}

const mapStateToProps = (state) => {
  const todoElements = state.todo && state.todo.data && state.todo.data.elements
  && state.todo.data.elements.length > 0 ? [...state.todo.data.elements] : [];
  return ({
    newOrder: state.newOrder.items,
    todoElements,
    rtl: state.rtl,
  });
};

const mapDispatchToProps = {
  fetchTodoListData,
  editTodoElement,
};

export default withTranslation('common')(connect(mapStateToProps, mapDispatchToProps)(ECommerceDashboard));
