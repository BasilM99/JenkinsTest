import React ,{useEffect, useState, useRef} from 'react';
import CloseCircleIcon from "mdi-react/CloseCircleIcon";

const TimePicker = ({ value, onChange })=>{
    const hours = [...Array(24).keys()];
    const minutes = [0,15,30,45];
    const firstRender = useRef(true);
    const [time,setTime]= useState(value);
    const onTimeChange = (hours, mins)=> {
     if (isNaN(hours) && isNaN(mins)){
         setTime(null);
         return;
     }
     if (isNaN(hours))
        hours = 0;
     if (isNaN(mins))
        mins = 0;
     var date = new Date();
     date.setHours(hours, mins, 0);
     setTime(date);
   }
    
   useEffect(()=>{
     if(firstRender.current){
       firstRender.current = false;
       return;
     }
     onChange && onChange(time);
   },[time]);
   
   useEffect(()=>{
      setTime(value);
   },[value]);
   
    return(
     <>
     <select name="hours"  style={{width:'60px'}} value={time?.getHours() ?? ""} onChange ={ e => onTimeChange(e.target.value,time?.getMinutes() )}>
       <option value=""></option>
       {
           hours.map(item=> <option value={item}>{item.toString().padStart(2, '0')}</option>)
       }
     </select >
     <h3>:</h3>
      <select name="minutes" style={{width:'60px'}} value={time?.getMinutes()?? ""} onChange ={e => onTimeChange(time?.getHours(), e.target.value)}>
      <option value=""></option>
        {
            minutes.map(item=> <option value={item}>{item.toString().padStart(2, '0')}</option>)
        }
     </select>
     {
       time &&
        <button 
          type="button"
          className="form__form-group-button"
          onClick={e => setTime(null)}
          >
         <CloseCircleIcon />
        </button> 
        }
     </>
    );
 
 }

 export default TimePicker;
 