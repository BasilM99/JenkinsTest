
import { BasicNotification,showBasicNotificationForAdFalcon } from '../Components/Notification';
import {MessageType} from '../Config/Enums';
const AdFalconNotificationsUtil =({Mesessges, Title })=>{



     if(Mesessges &&Mesessges.length>0)
    {
     Mesessges.forEach(function(entry) {
      // console.log(entry);

      if(entry.Type == MessageType.Success)
      {

    
    
       showBasicNotificationForAdFalcon(Title,entry.Message,"success","right-up");
      }
      if(entry.Type == MessageType.Error)
      {

    
    
       showBasicNotificationForAdFalcon(Title,entry.Message,"danger","right-up");
      }
      if(entry.Type == MessageType.Warning)
      {

    
    
       showBasicNotificationForAdFalcon(Title,entry.Message,"warning","right-up");
      }

      if(entry.Type == MessageType.Information)
      {

    
    
       showBasicNotificationForAdFalcon(Title,entry.Message,"","right-up");
      }

   }); 

}
}


export default AdFalconNotificationsUtil