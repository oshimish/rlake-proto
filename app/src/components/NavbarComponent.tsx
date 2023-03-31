import React from "react";
import logo from "../logo.svg";
import {
    Navbar
} from "reactstrap";


import SearchForm from "./SearchForm";
import UploadForm from "./UploadForm";
import { Link } from "react-router-dom";

const NavbarComponent: React.FC = () => {
    return (
        <Navbar color="light" light expand="md" >
            <Link to="/" className='navbar-brand'>
                <img alt="logo" src={logo} style={{ height: 40, width: 40 }} />
                ChatGPT Map
            </Link>

            <SearchForm />
            <UploadForm />
        </Navbar >
    );
};

export default NavbarComponent;
