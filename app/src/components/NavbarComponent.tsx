import React from "react";
import logo from "../logo.svg";
import {
    Navbar,
    NavbarBrand
} from "reactstrap";


import SearchForm from "./SearchForm";
import UploadForm from "./UploadForm";

const NavbarComponent: React.FC = () => {
    return (
        <Navbar color="light" light expand="md" >
            <NavbarBrand href="/" src={logo} className=''>
                <img alt="logo" src={logo} style={{ height: 40, width: 40 }} />
                ChatGPT Maps
            </NavbarBrand>

            <SearchForm />
            <UploadForm />
        </Navbar >
    );
};

export default NavbarComponent;
