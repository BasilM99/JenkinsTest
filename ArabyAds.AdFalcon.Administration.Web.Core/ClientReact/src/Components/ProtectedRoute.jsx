import React from "React"

import {Route,Redirect } from "react-router-dom"


function ProtectedRoute({
    component:Component,
logout:logout,
isAuthnicated:isAuthnicated,
...rest
}
)
{
return (
<Route
{...rest}
render={(props)=>{
if(isAuthnicated)
{
return <Component  logout={logout}></Component>

}
else{

<Redirect to={{pathname:"/User/Login", state:{from:props.location} } }></Redirect>

}

}}

></Route>


)


}

export default ProtectedRoute;