import { createContext, useContext } from "react";
import { ApiService } from "../features/TaskManagement/services/apiService";

export const ApiServiceContext = createContext<ApiService | null>(null);

export const ApiServiceProvider = ({ children }: { children: React.ReactNode }) => {
    const apiService = new ApiService();
    return <ApiServiceContext.Provider value={apiService}>{children}</ApiServiceContext.Provider>;
};

export const useApiService = () => {
    const apiService = useContext(ApiServiceContext);
    if (!apiService) {
        throw new Error('ApiServiceContext not found');
    }
    return apiService;
};

