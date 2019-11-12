import React, { useState } from "react";
/* $FlowFixMe */
import styled from "styled-components";
import { isNil } from "ramda";
import { fade, makeStyles } from "@material-ui/core/styles";

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

const BulletPoint = styled.p`
  text-align: left;
  width: 50%;
  margin: 0 auto;
  padding: 5px;
`;

const BulletPointHeader = styled.h2`
  text-align: left;
  width: 50%;
  margin: 0 auto;
`;

const BlueScene = styled.div`
  text-align: center;
  background-color: #33578c;
  color: #76ecf7;
  padding 10px;
  margin: auto;
  box-shadow: inset 0 0 2px 2px;
`;

export default () => (
  <div>
    <HeaderOne>
      <h1>What our site does to help students</h1>
      <TextStyle>
        We want to improve the way students can purchase textbooks for their
        classes and allow students an easier way to sell books they no longer
        need to keep around. We want our site to make it easier and more cost
        effective for students so that they can make their dreams a reality in
        the future.
      </TextStyle>
    </HeaderOne>
    <BlueScene>
      <h1>Our story and who we are</h1>
      <TextStyle>
        {" "}
        We are a group of software engineering students from West Virginia
        University at Parkersburg who created Studentxbooks originally as a
        senior project for our class, but now that project has evolved and is
        now a site that aims to help students out on dealing with the ever
        rising price of textbooks.
      </TextStyle>
      <h3>The Team</h3>
      <p>
        Levi Butcher - Project Manager <br></br> Sean Rickard - Developer{" "}
        <br></br> Jeremi Swann - Developer <br></br> Brady Starcher -
        Documenter/Designer <br></br> Samantha Burkey - Tester/Designer<br></br>{" "}
        Ethan Rhodes - Developer/Editor
      </p>
    </BlueScene>
    <HeaderOne>
      <h1>Our College!</h1>
      <BulletPointHeader>About the college:</BulletPointHeader>
      <BulletPoint>
        · Our college is West Virginia University at Parkersburg<br></br> · The
        current president of the college is Chris Gilmer<br></br> · The Software
        Engineering Professors that have led the way in our learning are Charles
        Almond and Gary Thompson
      </BulletPoint>
    </HeaderOne>
  </div>
);
