import React, { useState } from "react";
// $FlowFixMe
import styled from "styled-components";

import {
  ExpansionPanel,
  ExpansionPanelSummary,
  ExpansionPanelDetails,
  Typography
} from "@material-ui/core/";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";

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
  color: #4bb;
`;

const SubHeader = styled.div`
  color: #707070;
`;

const BlueScene = styled.div`
  text-align: center;
  background-color: #efefef;
  color: #333;
  border-top: 1px solid #bbb;
  border-bottom: 1px solid #bbb;
  padding: 10px;
  margin: 1.5em auto;
  width: 50%;
  /* box-shadow: inset 0 0 2px 2px; */
`;

const BlueOverallLayout = styled.div`
  display: grid;
  grid-template-columns: 90%;
  grid-template-rows: 90%;
  grid-row-gap: 5px;
  justify-items: center;
  align-items: center;
`;

const BlueScenePosition = styled.div`
  /* display: grid;
  grid-row-start: 1;
  grid-column-end: 3;
  align-items: center; */
  margin: 0 auto;
  width: 550px;
`;

const BlueSceneLayoutHeader = styled.h2`
  text-align: left;
  color: #4bb;
`;

const BlueSceneLayoutText = styled.p`
  text-align: left;
`;

const DevLinksPosition = styled.div`
  display: grid;
  grid-row-start: 1;
  grid-column-end: 2;
  align-items: center;
`;

// const TextPosition = styled.div`
//   display: grid;
//   grid-row-start: 4;
//   grid-column-end: 1;
//   align-items: center;
// `;

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
        <h2>FAQ</h2>
      </FAQ>
      <ExpansionPanel style={{ margin: "0 auto", width: "50%" }}>
        <ExpansionPanelSummary
          expandIcon={<ExpandMoreIcon />}
          aria-controls="panel1a-content"
          id="panel1a-header"
        >
          <Typography>
            How does the site gather the final price for a selling/buying of a
            book?
          </Typography>
        </ExpansionPanelSummary>
        <ExpansionPanelDetails>
          <Typography>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse
            malesuada lacus ex, sit amet blandit leo lobortis eget.
          </Typography>
        </ExpansionPanelDetails>
      </ExpansionPanel>
      <ExpansionPanel style={{ margin: "0 auto", width: "50%" }}>
        <ExpansionPanelSummary
          expandIcon={<ExpandMoreIcon />}
          aria-controls="panel1a-content"
          id="panel1a-header"
        >
          <Typography>
            Can I buy a book while one of my other books is still on sale?
          </Typography>
        </ExpansionPanelSummary>
        <ExpansionPanelDetails>
          <Typography>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse
            malesuada lacus ex, sit amet blandit leo lobortis eget.
          </Typography>
        </ExpansionPanelDetails>
      </ExpansionPanel>
    </SubHeader>
    <br></br>
    <BlueScene>
      <BlueOverallLayout>
        <BlueScenePosition>
          <BlueSceneLayoutHeader>
            Developer Contacts/Helpful Links
          </BlueSceneLayoutHeader>
          <BlueSceneLayoutText>
            If you have you have issues with the site you can contact us through
            our email:{" "}
            <a style={{ color: "#F93", fontWeight: "bold" }}>
              studentxbooks@gmail.com
            </a>
          </BlueSceneLayoutText>
          <BlueSceneLayoutText>
            For bugs, errors, or to give suggestions for improvement on our site
            you can create an issue at our GitHub repo: <t></t>
            <a
              href="https://github.com/studentsXbooks"
              style={{ color: "#220B8F" }}
            >
              https://github.com/studentsXbooks
            </a>
          </BlueSceneLayoutText>
        </BlueScenePosition>
      </BlueOverallLayout>
    </BlueScene>
  </div>
);
