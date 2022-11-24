import   React from 'react';
import  { useContext,useState,useEffect,useRef} from 'react';
import { Col, Container, Row,Label, Card, CardBody   , Button, ButtonToolbar,ButtonGroup} from 'reactstrap';
const MetricComponent = (props) =>{

const renderButtons = () =>{
    var tempList  = [];
    const [activeCode, setactiveCode] = useState('');
    useEffect(() => {
        setactiveCode(props.activeCode);
    },[props.activeCode]);
    
    if(typeof(props) != "undefined")
    return props.metricItems.map((x,index) => {

           
             
               
               return <Button   outline size="sm"  color="danger"  active={x.Code=== activeCode}  name={x.Name.GroupKey} customtext={x.CustomName}   value={x.Code} onClick={() => props.CallBackParent(x.Code,x.Name.Value)}>{x.Name.Value}</Button>
            
         
        });
    return tempList;
}


return<>
  
    {renderButtons()}
</>

}

export default MetricComponent;