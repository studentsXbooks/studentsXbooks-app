import React, { useState } from "react";
// $FlowFixMe
import styled from "styled-components";

const HeaderOne = styled.div`
  text-align: center;
  color: #707070;
`;

const TextStyle = styled.p`
  text-align: center;
  width: 50%;
  margin: 0 auto;
  padding: 20px;
`;

const QBorder = styled.p`
  text-align: center;
  margin: 0 auto;
  padding: 10px;
  border: solid;
  border-width: thin;
  box-shadow: 1px 1px;
  width: 50%;
`;

const FAQ = styled.div`
  text-align: left;
  width: 50%;
  margin: 0 auto;
  padding: 20px 0px 0px 0px;
  color: #707070;
`;

const SubHeader = styled.div`
  color: #707070;
`;

export default () => (
  <div>
    <HeaderOne>
      <h1>
        Need help with navigating the site?<br></br> Have a bug that needs to be
        reported?<br></br> Look no further!
      </h1>
    </HeaderOne>
    <SubHeader>
      <FAQ>
        <p>FAQ</p>
      </FAQ>
      <QBorder>
        How does the site gather the final price for a selling/buying of a book?
      </QBorder>
      <br></br>
      <QBorder>
        Can I buy a book while one of my other books is still on sale?
      </QBorder>
    </SubHeader>
    <br></br>
    <HeaderOne>
      <h3>Developer Contacts/Helpful Links</h3>
      <p>
        If you have you have issues with the site you can contact us through our
        email: <h4>Studentxbooks@gmail.com</h4>
      </p>
      <p>
        For bugs, errors, or to give suggestions for improvement on our site you
        can create an issue at our GitHub repo: <t></t>
        <a href="https://github.com/studentsXbooks">
          https://github.com/studentsXbooks
        </a>
      </p>
    </HeaderOne>
  </div>
);
