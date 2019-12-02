import React from "react";
import { Link } from "@reach/router";
import styled from "styled-components";
import {
  Grid,
  Card,
  CardHeader,
  CardContent,
  Typography
} from "@material-ui/core";
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
  height: 20em;
  width: 20em;
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
    >
      <StyledCard raised>
        <CardHeader
          title={
            <>
              <Typography variant="h5">{title}</Typography>
              <Typography variant="subtitle1">By: {authors}</Typography>
            </>
          }
        />
        <CardContent>
          <img src={thumbnail} alt="Book Cover" width="130" height="130" />

          <Typography variant="body1">ISBN10: {isbn10}</Typography>
          <Typography variant="body1">ISBN13: {isbn13}</Typography>
        </CardContent>
      </StyledCard>
    </Link>
  </Grid>
);

export default BookCard;
