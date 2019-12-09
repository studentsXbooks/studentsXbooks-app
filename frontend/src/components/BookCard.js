import React from "react";
import { Link } from "@reach/router";
import styled from "styled-components";
import { Grid, Card, Typography } from "@material-ui/core";
import ImageWithFallback from "./ImageWithFallback";
import buildQuery from "../utils/buildQuery";

type Props = {
  listing: {
    title: string,
    thumbnail: string,
    description: string,
    id: string,
    isbn13: string,
    isbn10: string,
    authors: [string]
  }
};

const StyledCard = styled(Card)`
  position: relative;
  display: grid;
  grid-template: "thumbnail about" auto / auto minmax(200px, 350px);
  grid-gap: 2rem;
  padding: 2rem;
  margin: 1rem;
  min-height: 200px;
  max-height: 200px;
  overflow: hidden;

  & img {
    height: auto;
    justify-self: center;
  }
  & * {
    text-overflow: ellipsis;
  }
`;

const BookCard = ({
  listing: { title, description, isbn10, isbn13, thumbnail, authors }
}: Props) => (
  <Grid item>
    {/* //$FlowFixMe */}
    <Link
      to={`/listing/new/${buildQuery({
        title,
        isbn10,
        isbn13,
        thumbnail: encodeURIComponent(thumbnail),
        description,
        authors
      })}`}
      style={{ textDecoration: "none" }}
    >
      <StyledCard raised>
        <div style={{ gridArea: "thumbnail" }}>
          <ImageWithFallback src={thumbnail} alt="Book Cover" />
        </div>
        <div style={{ gridArea: "about" }}>
          <Typography
            variant="h4"
            style={{
              textOverflow: "ellipsis",
              overflow: "hidden",
              whiteSpace: "nowrap"
            }}
            title={title}
          >
            {title}
          </Typography>
          <Typography variant="subtitle1">By: {authors}</Typography>
          <br />
          <Typography variant="body1">ISBN10: {isbn10}</Typography>
          <Typography variant="body1">ISBN13: {isbn13}</Typography>
        </div>
      </StyledCard>
    </Link>
  </Grid>
);

export default BookCard;
