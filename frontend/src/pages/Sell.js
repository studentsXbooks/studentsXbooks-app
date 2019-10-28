// @flow

import React, { useState } from "react";
// $FlowFixMe
import styled from "styled-components";

const HeaderOne = styled.div`
  text-align: center;
  color: #707070;
`;

const TimelineLayout = styled.div`
  display: grid;
  grid-template-row: auto;
  grid-template-cols: 50% 50%;
  justify-items: center;
  align-items: center;
`;

const TimelineBox = styled.div`
  width: 300px;
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
      <div>
        <h1>Sell or Trade your unneeded textbooks with us!</h1>
        <h2>Want to know how to start the process? Look no further!</h2>
      </div>
      <TimelineLayout>
        <div>
          <TimelineBox>
            <div>
              <p>
                To sell a book, you must first log into the site, then go to the
                Create A Listing link found under your username when it is
                hovered over
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
                Finally after clicking the Post Listing button, you are all set
                to have your book shown for sale or trade on the site!
              </p>
            </div>
          </TimelineBox>
        </div>
      </TimelineLayout>
      <div>
        <h3>Post your books now!</h3>
      </div>
    </HeaderOne>
    <BlueScene>
      <div>
        <h2>Advantages of using StudenTXTbooks</h2>
        <p>
          {" "}
          Through our site you can buy books much cheaper than what your college
          would sell you through their company backed online stores or through
          their on campus bookstores. If you have an unneeded book from a
          previous semester or year then you can sell the book on this site for
          a better deal than what a college would buy it for through their on
          campus bookstore. Also, unlike other ecommerce sites, there are no
          fees for listing, selling, or purchasing books on this site. We are
          here to help students make it through college without paying the
          outrageous prices on textbooks when the tuition alone is already
          expensive enough.
        </p>
      </div>
    </BlueScene>
  </div>
);
