import { NswagClient } from "./nswag";

const basePath = process.env.REACT_APP_API_BASE;

class ApiClient extends NswagClient {
   
    constructor() {
        super(basePath)
    }

}

const  Api = new ApiClient();

export default Api;