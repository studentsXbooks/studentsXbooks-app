// @flow

import React, { useState } from "react";
// $FlowFixMe
import styled from "styled-components";
import withSearchBar from "../components/withSearchBar";

const HeaderOne = styled.div`
  text-align: center;
  color: #707070;
`;

const TimelineLayout = styled.div`
  display: grid;
  grid-template-row: auto;
  grid-template-cols: 50% 50%;
  grid-row-gap: 60px;
  justify-items: center;
  align-items: center;
`;

const TimelineBox = styled.div`
  display: grid;
  grid-template-rows: auto;
  grid-template-columns: 1fr 300px;
  grid-gap: 60px;
  align-items: center;
`;

const BlueScene = styled.div`
  text-align: center;
  background-color: #efefef;
  color: #333;
  border-top: 1px solid #bbb;
  border-bottom: 1px solid #bbb;
  padding: 10px;
  margin: 1.5em auto;
`;

const BlueSceneLayoutHeader = styled.h2`
  color: #4bb;
`;

export default () => (
  <div>
    <HeaderOne>
      <div>
        <h1>Sell or Trade your unneeded textbooks with us!</h1>
        <h2>Want to know how to start the process? Look no further!</h2>
      </div>
      <TimelineLayout>
        <TimelineBox>
          <img
            src="./Book-titles-sell.jpg"
            style={{
              paddingRight: "60px",
              height: "300px"
            }}
          />
          <div>
            <div>
              <p>
                To sell a book, you must first log into the site, then hover
                over your username in the top right, and click the "Create a
                Listing" link when it appears.
              </p>
            </div>
            <div>
              <p>
                On the new page you can then input the information about your
                book
              </p>
            </div>
            <div>
              <p>
                This information will include: the name of book, author, ISBN,
                etc. This will also have your payment options you can select
                from
              </p>
            </div>
            <div>
              <p>
                Finally, after clicking the Post Listing button, your book will
                be seen by all potential buyers.
              </p>
            </div>
          </div>
        </TimelineBox>
      </TimelineLayout>
      <div>
        <h3>Post your books now!</h3>
      </div>
    </HeaderOne>
    <BlueScene>
      <div>
        <BlueSceneLayoutHeader>
          Advantages of using StudentXbooks
        </BlueSceneLayoutHeader>
        <p>
          {" "}
          If you're looking to buy textbooks, you can buy find them on our site
          much cheaper than alternative sources. If you have an unneeded book
          from a previous semester you sell it for a much better deal than you
          might get from the college bookstore. Additionally, unlike other
          ecommerce sites, there are no fees for listing, selling, or purchasing
          books on this site. Tuition is already expensive. We're here to help
          students make it through college without paying outrageous prices for
          textbooks as well.
        </p>
      </div>
    </BlueScene>
  </div>
);
