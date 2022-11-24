import * as React from 'react'

import {
    Chart,
    ChartTitle,
    ChartSeries,
    ChartSeriesItem,
    ChartCategoryAxis,
    ChartCategoryAxisItem
} from '@progress/kendo-react-charts';

//const categories = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'];

const ChartContainer   = ({props}) => {

    
        return(
            <Chart>
                <ChartTitle text={props.mainTitle} align="center" />
                <ChartCategoryAxis>
                    <ChartCategoryAxisItem  labels={{ format: 'd', /*rotation: 'auto',*/step:props.step,rotation: props.rotation}}   st title={{ text: props.categoriesTitle }} categories={props.categories} />
                </ChartCategoryAxis>
                <ChartSeries>
                    <ChartSeriesItem type="line" data={props.chartData} />
                </ChartSeries>
            </Chart>
        )
    
}

export default ChartContainer;

// ReactDOM.render(
//   <ChartContainer />,
//   document.querySelector('chart')
// );
