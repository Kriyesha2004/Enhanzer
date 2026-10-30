
export interface LoginRequest {
    email: string;
    password: string;
}

export interface Location {
    Location_Code: string;
    Location_Name: string;
}

export interface LoginResponse {
    message: string;
    email: string;
    userName: string;
    locationsSaved: number;
    locations: {
        Location_Code: string;
        Location_Name: string;
    }[];
}
