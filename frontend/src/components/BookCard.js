import React from "react";
import { Link } from "@reach/router";
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
    isbn13: String,
    isbn10: String,
    authors: [string]
  }
};

const BookCard = ({
  listing: { title, description, id, isbn10, isbn13, thumbnail, authors }
}: Props) => (
  <Grid item>
    {/* //$FlowFixMe */}
    <Link
      to={`/listing/new/${buildQuery({
        title,
        isbn10,
        isbn13,
        thumbnail,
        description,
        authors
      })}`}
    >
      <Card raised>
        <CardHeader
          title={
            <>
              <Typography variant="h5">{title}</Typography>
              <Typography variant="subtitle1">
                By: {authors.join(",")}
              </Typography>
            </>
          }
        />
        <CardContent>
          <img src={thumbnail} alt="Book Cover" width="130" height="130" />
          <Typography variant="body1">ISBN10: {isbn10}</Typography>
          <Typography variant="body1">ISBN13: {isbn13}</Typography>
        </CardContent>
      </Card>
    </Link>
  </Grid>
);

export default BookCard;
