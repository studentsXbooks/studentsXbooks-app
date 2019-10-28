import React, { useState } from "react";
/* $FlowFixMe */
import styled from "styled-components";
import { isNil } from "ramda";
import { fade, makeStyles } from "@material-ui/core/styles";

const HeaderOne = styled.div`
  text-align: center;
  color: #707070;
`;

const BlueScene = styled.div`
  text-align: center;
  background-color: #7af4ff;
  color: #b36b39;
  padding 10px;
`;

export default () => (
  <div>
    <HeaderOne>
      <h1>What our site does to help students</h1>
      <p>
        We want to improve the way students can purchase their textbooks for
        their classes, as well as allow students an easier way to offload books
        they no longer need or want to keep around. We want our site to make it
        easier and more cost effective for students so that they can make their
        dreams a reality in the future.
      </p>
    </HeaderOne>
    <BlueScene>
      <h1>Our story and who we are</h1>
      <p>
        {" "}
        We are a group of software engineering students from West Virginia
        University at Parkersburg who created Studentxbooks originally as a
        senior project for our class, but now that project has evolved and is
        now a site that aims to help students out on dealing with the ever
        rising price of textbooks.
      </p>
      <p>The Team</p>
      <p>
        Levi Butcher - Project Manager <br></br> Sean Rickard - Developer{" "}
        <br></br> Jeremi Swann - Developer <br></br> Brady Starcher -
        Documenter/Designer <br></br> Samantha Burkey - Tester/Designer
      </p>
    </BlueScene>
    <HeaderOne>
      <h1>Our College!</h1>
      <h2>About the college:</h2>
      <p>
        Our college is West Virginia University at Parkersburg<br></br> The
        current president of the college is Chris Gilmer<br></br> The Software
        Engineering Professors that have lead the way in our learning are
        Charles Almond and Gary Thompson
      </p>
    </HeaderOne>
  </div>
);
