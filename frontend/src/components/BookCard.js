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
    id: string,
    isbN10: String,
    isbN13: String,
    authors: [string]
  }
};

const BookCard = ({
  listing: { title, id, isbN10, isbN13, thumbnail, authors }
}: Props) => (
  <Grid item>
    {/* //$FlowFixMe */}
    <Link
      to={`/listing/new/${buildQuery({
        title,
        isbN10,
        isbN13,
        thumbnail,
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
          <Typography variant="body1">ISBN10: {isbN10}</Typography>
          <Typography variant="body1">ISBN13: {isbN13}</Typography>
        </CardContent>
      </Card>
    </Link>
  </Grid>
);

export default BookCard;
