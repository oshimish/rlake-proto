import React from 'react';
import {AzureMap, IAzureMapOptions} from 'react-azure-maps'
import {AuthenticationType} from 'azure-maps-control'

const option: IAzureMapOptions = {
    authOptions: {
        authType: AuthenticationType.subscriptionKey,
        subscriptionKey: process.env.REACT_APP_MAPS_KEY ?? '' // Your subscription key
    },
}


function Map() {
    return (
        <AzureMap options={option} />
    )
}


export default Map;