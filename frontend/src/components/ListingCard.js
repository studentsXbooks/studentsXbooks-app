import React from "react";
import { Link } from "@reach/router";
import { Grid, Card, Typography } from "@material-ui/core";
import styled from "styled-components";
import ImageWithFallback from "./ImageWithFallback";

type Props = {
  listing: {
    title: string,
    price: string,
    id: string,
    isbn10: String,
    isbn13: String,
    condition: string,
    authors: string,
    thumbnail: string
  }
};

const StyledCard = styled(Card)`
  position: relative;
  display: grid;
  grid-template:
    "thumbnail" auto
    "about"/ 1fr;
  width: 400px;
  grid-gap: 1rem;
  padding: 1rem;
  overflow: visible !important;
  margin: 1rem;

  & img {
    width: 100%;
  }

  & *[data-price] {
    color: white;
    position: absolute;
    right: -25px;
    top: -25px;
    z-index: 10;
    background: #3f51b5;
    padding: 0 0.5rem;
    * {
      font-size: 2rem;
    }
  }
`;

const ExtraInfoLayout = styled.div`
  padding-top: 1rem;
  display: grid;
  grid-template-columns: 1fr 1fr;

  & > *:last-child {
    justify-self: flex-end;
    align-self: flex-end;
  }
`;

const ListingCard = ({
  listing: { title, price, id, isbn10, isbn13, condition, authors, thumbnail }
}: Props) => (
  <Grid item>
    {/* //$FlowFixMe */}
    <Link to={`/listing/${id}`} style={{ textDecoration: "none" }}>
      <StyledCard raised>
        <div style={{ gridArea: "thumbnail" }}>
          <ImageWithFallback src={thumbnail || ""} alt={`${title}`} />
        </div>
        <div style={{ gridArea: "about" }}>
          <Typography variant="h4">{title}</Typography>
          <Typography variant="subtitle1">By: {authors}</Typography>
          <ExtraInfoLayout>
            <div>
              <Typography variant="body1">ISBN10: {isbn10 || ""}</Typography>
              <Typography variant="body1">ISBN13: {isbn13 || ""}</Typography>
            </div>
            <Typography variant="body1">{condition}</Typography>
          </ExtraInfoLayout>
          <div data-price>
            <Typography>$ {price}</Typography>
          </div>
        </div>
      </StyledCard>
    </Link>
  </Grid>
);

export default ListingCard;
