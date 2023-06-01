const fetchDataReducer = (state, action) => { 
    switch(action.type){
        case "INIT":
            return {
                isLoading: true,
                isError: false
            };
        case "SUCCESS":
            return {
                isLoading: false,
                isError: false,
            }
        case "FAILURE":
            return {
                isLoading: false,
                isError: true,
                errorMessage: action.payload
            }
        default:
            throw new Error();
    }
  }


export default fetchDataReducer;